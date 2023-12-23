using System.Diagnostics;
using System.Security.Claims;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.iRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Stripe;
using Stripe.Checkout;


namespace Bulky_Web.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize]
public class CartController : Controller
{
    //write the constructor for this controller
    private readonly iShoppingCartRepository _cartRepo;
    private readonly ShoppingCartVM _cartVM;
    private readonly iApplicationUserRepository _userRepo;
    private readonly iOrderHeaderRepository _orderHeaderRepo;
    private readonly iOrderDetailRepository _orderDetailRepo;

    public CartController(iShoppingCartRepository cartRepo, iApplicationUserRepository userRepo,
        iOrderHeaderRepository orderHeaderRepo, iOrderDetailRepository orderDetailRepo)
    {
        _cartRepo = cartRepo;
        _userRepo = userRepo;
        _orderHeaderRepo = orderHeaderRepo;
        _orderDetailRepo = orderDetailRepo;
    }

    public IActionResult Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var user_id = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        ShoppingCartVM cartVM = new()
        {
            ListCart = _cartRepo.GetAll(u => u.ApplicationUserId == user_id, includeProperties: "Product"),
            OrderHeader = new()
        };

        //calculate the total order
        foreach (var cart in cartVM.ListCart)
        {
            cartVM.OrderHeader.OrderTotal += cart.Count * cart.Product.Price;
        }


        return View(cartVM);
    }

    public IActionResult Summary()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var user_id = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        ShoppingCartVM cartVM = new()
        {
            ListCart = _cartRepo.GetAll(u => u.ApplicationUserId == user_id, includeProperties: "Product"),
            OrderHeader = new()
        };

        //calculate the total order
        foreach (var cart in cartVM.ListCart)
        {
            cartVM.OrderHeader.OrderTotal += cart.Count * cart.Product.Price;
        }


        return View(cartVM);
    }

    [HttpPost]
    [ActionName("Summary")]
    public IActionResult SummaryPost(ShoppingCartVM vm)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var user_id = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        
        vm.OrderHeader.ApplicationUserId = user_id;

        vm.ListCart = _cartRepo.GetAll(u => u.ApplicationUserId == vm.OrderHeader.ApplicationUserId,
            includeProperties: "Product");


        
        vm.OrderHeader.OrderDate = DateTime.Now;

        vm.OrderHeader.ApplicationUser = _userRepo.Get(u => u.Id == user_id);


        foreach (var cart in vm.ListCart)
        {
            vm.OrderHeader.OrderTotal += cart.Count * cart.Product.Price;
        }

        if (vm.OrderHeader.ApplicationUser.CompanyID.GetValueOrDefault() == 0)
        {
            //regular customer
            vm.OrderHeader.OrderStatus = SD.StatusPending;
            vm.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
        }
        else
        {
            //company customer
            vm.OrderHeader.OrderStatus = SD.StatusApproved;
            vm.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
        }

        _orderHeaderRepo.Add(vm.OrderHeader);
        _orderHeaderRepo.Save();

        //fill the order details
        foreach (var cart in vm.ListCart)
        {
            OrderDetails orderDetails = new()
            {
                ProductID= cart.ProductId,
                OrderHeaderId = vm.OrderHeader.Id,
                Price = cart.Product.Price,
                Count = cart.Count
            };
            _orderDetailRepo.Add(orderDetails);
        }
        
        _orderDetailRepo.Save();
        
        if (vm.OrderHeader.ApplicationUser.CompanyID.GetValueOrDefault() == 0)
        {
            //regular customer
            //stripe logic
            //pay and you will get orderconfirmation 
            //check if environment is development
            var domain = "https://localhost:7241";

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                 domain = "movieflix.azurewebsites.net";

            }
             
            
            
          
            
           
           
            
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"/customer/cart/orderconfirmation?orderheaderid={vm.OrderHeader.Id}",
                CancelUrl = domain + "/customer/cart/index", 
                LineItems = new List<SessionLineItemOptions>()
                {
                },
                Mode="payment",
                
            };

            foreach (var item in vm.ListCart)
            {
                var sessionLineItem= new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long?) (item.Product.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    },
                    Quantity = item.Count
                    
                };
                options.LineItems.Add(sessionLineItem);
            }
            
            var service = new SessionService();
            Session session = service.Create(options);
            
            _orderHeaderRepo.UpdateStripePaymentID(vm.OrderHeader.Id, session.Id,session.PaymentIntentId);
            //paymentintendid is null because payment is not successful is yet
            _orderDetailRepo.Save();
            
            Response.Headers.Add("Location", session.Url);//redirect to stripe payment page
            return new StatusCodeResult(303);
            
        }


        return RedirectToAction("orderconfirmation",new{orderheaderid = vm.OrderHeader.Id});
    }

    public IActionResult orderconfirmation(int orderheaderid)
    {
        OrderHeader orderHeader = _orderHeaderRepo.Get(u=>u.Id==orderheaderid,includeProperties:"ApplicationUser");

        if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
        {
            //this is not company order 
            var service = new SessionService();

            Session session = service.Get(orderHeader.SessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _orderHeaderRepo.UpdateStripePaymentID(orderheaderid, session.Id, session.PaymentIntentId);
                _orderHeaderRepo.UpdateStatus(orderheaderid, SD.StatusApproved, SD.PaymentStatusApproved);
                _orderHeaderRepo.Save();
                
            }
            
            

        }
        List<ShoppingCart> carts = _cartRepo.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
        _cartRepo.RemoveRange(carts);
        
        _cartRepo.Save(); //clearing carts after payment
        
        return View(orderheaderid);
    }

    public IActionResult Plus(int cartId)
    {
        var cart = _cartRepo.Get(u => u.Id == cartId);
        cart.Count++;
        _cartRepo.Update(cart);
        _cartRepo.Save();
        return RedirectToAction("Index");
    }

    public IActionResult Minus(int cartId)
    {
        var cart = _cartRepo.Get(u => u.Id == cartId);
        cart.Count--;
        _cartRepo.Update(cart);
        _cartRepo.Save();
        return RedirectToAction("Index");
    }

    public IActionResult Remove(int cartId)
    {
        var cart = _cartRepo.Get(u => u.Id == cartId);
        _cartRepo.Remove(cart);
        _cartRepo.Save();
        return RedirectToAction("Index"); 
    }
}
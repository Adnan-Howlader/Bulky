using System.Security.Claims;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.iRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace Bulky_Web.Areas.Admin.Controllers;

[Area("Admin")]
public class OrderController: Controller

{
    //create constructor for this controller
    private readonly iOrderHeaderRepository _orderHeaderRepo;
    private readonly iOrderDetailRepository _orderDetailRepo;
    private readonly iProductRepository _productRepo;
    
    [BindProperty]
    public  OrderVM ordervm{get;set;}
    
    public OrderController(iOrderHeaderRepository orderHeaderRepo, iOrderDetailRepository orderDetailRepo, iProductRepository productRepo)
    {
        _orderHeaderRepo = orderHeaderRepo;
        _orderDetailRepo = orderDetailRepo;
        _productRepo = productRepo;
    }

    public IActionResult Index()
    {

        return View();
    }

    public IActionResult Details(int orderId)
    {
       ordervm= new()
        {
            OrderHeader = _orderHeaderRepo.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
            OrderDetails = _orderDetailRepo.GetAll(u => u.OrderHeaderId == orderId,includeProperties: "Product")
          
        };
        return View(ordervm);
        
        
    }
    
    [HttpPost]
    public IActionResult UpdateOrderDetails()
    {
        var orderHeaderFromDb = _orderHeaderRepo.Get(u=> u.Id == ordervm.OrderHeader.Id);
        orderHeaderFromDb.Name = ordervm.OrderHeader.Name;
        orderHeaderFromDb.PhoneNumber = ordervm.OrderHeader.PhoneNumber;
        orderHeaderFromDb.StreetAddress = ordervm.OrderHeader.StreetAddress;
        orderHeaderFromDb.City = ordervm.OrderHeader.City;
        orderHeaderFromDb.State = ordervm.OrderHeader.State;
        orderHeaderFromDb.PostalCode = ordervm.OrderHeader.PostalCode;
        if (!string.IsNullOrEmpty(ordervm.OrderHeader.Carrier)) {
            orderHeaderFromDb.Carrier = ordervm.OrderHeader.Carrier;
        }
        if (!string.IsNullOrEmpty(ordervm.OrderHeader.TrackingNumber)) {
            orderHeaderFromDb.Carrier = ordervm.OrderHeader.TrackingNumber;
        }
        _orderHeaderRepo.Update(orderHeaderFromDb);
        _orderHeaderRepo.Save();
        

        TempData["Success"] = "Order Details Updated Successfully.";


        return RedirectToAction(nameof(Details), new {orderId= orderHeaderFromDb.Id});
     
    }

    [HttpPost]
    public IActionResult StartProcessing()
    {
        var orderHeaderFromDb = _orderHeaderRepo.Get(u=> u.Id == ordervm.OrderHeader.Id);
        orderHeaderFromDb.OrderStatus = SD.StatusInProcess;
        _orderHeaderRepo.Update(orderHeaderFromDb);
        _orderHeaderRepo.Save();
        
        return RedirectToAction(nameof(Details), new {orderId= orderHeaderFromDb.Id});
        
    }
    
    [HttpPost]
    public IActionResult ShipOrder()
    {
        var orderHeaderFromDb = _orderHeaderRepo.Get(u=> u.Id == ordervm.OrderHeader.Id);
        orderHeaderFromDb.OrderStatus = SD.StatusShipped;
        _orderHeaderRepo.Update(orderHeaderFromDb);
        _orderHeaderRepo.Save();
        
        return RedirectToAction(nameof(Details), new {orderId= orderHeaderFromDb.Id});
        
    }
    
    [HttpPost]
    public IActionResult CancelOrder()
    {
        var orderHeaderFromDb = _orderHeaderRepo.Get(u=> u.Id == ordervm.OrderHeader.Id);
        orderHeaderFromDb.OrderStatus = SD.StatusCancelled;
        _orderHeaderRepo.Update(orderHeaderFromDb);
        _orderHeaderRepo.Save();
        
        return RedirectToAction(nameof(Details), new {orderId= orderHeaderFromDb.Id});
        
    }
    
    
    [HttpGet]
    public IActionResult GetAll()
    {
        //if admin or employee
        if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
        {
            List<OrderHeader> orderHeaders = _orderHeaderRepo.GetAll(includeProperties: "ApplicationUser").ToList();
            return Json(new { data = orderHeaders });
        }
        else
        {
            //just application user
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var user_id = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            List<OrderHeader> orderHeaders = _orderHeaderRepo.GetAll(u => u.ApplicationUserId == user_id, includeProperties: "ApplicationUser").ToList();
            return Json(new { data = orderHeaders });
           
            
        }
      
        
       
    }
    
}
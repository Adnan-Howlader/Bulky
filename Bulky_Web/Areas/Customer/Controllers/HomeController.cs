using System.Diagnostics;
using System.Security.Claims;
using Bulky.DataAccess.Repository.iRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

namespace Bulky_Web.Areas.Customer.Controllers;


[Area ("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly iProductRepository _productRepository;
    private readonly iCategoryRepository _categoryRepository;
    private readonly iShoppingCartRepository _shoppingCartRepository;

    public HomeController(ILogger<HomeController> logger,iProductRepository productRepository,iCategoryRepository categoryRepository,iShoppingCartRepository shoppingCartRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _shoppingCartRepository = shoppingCartRepository;
        
    }
   

    public IActionResult Index()//this method is called when the user requests the home page
    {
        IEnumerable<Product> productList = _productRepository.GetAll();
        return View(productList);//index parameter is passed to the view method
    }
    
    public IActionResult Details(int id)//this method is called when the user requests the home page
    {
        //print the id in the console
        Debug.WriteLine(id);
        ShoppingCart cart = new() 
        {
            
            ProductId= id,
            Product = _productRepository.Get(p => p.Id == id)
            
            
        };

        
        
        
       
        //get the category name from category id 
        Category category=_categoryRepository.Get(c=>c.Id==cart.Product.CategoryId);
        
       //make a tuplemodel
        Tuple<ShoppingCart,Category> tuple=new Tuple<ShoppingCart,Category>(cart,category);
        
        //return the tuple model
        return View(cart);//index parameter is passed to the view method
        
       

        
     
    }
    
    //post method
    [HttpPost]
    [Authorize] //must be logged in user
    public IActionResult Details(ShoppingCart cart)
    {
        
        
        
        //get the user id from the session
        var claimsIdentity =(ClaimsIdentity)User.Identity;
        var user_id = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        cart.ApplicationUserId = user_id;
        
        //add to database
        cart.ProductId = cart.Id; //somehow it got replaced
        cart.Id = 0;//0 so that it creates a new id
       
        
         //check if the cart already exists
         ShoppingCart? existingCart = _shoppingCartRepository.Get(u => u.ApplicationUserId == user_id && u.ProductId == cart.ProductId);
         if (existingCart != null)
         {
             existingCart.Count+=cart.Count;
             _shoppingCartRepository.Update(existingCart);
         }
         else
         {
             
             _shoppingCartRepository.Add(cart);
         }
         
         
         
       
        _shoppingCartRepository.Save();
         
        
        
        

        //return to the index
        return RedirectToAction("Index");
    }

    public IActionResult Privacy()//this method is called when the user requests the privacy page
    {
        return View();//privacy parameter is passed to the view method
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
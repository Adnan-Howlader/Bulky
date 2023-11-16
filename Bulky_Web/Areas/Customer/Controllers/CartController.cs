using System.Diagnostics;
using System.Security.Claims;
using Bulky.DataAccess.Repository.iRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

namespace Bulky_Web.Areas.Customer.Controllers;


[Area("Customer")]

[Authorize]
public class CartController: Controller
{
    //write the constructor for this controller
    private readonly iShoppingCartRepository _cartRepo;
    private readonly ShoppingCartVM _cartVM;
    
    public CartController(iShoppingCartRepository cartRepo)
    {
        _cartRepo = cartRepo;
        
    }
  
    public IActionResult Index()
    {
        var claimsIdentity =(ClaimsIdentity)User.Identity;
        var user_id = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        
        ShoppingCartVM cartVM = new()
        {
            ListCart = _cartRepo.GetAll(u=>u.ApplicationUserId==user_id),
            
        };
        
        
        return View();
    }
    
}
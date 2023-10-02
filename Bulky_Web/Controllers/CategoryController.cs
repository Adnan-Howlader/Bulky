using Bulky_Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace Bulky_Web.Controllers;

public class CategoryController : Controller
{
   
    private readonly ApplicationDbContext _db;
    

    public CategoryController(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public IActionResult Index()//action method
    {
        var objList = _db.Categories.ToList(); //type of objlist is list of category 
        return View(objList);
    }
    
    public IActionResult Create()
    {
        return View();//how view knows which view to return??  //create.cshtml
    }
    
    
}
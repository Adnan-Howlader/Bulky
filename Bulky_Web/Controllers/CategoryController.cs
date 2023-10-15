using Bulky_Web.Data;
using Bulky_Web.Models;
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
        return View();//how view knows which view to return??  //it passes the name of the action method in the view
    }

    [HttpPost]//what is this attribute?? //it is an attribute that tells the compiler that this method is a post method
    public IActionResult Create(Category obj)
    {
        //if object category name and display order is same ,add error
        if (obj.Name==obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Name","Category Name and Display Order cannot be same");
        }
        
        
        if (ModelState.IsValid)//what is this?? //it is a property of the controller class that checks if the model is valid or not
        {
            //create a category object
            _db.Categories.Add(obj);//add changes to dbcontext of entity framework 
            _db.SaveChanges();//make all the changes migrate and from migration update the database (internal migration)
            return RedirectToAction("Index","Category");//redirect to the index action method
        }
        
        return View(obj);
            
        

    }
    
    
    
}
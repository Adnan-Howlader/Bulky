using Bulky.DataAccess.Repository.iRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bulky_Web.Areas.Admin.Controllers;

[Area("Admin")]//this attribute tells the compiler that this controller belongs to the admin area
[Authorize(Roles = SD.Role_Admin)]
public class CategoryController : Controller
{

    private readonly iCategoryRepository _categoryRepo;


    public CategoryController(iCategoryRepository categoryRepo)//who calls this constructor?? //it is called by the framework when the controller is called
    {
        _categoryRepo = categoryRepo;
    }

    public IActionResult Index() //action method
    {
        var objList =_categoryRepo.GetAll().ToList(); //type of objlist is list of category 
        return View(objList);
    }

    public IActionResult Create() //get method
    {
        return View(); //how view knows which view to return??  //it passes the name of the action method in the view
    }

    [HttpPost] //what is this attribute?? //it is an attribute that tells the compiler that this method is a post method
    public IActionResult Create(Category obj) //post method
    {
        //if obj id already exists ,add error
        if (_categoryRepo.Get(c => c.Id == obj.Id) != null)
        {
            ModelState.AddModelError("Id", "ID already exists");
        }
      
        //if object category name and display order is same ,add error

        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Name", "Category Name and Display Order cannot be same");
        }


        if (ModelState.IsValid) //what is this?? //it is a property of the controller class that checks if the model is valid or not
        {
            //create a category object
            //obj id will be auto generated,so it will be 0 by default first,0 means create a new object
            _categoryRepo.Add(obj); //add changes to dbcontext of entity framework ,Add is used to add new object to dbcontext
            _categoryRepo.Save(); //make all the changes migrate and from migration update the database (internal migration)
            TempData["success"] = "Category created successfully";// its available only for the next render
            return RedirectToAction("Index", "Category"); //redirect to the index action method
        }

        return View();



    }

    public IActionResult Edit(int id) //get method.

    {


        Category? obj = _categoryRepo.Get(u => u.Id== id);
        if (obj == null)
        {
            return NotFound();
        }

        return View(obj); //how view knows which view to return??  //it passes the name of the action method in the view
    }

    [HttpPost] //what is this attribute?? //it is an attribute that tells the compiler that this method is a post method
    public IActionResult Edit(Category obj) //post method
    {

        //if object category name and display order is same ,add error

        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Name", "Category Name and Display Order cannot be same");
        }


        if (ModelState.IsValid) //what is this?? //it is a property of the controller class that checks if the model is valid or not
        {
            //update a category object
            _categoryRepo.Update(obj); //add changes to dbcontext of entity framework 
            _categoryRepo.Save(); //make all the changes migrate and from migration update the database (internal migration)
            TempData["success"] = "Category updated successfully";// its available only for the next render
            return RedirectToAction("Index", "Category"); //redirect to the index action method
        }

        return View();



    }

    public IActionResult Delete(int id) //get method.

    {


        Category? obj = _categoryRepo.Get(u => u.Id == id);
        if (obj == null)
        {
            return NotFound();
        }

        return View(obj); //how view knows which view to return??  //it passes the name of the action method in the view
    }

    [HttpPost] //what is this attribute?? //it is an attribute that tells the compiler that this method is a post method
    public IActionResult Delete(Category obj) //post method
    {
        
      

        //delete object if it exists
       _categoryRepo.Remove(obj); //add changes to dbcontext of entity framework
        _categoryRepo.Save(); //make all the changes migrate and from migration update the database (internal migration)
        TempData["success"] = "Category deleted successfully";// its available only for the next render
        return RedirectToAction("Index", "Category"); //redirect to the index action method
  
    }
}
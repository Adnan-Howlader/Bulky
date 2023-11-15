using Bulky.DataAccess.Repository.iRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bulky_Web.Areas.Admin.Controllers;

[Area("Admin")]//this attribute tells the compiler that this controller belongs to the admin area
[Authorize(Roles = SD.Role_Admin)]
public class CompanyController : Controller
{

    private readonly iCompanyRepository _CompanyRepo;


    public CompanyController(iCompanyRepository CompanyRepo)//who calls this constructor?? //it is called by the framework when the controller is called
    {
        _CompanyRepo = CompanyRepo;
    }

    public IActionResult Index() //action method
    {
        var objList =_CompanyRepo.GetAll().ToList(); //type of objlist is list of Company 
        return View(objList);
    }

    public IActionResult Create() //get method
    {
        return View(); //how view knows which view to return??  //it passes the name of the action method in the view
    }

    [HttpPost] //what is this attribute?? //it is an attribute that tells the compiler that this method is a post method
    public IActionResult Create(Company obj) //post method
    {
        //if obj id already exists ,add error
        if (_CompanyRepo.Get(c => c.Id == obj.Id) != null)
        {
            ModelState.AddModelError("Id", "ID already exists");
        }
      
        //if object Company name and display order is same ,add error

        if (obj.Name == obj.ToString())
        {
            ModelState.AddModelError("Name", "Company Name and Display Order cannot be same");
        }


        if (ModelState.IsValid) //what is this?? //it is a property of the controller class that checks if the model is valid or not
        {
            //create a Company object
            //obj id will be auto generated,so it will be 0 by default first,0 means create a new object
            _CompanyRepo.Add(obj); //add changes to dbcontext of entity framework ,Add is used to add new object to dbcontext
            _CompanyRepo.Save(); //make all the changes migrate and from migration update the database (internal migration)
            TempData["success"] = "Company created successfully";// its available only for the next render
            return RedirectToAction("Index", "Company"); //redirect to the index action method
        }

        return View();



    }

    public IActionResult Edit(int id) //get method.

    {


        Company? obj = _CompanyRepo.Get(u => u.Id== id);
        if (obj == null)
        {
            return NotFound();
        }

        return View(obj); //how view knows which view to return??  //it passes the name of the action method in the view
    }

    [HttpPost] //what is this attribute?? //it is an attribute that tells the compiler that this method is a post method
    public IActionResult Edit(Company obj) //post method
    {

        //if object Company name and display order is same ,add error

        if (obj.Name == obj.ToString())
        {
            ModelState.AddModelError("Name", "Company Name and Display Order cannot be same");
        }


        if (ModelState.IsValid) //what is this?? //it is a property of the controller class that checks if the model is valid or not
        {
            //update a Company object
            _CompanyRepo.Update(obj); //add changes to dbcontext of entity framework 
            _CompanyRepo.Save(); //make all the changes migrate and from migration update the database (internal migration)
            TempData["success"] = "Company updated successfully";// its available only for the next render
            return RedirectToAction("Index", "Company"); //redirect to the index action method
        }

        return View();



    }

    public IActionResult Delete(int id) //get method.

    {


        Company? obj = _CompanyRepo.Get(u => u.Id == id);
        if (obj == null)
        {
            return NotFound();
        }

        return View(obj); //how view knows which view to return??  //it passes the name of the action method in the view
    }

    [HttpPost] //what is this attribute?? //it is an attribute that tells the compiler that this method is a post method
    public IActionResult Delete(Company obj) //post method
    {
        
      

        //delete object if it exists
       _CompanyRepo.Remove(obj); //add changes to dbcontext of entity framework
        _CompanyRepo.Save(); //make all the changes migrate and from migration update the database (internal migration)
        TempData["success"] = "Company deleted successfully";// its available only for the next render
        return RedirectToAction("Index", "Company"); //redirect to the index action method
  
    }
}
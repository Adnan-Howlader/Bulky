using Bulky.DataAccess.Repository.iRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Bulky_Web.Areas.Admin.Controllers;

[Area("Admin")]//this attribute tells the compiler that this controller belongs to the admin area
[Authorize(Roles = SD.Role_Admin)]
public class ProductController:Controller
{ 
    private readonly IWebHostEnvironment _webHostEnvironment; 
    private readonly iCategoryRepository _categoryRepo;
    private readonly iProductRepository _productRepo;
  //readonly means that the value of the field can be changed only in the constructor of the class
  
  public ProductController(iProductRepository productRepo,iCategoryRepository categoryRepo,IWebHostEnvironment environment)
  {
    _productRepo = productRepo;
    _categoryRepo = categoryRepo;
    _webHostEnvironment = environment;
    
  }
  public IActionResult Index() //action method
    {
        var objList =_productRepo.GetAll().ToList(); //type of objlist is list of product 
        
        //get all the categories name from the database
        
     
        
        
        
        return View(objList);
    }

    public IActionResult Create() //get method
    {
        IEnumerable<SelectListItem> CategoryDropDown = _categoryRepo.GetAll().Select(i => new SelectListItem
        {
            Text = i.Name,
            Value = i.Id.ToString()// key value pair
        });//what we are doing here?? //we are creating a list of select list item and storing it in a variable
        
        ProductVM productVm = new ProductVM()
        {
            Product = new Product(),
            CategoryList = CategoryDropDown
        };
   
        return View(productVm); //how view knows which view to return??  //it passes the name of the action method in the view
    }

    [HttpPost] //what is this attribute?? //it is an attribute that tells the compiler that this method is a post method
    public IActionResult Create(ProductVM productvm,IFormFile file) //post method
    {
        //if foreign
        if (_categoryRepo.Get(u => u.Id== productvm.Product.CategoryId) == null)
        {
            ModelState.AddModelError("CategoryId", "category ID dont exist");
            return View();
        }


      
        {
            //upload image
            string webRootPath = _webHostEnvironment.WebRootPath;//get the Bulky_Web path of the web
            string filepath= Path.Combine(webRootPath, @"images/product"); //combine the Bulky_Web path with the images folder
            string filename = Guid.NewGuid().ToString()+".jpg"; //generate a unique name for the image
            
         
            
            
            
            //save the image in the folder
            using (var filestream = new FileStream(Path.Combine(filepath,filename),FileMode.Create))
            {
                //seek to the beginning of the file
                file.OpenReadStream().Seek(0, SeekOrigin.Begin);
                
                file.CopyTo(filestream);
          
                
                
                
            }
            
            //saving imageurl
            productvm.Product.ImageUrl = @"/images/product/" + filename;
            
            
            
            
            
            
            
            _productRepo.Add(productvm.Product); //add changes to dbcontext of entity framework ,Add is used to add new object to dbcontext
            _productRepo.Save(); //make all the changes migrate and from migration update the database (internal migration)
            TempData["success"] = "product created successfully"; // its available only for the next render
            return RedirectToAction("Index", "product"); //redirect to the index action method
        }

        
        

        



    }

    public IActionResult Edit(int id) //get method.

    {
        IEnumerable<SelectListItem> CategoryDropDown = _categoryRepo.GetAll().Select(i => new SelectListItem
        {
            Text = i.Name,
            Value = i.Id.ToString()
        });


        ProductVM productVm = new ProductVM()
        {
            Product = _productRepo.Get(u => u.Id == id),
            CategoryList = CategoryDropDown
        };

        return View(productVm); //how view knows which view to return??  //it passes the name of the action method in the view
    }

    [HttpPost] //what is this attribute?? //it is an attribute that tells the compiler that this method is a post method
    public IActionResult Edit(ProductVM productvm,IFormFile update_file) //post method
    {
        if (_categoryRepo.Get(u => u.Id== productvm.Product.CategoryId) == null)
        {
            ModelState.AddModelError("CategoryId", "category ID dont exist");
            return View();
        }


      
        {
            //upload image
            string webRootPath = _webHostEnvironment.WebRootPath;//get the Bulky_Web path of the web
            string filepath= Path.Combine(webRootPath, @"images/product"); //combine the Bulky_Web path with the images folder
            string filename = Guid.NewGuid().ToString()+".jpg";//generate a unique name for the image
            
            //save the image in the folder
            using (var filestream = new FileStream(Path.Combine(filepath,filename),FileMode.Create))
            {
                //seek to the beginning of the file
                update_file.OpenReadStream().Seek(0, SeekOrigin.Begin);
                
                update_file.CopyTo(filestream);
                
            }
            
            //saving imageurl
            productvm.Product.ImageUrl = @"/images/product/" + filename;
            
            
            
            
            
            
            
            _productRepo.Update(productvm.Product); //add changes to dbcontext of entity framework ,Add is used to add new object to dbcontext
            _productRepo.Save(); //make all the changes migrate and from migration update the database (internal migration)
            TempData["success"] = "product updated successfully"; // its available only for the next render
            return RedirectToAction("Index", "product"); //redirect to the index action method
        }





    }

    public IActionResult Delete(int id) //get method.

    {


        Product? obj = _productRepo.Get(u => u.Id == id);
        if (obj == null)
        {
            return NotFound();
        }

        return View(obj); //how view knows which view to return??  //it passes the name of the action method in the view
    }

    [HttpPost] //what is this attribute?? //it is an attribute that tells the compiler that this method is a post method
    public IActionResult Delete(Product obj) //post method
    {
        //delete the image from the webhost environment folder
        
        
        
      

        //delete object if it exists
       _productRepo.Remove(obj); //add changes to dbcontext of entity framework
        _productRepo.Save(); //make all the changes migrate and from migration update the database (internal migration)
        TempData["success"] = "product deleted successfully";// its available only for the next render
        return RedirectToAction("Index", "product"); //redirect to the index action method
  
    }

 
  
  
  
  


}
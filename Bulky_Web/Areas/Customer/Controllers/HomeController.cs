using System.Diagnostics;
using Bulky.DataAccess.Repository.iRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

namespace Bulky_Web.Areas.Customer.Controllers;


[Area ("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly iProductRepository _productRepository;
    private readonly iCategoryRepository _categoryRepository;

    public HomeController(ILogger<HomeController> logger,iProductRepository productRepository,iCategoryRepository categoryRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        
    }
   

    public IActionResult Index()//this method is called when the user requests the home page
    {
        IEnumerable<Product> productList = _productRepository.GetAll();
        return View(productList);//index parameter is passed to the view method
    }
    
    public IActionResult Details(int id)//this method is called when the user requests the home page
    {
        Product product = _productRepository.Get(p=>p.Id==id);
        //get the category name from category id 
        Category category=_categoryRepository.Get(c=>c.Id==product.CategoryId);
        
       //make a tuplemodel
        Tuple<Product,Category> tuple=new Tuple<Product,Category>(product,category);
        
        //return the tuple model
        return View(tuple);//index parameter is passed to the view method
        
       

        
        
        
     
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
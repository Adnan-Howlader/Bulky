using Microsoft.AspNetCore.Mvc;

namespace Bulky_Web.Controllers;

public class CategoryController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
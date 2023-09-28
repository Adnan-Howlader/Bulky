using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bulky_Web.Models;

namespace Bulky_Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()//this method is called when the user requests the home page
    {
        return View();//index parameter is passed to the view method
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
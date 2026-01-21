using Microsoft.AspNetCore.Mvc;
using StockApp.Services;

namespace StockApp.Controllers;

public class HomeController : Controller
{
    private readonly MyService _myService;

    public HomeController(MyService myService)
    {
        _myService = myService;
    }
    
    // GET
    [Route("/")]
    [Route("/home")]
    public async Task<IActionResult> Index()
    {
        await _myService.TestMethod(); 
        return View();
    }
}
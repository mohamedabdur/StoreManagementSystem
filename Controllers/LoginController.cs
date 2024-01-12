using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using practice.Models;

namespace practice.Controllers;

public class LoginController : Controller
{
     private readonly ILogger<HomeController> _logger;
     private readonly IRepository _repository;
    private readonly IConfiguration _configuration;

     public LoginController(ILogger<HomeController> logger, IRepository repository,IConfiguration configuration)
    {
        _logger = logger;
        _repository = repository;
        _configuration=configuration;
    }
    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.User = TempData["User"];
        Console.WriteLine("-----------------"+ _configuration["Name"]);
        return View();
        
    }
    [HttpPost]
    public IActionResult Index(User user)
    {
        string loginmessage = _repository.checkLogin(user);
        if(loginmessage == "AdminLogin")
        {
            HttpContext.Session.SetString("Session",user.Name);
            TempData["Notify"] = " Admin Logged in successfully";
            return RedirectToAction("Index","Home");
        }
        
        else if(loginmessage == "UserLogin")
        {
            HttpContext.Session.SetString("Session",user.Name);
            TempData["Notify"] = " User Logged in successfully";
            return RedirectToAction("Index","User");
        }
        else if(loginmessage == "Invalid")
        {
            ModelState.AddModelError("name", "Invalid Login.");
            // ViewData["Notify"] = "Invalid Login";
            return View("Index");
        }
        return View();
    }
}
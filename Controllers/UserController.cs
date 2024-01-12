#nullable disable

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using practice.Data;
using System.Linq;
using practice.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;

namespace practice.Controllers;

public class UserController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly StoreMS _db;
    private readonly IRepository _repository;
    Uri baseAddress = new Uri("http://localhost:5213/api");
    HttpClient httpClient;
    
    public UserController(ILogger<HomeController> logger, StoreMS db, IRepository repository)
    {
        _logger = logger;
        _db = db;
        _repository = repository;
        httpClient = new HttpClient();
        httpClient.BaseAddress = baseAddress;
    }

    [HttpGet]
    public IActionResult Index(Product product)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            List<Product> products = _db.Products.ToList();
            ViewBag.ProductList = products;
            ViewBag.Notify = TempData["Notify"];
            return View();
        }
        return RedirectToAction("Index", "Login");
    }

    [HttpPost]
    public IActionResult Index(SellProduct sellProduct)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            sellProduct.customername = HttpContext.Session.GetString("Session");
            var product = _db.Products.Where(x => x.productname == sellProduct.productname).FirstOrDefault();
            if(sellProduct.quantity > 0)
            {
                if(sellProduct.quantity <= product.quantity)
                {
                    sellProduct.status="Pending";
                    _db.CustomerRequest.Add(sellProduct);
                    _db.SaveChanges();
                    TempData["UserProduct"] = "The Request is Submitted, Please Wait for sometime for the admin to Respond!!!";
                    return RedirectToAction("History");
                }
                else
                {
                    Console.WriteLine(product.quantity);
                    TempData["quantitymessage"] = "The available quantity is "+ product.quantity + "";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["quantitymessage"] = "The Quantity should be greater than 1";
                return RedirectToAction("Index");
            }
        }
        return RedirectToAction("Index", "Login");
    }
     public IActionResult Store()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");     
            string customer  =HttpContext.Session.GetString("Session");
            IEnumerable<SellProduct> acceptedProducts = _db.CustomerRequest.Where(p => p.status == "Accepted" && p.customername == customer);
            return View(acceptedProducts);

        }
        return RedirectToAction("Index", "Login");
    }
    public IActionResult MainStore()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            IEnumerable<Product> productList = _db.Products;
            ViewBag.message = TempData["store"];      
            return View(productList);
        }
        return RedirectToAction("Index", "Login");

    }

    public IActionResult History()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            string customer=HttpContext.Session.GetString("Session");
            var History = _db.CustomerRequest.Where(s=>s.customername==customer).ToList();
            ViewBag.UserProduct = TempData["UserProduct"];
            return View(History);
        }
        return RedirectToAction("Index", "Login");
    }
    
    public IActionResult DeleteHistory(string name)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
        var deleteProducts = _db.CustomerRequest.Where(dp => dp.status == name).FirstOrDefault();
        _db.CustomerRequest.Remove(deleteProducts);
        _db.SaveChanges();
        return RedirectToAction("History");
        }
        return RedirectToAction("Index", "Login");
    }

    [HttpGet]
    public IActionResult Message()
    {
         if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            ViewBag.Info = TempData["Info"];
            return View();
        }
        return RedirectToAction("Index", "Login");
    }
    // [HttpPost]
    // public IActionResult Message(Message message)
    // {
    //     if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
    //     {
    //         message.customerName = HttpContext.Session.GetString("Session");
    //         // Console.WriteLine("+++++++++++++++++++++@@@@@@@@@@@@+" +message.message);  
    //         message.id = Convert.ToString(Guid.NewGuid());
    //         _repository.insertingMessage(message);
    //         return RedirectToAction("Message","User");
    //     }
    //     return RedirectToAction("Index", "Login");
    // }

    // webapi

    [HttpPost]
    public IActionResult Message(IFormCollection form)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
        Console.WriteLine("message posted");
        // message.id = "3";
        // message.customerName = "Libra";
        // message.message = "hi";
        // message.date = "08/08/2023";
        Console.WriteLine(form["id"]);
        Console.WriteLine(form["customerName"]);
        Console.WriteLine(form["message"]);
        Console.WriteLine(form["date"]);
        Message message = new Message();
        message.id = form["id"];
        message.customerName = form["customerName"];
        message.message = form["message"];
        message.date = form["date"];
        string data = JsonConvert.SerializeObject(message);
        StringContent content = new StringContent(data,Encoding.UTF8,"application/json");
        HttpResponseMessage response = httpClient.PostAsync(httpClient.BaseAddress+"/Message/PostMessage/",content).Result;
        if(response.IsSuccessStatusCode)
        {
                Console.WriteLine("message posted");
                TempData["Info"] = "Message Received successfully";
                return RedirectToAction("Message", "User");
        } 
        }
        return RedirectToAction("Index", "Login");
    }
}
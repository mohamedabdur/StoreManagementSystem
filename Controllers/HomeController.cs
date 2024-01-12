using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using practice.Data;
using practice.Models;
using System.Data;

namespace practice.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly StoreMS _db;
    // Constructor injection 
    private readonly IRepository _repository;
    public HomeController(StoreMS db, IRepository repository,ILogger<HomeController> logger)
    {
        _logger = logger;
        _db = db;
        _repository = repository;
    }
    public IActionResult Index()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            // Logged out message
            ViewBag.Notify = TempData["Notify"];
            return View();
        }
        return RedirectToAction("Index", "Login");
    }
    public IActionResult Privacy()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            return View();
        }
        return RedirectToAction("Index", "Login");
    }
    public IActionResult Store()
    {
        // Main Store 
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");

            // Displaying the products in the main store
            IEnumerable<Product> productList = _db.Products;
            
            // product edited message
            ViewBag.message = TempData["editmessage"];

            return View(productList);    
        }
        return RedirectToAction("Index", "Login");
    }

    // Adding products to the store

    [HttpGet]
    public IActionResult BuyProduct()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
        return View();
    }
    return RedirectToAction("Index", "Login");
    }

    [HttpPost]
    public IActionResult BuyProduct(Product products)
    { 
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            if(ModelState.IsValid){
            ViewBag.Session = HttpContext.Session.GetString("Session");
            var existingProduct = _db.Products.Where(existingproduct => existingproduct.productname == products.productname && existingproduct.suppliername == products.suppliername).ToList();
            int count = existingProduct.Count;
            if (count == 0)
            {
                _db.Products.Add(products);
                _db.SaveChanges();
                TempData["editmessage"] = "Product Added to the Store!!";
                return RedirectToAction("Store");
            }
            else
            {
                ViewBag.existingProduct = "The Product already exists in the store, So please Just add Some quantity Alone";
                return View();
            }
        }
        else
        {
            return View();
        }
        }
        return RedirectToAction("Index", "Login");
    }

    // Updating the product in the store

    [HttpGet]
    public IActionResult UpdateProduct(int id)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            var product = _db.Products.Find(id);
            return View(product);
        }
        return RedirectToAction("Index", "Login");
    }
    [HttpPost]
    public IActionResult UpdateProduct(Product product)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            _db.Products.Update(product);
            _db.SaveChanges();
            TempData["editmessage"] = "Product Updated!!!";
            return RedirectToAction("Store","Home");
        }
        return RedirectToAction("Index", "Login");
    }

    // Deleting the products
     
    public IActionResult DeleteProduct(int id)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            var deleteProduct = _db.Products.Find(id);
            _db.Remove(deleteProduct);
            _db.SaveChanges();
            TempData["editmessage"] = "Product Deleted!!!";
            return RedirectToAction("Store");
        }
        return RedirectToAction("Index", "Login");
    }

    // Logout

    public IActionResult Logout()
    {
        HttpContext.Session.SetString("Session", "");
        TempData["User"] = "Logged Out Successfully";
        return RedirectToAction("Index", "Login");
    } 

    // Requested Products from the Customers

    [HttpGet]
    public IActionResult CustomerRequest()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            var Request = _db.CustomerRequest.Where(customerrequest => customerrequest.status == "Pending").ToList();

            // Response to the requested products
            ViewBag.Response = TempData["Response"];
            
            return View(Request);
        }
        return RedirectToAction("Index", "Login");
    }

    // Accepting the products for the customers request

     public IActionResult Accept(int id, string name)
     {
        var requestProduct = _db.CustomerRequest.Find(id);
        var mainStore = _db.Products.Where(mainstoreproduct => mainstoreproduct.productname == name).FirstOrDefault();     
        if(mainStore.quantity >= requestProduct.quantity)
        {
            mainStore.quantity = mainStore.quantity - requestProduct.quantity;
            requestProduct.status = "Accepted";
            _db.SaveChanges();
            TempData["Response"] = "Product Accepted!!!";
            return RedirectToAction("CustomerRequest","Home");
        }
        else{
            TempData["Response"] = "Please Check the Store, The Product is Not Available So You cant Accept the Request!!!";
            return RedirectToAction("CustomerRequest","Home");
        }
    }

    // Declining the products for the customers request

    public IActionResult Decline(int id)
    {
        var requestProduct = _db.CustomerRequest.Find(id);
        requestProduct.status = "Declined";
        _db.SaveChanges();
        TempData["Response"] = "Product Declined";
        return RedirectToAction("CustomerRequest","Home");
    }

    // History of the Products sold

    public IActionResult SoldHistory()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            var History = _db.CustomerRequest.Where(history => history.status == "Accepted");
            return View(History);
        }
        return RedirectToAction("Index", "Login");
    }

    // Details of the Seller

    public IActionResult SellerDetails()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            IEnumerable<Product> productList = _db.Products;
            return View(productList);
        }
        return RedirectToAction("Index", "Login");
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // Editing the users (Customers)

    [HttpGet]
    public IActionResult EditUser()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            return View();
        }
        return RedirectToAction("Index", "Login");
    }

    [HttpPost]
    public IActionResult EditUser(NewUser newUser)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            string message = _repository.insertingUser(newUser);
            //return RedirectToAction("ViewUser");
            if (message == "Done")
            {
                TempData["User"] = "User Added Successful";
                return RedirectToAction("ViewUser", "Home");
            }
            else if (message == "ExistingUser")
            {
                ViewData["Notify"] = "The Username already Exist:)";
                return View("EditUser");
            }
            else if (message == "Password doesnot match")
            {
                ViewData["Notify"] = "Password Doesnot Match";
                return View("EditUser");
            }
            else if (message == "Fail")
            {
                ViewData["Notify"] = "Enter Valid details";
                return View("EditUser");
            }
        }
        return RedirectToAction("Index", "Login");
    }

    // View Customers Credentials

    public IActionResult ViewUser(NewUser newUser)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            DataTable datas = _repository.ViewUsers(newUser);
            
            // editing message
            ViewBag.User = TempData["User"];
            
            return View(datas);
        }
        return RedirectToAction("EditUser");
    }

    // Delete the customer

    public IActionResult DeleteUser(string myString)
    {
        _repository.deleteUser(myString);
        TempData["User"] = "User Deleted!!! ";
        return RedirectToAction("ViewUser");
    }

    // Updating the customer credentials

    [HttpGet]
    public IActionResult UpdateUser(string myString)
    {
      if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            DataTable user = _repository.getUser(myString);
            // collecting all the customer details in viewbag and passing it to the UpdateUser view.
            ViewBag.Users = user;
            return View();
        }
            return RedirectToAction("Index", "Login");

    }

    [HttpPost]
    public IActionResult UpdateUser(NewUser newUser)
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            ViewBag.Session = HttpContext.Session.GetString("Session");
            _repository.updateUser(newUser);
            TempData["User"] = "User Updated!!! ";
            return RedirectToAction("ViewUser","Home");
        }
        return RedirectToAction("EditUser");
    }

    // public IActionResult Message(Message message)
    // {
    //     if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
    //     {
    //         ViewBag.Session = HttpContext.Session.GetString("Session");
    //         DataTable messages = _repository.viewMessage(message);
    //         ViewBag.Message = TempData["DeleteMessage"];
    //         return View(messages);
    //     }
    //     return RedirectToAction("Index", "Login");
    // }

    // public IActionResult DeleteMessages(string id)
    // {
    //     if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
    //     {
    //         ViewBag.Session = HttpContext.Session.GetString("Session");
    //         _repository.deleteMessages(id);
    //         TempData["DeleteMessage"] ="Message Deleted!!";
    //         return RedirectToAction("Message");
    // }
    //  return RedirectToAction("Index", "Login");
    // }
}





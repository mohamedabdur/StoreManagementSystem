using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using practice.Data;
using practice.Models;
using Microsoft.AspNetCore.Http;
using System.Data;
using Newtonsoft.Json;

namespace practice.Controllers;

public class MessageAPIController : Controller
{
    Uri baseAddress = new Uri("http://localhost:5213/api");
    HttpClient httpClient;

    public MessageAPIController()
    {
        httpClient = new HttpClient();
        httpClient.BaseAddress = baseAddress;
    }

    public IActionResult ViewMessage()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        {
            List<Message>? messages = new List<Message>();
            HttpResponseMessage response =  httpClient.GetAsync(httpClient.BaseAddress + "/Message/GetMessage/").Result;

            if(response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                messages = JsonConvert.DeserializeObject<List<Message>>(data);
            }
            return View(messages);
        }
        return RedirectToAction("Index", "Login");
    }
}
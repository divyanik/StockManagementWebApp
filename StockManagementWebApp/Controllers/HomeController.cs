using Microsoft.AspNetCore.Mvc;
using StockManagementWebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.EntityFrameworkCore;
using StockManagementWebApp.Data;

public class HomeController : Controller
{
    private const string HardcodedUsername = "admin";
    private const string HardcodedPassword = "password";
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        // Check if the user is logged in by checking session
        if (HttpContext.Session.Id != null)
        {
            ViewBag.IsLoggedIn = true;
        }
        else
        {
            ViewBag.IsLoggedIn = false;
        }

        // logic to display popular items
        var products = _context.Products
           .OrderByDescending(p => p.QuantityPurchased)
           .Take(10)
           .ToList();
        return View(products);
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.Username == HardcodedUsername && model.Password == HardcodedPassword)
            {
                // Store username in session
                HttpContext.Session.SetString("Username", model.Username);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
            }
        }

        return View(model);
    }

    public IActionResult Logout()
    {
        // Clear session
        HttpContext.Session.Remove("Username");
        return RedirectToAction("Index");
    }
}

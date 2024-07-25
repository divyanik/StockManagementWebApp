using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManagementWebApp.Data;
using StockManagementWebApp.Models;

public class StockController : Controller
{
    private readonly ApplicationDbContext _context;

    public StockController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(string sortBy)
    {
        IQueryable<Product> products = _context.Products;

        switch (sortBy)
        {
            case "price":
                products = products.OrderBy(p => p.Price);
                break;
            case "location":
                products = products.OrderBy(p => p.Location);
                break;
            case "demand":
                products = products.OrderByDescending(p => p.Demand);
                break;
        }

        return View(products.ToList());
    }
}

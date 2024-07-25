using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockManagementWebApp.Data;
using StockManagementWebApp.Models;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;

    public OrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    private bool IsLoggedIn()
    {
        return HttpContext.Session.GetString("UserId") != null;
    }

    public IActionResult Index()
    {
        if (!IsLoggedIn())
        {
            return RedirectToAction("Login", "Account");
        }

        var userId = int.Parse(HttpContext.Session.GetString("UserId"));
        var orders = _context.Orders
            .Include(o => o.CartItems)
            .ThenInclude(ci => ci.Product)
            .Where(o => o.UserId == userId)
            .ToList();

        return View(orders);
    }

    public IActionResult AddToCart(int productId, int quantity)
    {
        if (!IsLoggedIn())
        {
            return RedirectToAction("Login", "Account");
        }

        var userId = int.Parse(HttpContext.Session.GetString("UserId"));
        var order = _context.Orders.FirstOrDefault(o => o.UserId == userId && !o.CartItems.Any());

        if (order == null)
        {
            order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                CartItems = new List<CartItems>()
            };
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        var cartItem = new CartItems
        {
            ProductId = productId,
            OrderId = order.Id,
            Quantity = quantity
        };

        _context.CartItems.Add(cartItem);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult EditCartItem(int cartItemId)
    {
        if (!IsLoggedIn())
        {
            return RedirectToAction("Login", "Account");
        }

        var cartItem = _context.CartItems
            .Include(ci => ci.Product)
            .FirstOrDefault(ci => ci.Id == cartItemId);

        if (cartItem == null)
        {
            return NotFound();
        }

        return View(cartItem);
    }

    [HttpPost]
    public IActionResult EditCartItem(int cartItemId, int quantity)
    {
        if (!IsLoggedIn())
        {
            return RedirectToAction("Login", "Account");
        }

        var cartItem = _context.CartItems.Find(cartItemId);
        if (cartItem == null)
        {
            return NotFound();
        }

        cartItem.Quantity = quantity;
        _context.CartItems.Update(cartItem);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult DeleteCartItem(int cartItemId)
    {
        if (!IsLoggedIn())
        {
            return RedirectToAction("Login", "Account");
        }

        var cartItem = _context.CartItems.Find(cartItemId);
        if (cartItem == null)
        {
            return NotFound();
        }

        _context.CartItems.Remove(cartItem);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}

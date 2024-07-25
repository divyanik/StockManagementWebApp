using Microsoft.AspNetCore.Mvc;

namespace StockManagementWebApp.Controllers
{
    public class AccountController : Controller
    {
        private const string Username = "admin";
        private const string Password = "password";
        private const int UserId = 1;

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == Username && password == Password)
            {
                HttpContext.Session.SetString("UserId", UserId.ToString());
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = "Invalid login attempt.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index", "Home");
        }
    }
}

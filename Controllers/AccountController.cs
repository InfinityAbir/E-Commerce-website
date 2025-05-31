using System.Security.Cryptography;
using System.Text;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

public class AccountController : Controller
{
    private readonly EcommerceDbContext _context;

    public AccountController(EcommerceDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult Login()
    {
        var model = new LoginViewModel();
        return View(model);
    }

    [HttpPost]
    public ActionResult Login(LoginViewModel model)
    {
        Console.WriteLine("Input Email: " + model.Email);
        Console.WriteLine("Input Password: " + model.Password);
        Console.WriteLine("Input Role: " + model.Role);

        var user = _context.Users.FirstOrDefault(u =>
            u.Email.ToLower() == model.Email.ToLower() &&
            u.Password == model.Password &&
            u.Role == model.Role);

        if (user != null)
        {
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());

            // ✅ Redirect to the correct dashboard based on role
            switch (user.Role)
            {
                case Role.Admin:
                    return RedirectToAction("Dashboard", "Admin");

                case Role.Seller:
                    return RedirectToAction("Dashboard", "Seller");

                case Role.Customer:
                    return RedirectToAction("Dashboard", "Users");  // Or "Customer" if that's your controller name

                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        ViewBag.Error = "Invalid credentials or role mismatch";
        return View(model);
    }

    public ActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}

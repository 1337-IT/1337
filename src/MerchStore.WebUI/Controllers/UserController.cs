using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MerchStore.Application.Services.Interfaces;
using MerchStore.Domain.Entities;

namespace MerchStore.WebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly IAuthService _authService;
        private const string UserAuthScheme = "UserCookie";

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string username, string password)
        {
            bool success = await _authService.RegisterUserAsync(username, password, "User");
            if (!success)
            {
                ViewBag.Error = "🚫 Username already exists.";
                return View();
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _authService.AuthenticateAsync(username, password);
            if (user == null || user.Role != "User")
            {
                ViewBag.Error = "❌ Invalid username or password.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "User")
            };

            var identity = new ClaimsIdentity(claims, UserAuthScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(UserAuthScheme, principal);

            Console.WriteLine($"✅ User '{username}' logged in successfully.");
            return RedirectToAction("Index", "Catalog");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(UserAuthScheme);
            return RedirectToAction("Login");
        }
    }
}

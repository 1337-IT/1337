using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MerchStore.Application.Services.Interfaces;
using MerchStore.Domain.Entities;
using MerchStore.Infrastructure.Storage;

namespace MerchStore.WebUI.Controllers
{
    public static class AuthSchemes
    {
        public const string Admin = "AdminCookie";
    }

    public class AdminController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly BlobStorageService _blobStorage;
        private readonly IAuthService _authService;

        public AdminController(
            ICatalogService catalogService,
            BlobStorageService blobStorage,
            IAuthService authService)
        {
            _catalogService = catalogService;
            _blobStorage = blobStorage;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _authService.AuthenticateAsync(username, password);
            if (user is null || user.Role != "Admin")
            {
                ViewBag.Error = "❌ Invalid admin credentials.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(claims, AuthSchemes.Admin);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(AuthSchemes.Admin, principal);

            Console.WriteLine($"✅ Admin '{user.Username}' logged in successfully.");

            return RedirectToAction("AddProduct");
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public IActionResult AddProduct() => View();

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> AddProduct(string name, string description, IFormFile imageFile, decimal price, int stock)
        {
            string? imageUrl = null;

            if (imageFile != null && imageFile.Length > 0)
            {
                imageUrl = await _blobStorage.UploadFileAsync(imageFile);
            }

            var product = new Product(
                name,
                description,
                imageUrl is not null ? new Uri(imageUrl) : null,
                price,
                stock);

            await _catalogService.AddProductAsync(product);
            ViewBag.Message = "✅ Product added!";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(AuthSchemes.Admin);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied() => View("AccessDenied");
    }
}

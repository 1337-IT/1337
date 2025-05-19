using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MerchStore.Application.Services.Interfaces;
using MerchStore.Domain.Entities;
using MerchStore.Infrastructure.Storage; // ✅ Needed for image upload

namespace MerchStore.WebUI.Controllers;

public class AdminController : Controller
{
    private readonly ICatalogService _catalogService;
    private readonly BlobStorageService _blobStorage; // ✅ Injected for upload

    public AdminController(ICatalogService catalogService, BlobStorageService blobStorage)
    {
        _catalogService = catalogService;
        _blobStorage = blobStorage;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        if (username == "admin" && password == "admin123") // 🔒 Demo credentials
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(claims, "AdminCookie");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("AdminCookie", principal);

            return RedirectToAction("AddProduct");
        }

        ViewBag.Error = "Invalid username or password.";
        return View();
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
            stock
        );

        await _catalogService.AddProductAsync(product);
        ViewBag.Message = "✅ Product added!";
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("AdminCookie");
        return RedirectToAction("Login");
    }

    public IActionResult AccessDenied() => View("AccessDenied");
}

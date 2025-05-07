using Microsoft.AspNetCore.Mvc;
using MerchStore.Domain.Entities;
using MerchStore.Application.Services.Interfaces;
using System.Text.Json;

namespace MerchStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "CartItems";
        private readonly ICatalogService _catalogService;

        public CartController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        // Load cart from session
        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString(CartSessionKey);
            return string.IsNullOrEmpty(json)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        }

        // Save cart to session
        private void SaveCart(List<CartItem> cart)
        {
            var json = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CartSessionKey, json);
        }

        // Show cart page
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        // ✅ Add to cart with stock check
        [HttpPost]
        public async Task<IActionResult> Add(Guid productId, string productName, decimal unitPrice, int quantity = 1)
        {
            var product = await _catalogService.GetProductByIdAsync(productId);

            if (product == null || product.StockQuantity <= 0)
            {
                TempData["Error"] = "Sorry, this product is out of stock and cannot be added.";
                return RedirectToAction("Index");
            }

            var cart = GetCart();
            var existing = cart.FirstOrDefault(c => c.ProductId == productId);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.Price.Amount,
                    Quantity = quantity
                });
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // ✅ Update quantity
        [HttpPost]
        public IActionResult Update(Guid productId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);
            if (item != null && quantity > 0)
            {
                item.Quantity = quantity;
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        // ✅ Remove item
        [HttpPost]
        public IActionResult Remove(Guid productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        // ✅ Clear cart
        public IActionResult Clear()
        {
            SaveCart(new List<CartItem>());
            return RedirectToAction("Index");
        }

        // ✅ Checkout (save order summary to TempData)
        public IActionResult Checkout()
        {
            var cart = GetCart();

            if (!cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            TempData["OrderSummary"] = JsonSerializer.Serialize(cart);
            SaveCart(new List<CartItem>());

            TempData["Message"] = "Thank you for your order!";
            return RedirectToAction("Confirmation");
        }

        // ✅ Confirmation (view will deserialize this)
        public IActionResult Confirmation()
        {
            return View();
        }

        // Optional product details view
        public async Task<IActionResult> ProductDetails(Guid id)
        {
            var product = await _catalogService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}

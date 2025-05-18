using Microsoft.AspNetCore.Mvc;
using MerchStore.Domain.Entities;
using MerchStore.Application.Services.Interfaces;
using MerchStore.WebUI.Models.Cart;
using System.Text.Json;

namespace MerchStore.WebUI.Controllers
{
    public class CartController : BaseController
    {
        private const string CartSessionKey = "CartItems";
        private readonly ICatalogService _catalogService;

        public CartController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString(CartSessionKey);
            return string.IsNullOrEmpty(json)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            var json = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CartSessionKey, json);
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Guid productId, string productName, decimal unitPrice, int quantity = 1)
        {
            var product = await _catalogService.GetProductByIdAsync(productId);

            if (product is not { StockQuantity: > 0 })
            {
                TempData["ToastMessage"] = "❌ Sorry, this product is out of stock.";
                //return Redirect(Request.Headers["Referer"].ToString());
                return RedirectToAction("Index");
            }

            var cart = GetCart();
            var existing = cart.FirstOrDefault(c => c.ProductId == productId);

            if (existing is not null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = quantity
                });
            }

            SaveCart(cart);
            TempData["ToastMessage"] = "✔️ Added to cart!";
            return Redirect(Request.Headers["Referer"].ToString());
            
        }

        [HttpPost]
        public async Task<IActionResult> AddJson([FromBody] AddToCartDto incoming)
        {
            Console.WriteLine($"DEBUG: ProductId = {incoming.ProductId}");
            Console.WriteLine($"DEBUG: ProductName = {incoming.ProductName}");
            Console.WriteLine($"DEBUG: UnitPrice = {incoming.UnitPrice}");
            Console.WriteLine($"DEBUG: Quantity = {incoming.Quantity}");

            var product = await _catalogService.GetProductByIdAsync(incoming.ProductId);

            Console.WriteLine($"DEBUG: Retrieved = {product?.Name}, Stock = {product?.StockQuantity}");

            if (product is not { StockQuantity: > 0 })
            {
                return Json(new { success = false, message = "Out of stock." });
            }

            var cart = GetCart();
            var existing = cart.FirstOrDefault(c => c.ProductId == incoming.ProductId);

            if (existing is not null)
            {
                existing.Quantity += incoming.Quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = incoming.Quantity
                });
            }

            SaveCart(cart);
            var totalItems = cart.Sum(x => x.Quantity);
            return Json(new { success = true, cartCount = totalItems });
        }

        [HttpPost]
        public IActionResult Update(Guid productId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item is not null && quantity > 0)
            {
                item.Quantity = quantity;
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(Guid productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item is not null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            SaveCart(new List<CartItem>());
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = GetCart();

            if (!cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            return View("CheckoutForm", new CheckoutViewModel());
        }

        [HttpPost]
        public IActionResult Checkout(CheckoutViewModel model)
        {
            var cart = GetCart();

            if (!cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            var name = model.FullName;
            var address = $"{model.AddressLine1}, {model.City}, {model.PostalCode}, {model.Country}";

            TempData["OrderSummary"] = JsonSerializer.Serialize(cart);
            TempData["Message"] = $"Thank you for your order, {name}!";
            TempData["Address"] = address;

            SaveCart(new List<CartItem>());
            return RedirectToAction("Confirmation");
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        public async Task<IActionResult> ProductDetails(Guid id)
        {
            var product = await _catalogService.GetProductByIdAsync(id);
            return product is null ? NotFound() : View(product);
        }
    }
}

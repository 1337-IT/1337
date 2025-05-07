using Microsoft.AspNetCore.Mvc;
using MerchStore.Domain.Entities;
using System.Text.Json;

namespace MerchStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "CartItems";

        // Load the cart from session
        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString(CartSessionKey);
            return string.IsNullOrEmpty(json)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        }

        // Save the cart to session
        private void SaveCart(List<CartItem> cart)
        {
            var json = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CartSessionKey, json);
        }

        // Show the cart page
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        // Add a product to cart (usually from a product detail page)
        [HttpPost]
        public IActionResult Add(Guid productId, string productName, decimal unitPrice, int quantity = 1)
        {
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
                    ProductId = productId,
                    ProductName = productName,
                    UnitPrice = unitPrice,
                    Quantity = quantity
                });
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // ✅ New: Update quantity for an item in the cart
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

        // Remove item from cart
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

        // Clear the cart
        public IActionResult Clear()
        {
            SaveCart(new List<CartItem>());
            return RedirectToAction("Index");
        }

        // Checkout action (to be implemented)
        public IActionResult Checkout()
        {
               // Clear the cart after confirming order
              SaveCart(new List<CartItem>());
             TempData["Message"] = "Thank you for your order! Your cart has been cleared.";
             return RedirectToAction("Confirmation");
        }
        // Confirmation action (to be implemented)  
         public IActionResult Confirmation()
          {
                // Show order confirmation details here
                return View();
          }
       
    }
}

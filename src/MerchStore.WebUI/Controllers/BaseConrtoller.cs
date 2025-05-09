using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MerchStore.Domain.Entities;
using System.Text.Json;

namespace MerchStore.WebUI.Controllers
{
    public class BaseController : Controller
    {
        private const string CartSessionKey = "CartItems";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var json = HttpContext.Session.GetString(CartSessionKey);
            var cart = string.IsNullOrEmpty(json)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();

            ViewBag.CartCount = cart.Sum(c => c.Quantity);

            base.OnActionExecuting(context);
        }
    }
}

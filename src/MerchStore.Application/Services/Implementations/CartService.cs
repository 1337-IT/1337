using MerchStore.Application.Services.Interfaces;
using MerchStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MerchStore.Application.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly List<CartItem> _cartItems = new();

        public List<CartItem> GetCartItems()
        {
            return _cartItems;
        }

        public void AddToCart(CartItem item)
        {
            var existing = _cartItems.FirstOrDefault(c => c.ProductId == item.ProductId);
            if (existing != null)
            {
                existing.Quantity += item.Quantity;
            }
            else
            {
                _cartItems.Add(item);
            }
        }

        public void RemoveFromCart(Guid productId)
        {
            var item = _cartItems.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                _cartItems.Remove(item);
            }
        }

        public void ClearCart()
        {
            _cartItems.Clear();
        }
    }
}

using MerchStore.Domain.Entities;
using System.Collections.Generic;

namespace MerchStore.Application.Services.Interfaces
{
    public interface ICartService
    {
        List<CartItem> GetCartItems();
        void AddToCart(CartItem item);
        void RemoveFromCart(Guid productId);
        void ClearCart();
    }
}

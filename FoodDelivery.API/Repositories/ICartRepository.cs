using System.Threading.Tasks;
using FoodDelivery.API.DTOs;

namespace FoodDelivery.API.Repositories
{
    public interface ICartRepository
    {
        Task<CartDto> GetCartAsync(string cartId);
        Task<CartDto> AddToCartAsync(string cartId, int menuItemId, int quantity, decimal price, string itemName, int restaurantId, string restaurantName);
        Task<CartDto> UpdateCartItemAsync(string cartId, int menuItemId, int quantity);
        Task<CartDto> RemoveFromCartAsync(string cartId, int menuItemId);
        Task ClearCartAsync(string cartId);
        Task<CartDto> ApplyCouponAsync(string cartId, string couponCode, decimal discountAmount);
    }
}

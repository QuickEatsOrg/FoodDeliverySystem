using System.Threading.Tasks;
using FoodDelivery.API.DTOs.Sahil;

namespace FoodDelivery.API.Services.Interfaces.Sahil
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(string cartId);
        Task<CartDto> AddToCartAsync(string cartId, int menuItemId, int quantity);
        Task<CartDto> UpdateCartItemAsync(string cartId, int menuItemId, int quantity);
        Task<CartDto> RemoveFromCartAsync(string cartId, int menuItemId);
        Task ClearCartAsync(string cartId);
        Task<CartDto> ApplyCouponAsync(string cartId, string couponCode, decimal orderAmount);
    }
}

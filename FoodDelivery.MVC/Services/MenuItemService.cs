using FoodDelivery.MVC.DTOs;
using FoodDelivery.MVC.ViewModel;

namespace FoodDelivery.MVC.Services
{
    public class MenuItemService : ApiClientBase
    {
        public MenuItemService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
            : base(httpClient, httpContextAccessor)
        {
        }

        public async Task<List<MenuItemDto>> GetAllAsync()
        {
            var items = await HttpClient.GetFromJsonAsync<List<MenuItemDto>>("api/MenuItem");
            return items ?? new List<MenuItemDto>();
        }

        public async Task<MenuItemDto?> GetByIdAsync(int itemId)
        {
            var response = await HttpClient.GetAsync($"api/MenuItem/{itemId}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MenuItemDto>();
        }

        public async Task<List<MenuItemDto>> GetByRestaurantAsync(int restaurantId)
        {
            var items = await HttpClient.GetFromJsonAsync<List<MenuItemDto>>($"api/MenuItem/restaurant/{restaurantId}");
            return items ?? new List<MenuItemDto>();
        }

        public async Task<(bool Success, string? Error)> CreateAsync(CreateMenuItemViewModel model)
        {
            AddBearerToken();

            var response = await HttpClient.PostAsJsonAsync("api/MenuItem", new
            {
                model.ItemName,
                model.ItemDescription,
                model.ItemPrice,
                model.RestaurantId
            });

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }

            return (false, await ReadErrorMessage(response));
        }

        public async Task<(bool Success, string? Error)> UpdateAsync(EditMenuItemViewModel model)
        {
            AddBearerToken();

            var response = await HttpClient.PutAsJsonAsync($"api/MenuItem/{model.ItemId}", new
            {
                model.ItemName,
                model.ItemDescription,
                model.ItemPrice
            });

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }

            return (false, await ReadErrorMessage(response));
        }

        public async Task<(bool Success, string? Error)> DeleteAsync(int itemId)
        {
            AddBearerToken();

            var response = await HttpClient.DeleteAsync($"api/MenuItem/{itemId}");
            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }

            return (false, await ReadErrorMessage(response));
        }
    }
}

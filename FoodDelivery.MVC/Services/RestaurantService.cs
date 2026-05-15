using FoodDelivery.MVC.DTOs;
using FoodDelivery.MVC.ViewModel;

namespace FoodDelivery.MVC.Services
{
    public class RestaurantService : ApiClientBase
    {
        public RestaurantService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
            : base(httpClient, httpContextAccessor)
        {
        }

        public async Task<RestaurantDto?> GetByIdAsync(int id)
        {
            var response = await HttpClient.GetAsync($"api/Restaurant/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<RestaurantDto>();
        }

        public async Task<List<RestaurantDto>> GetAllAsync()
        {
            var restaurants = await HttpClient.GetFromJsonAsync<List<RestaurantDto>>("api/Restaurant");
            return restaurants ?? new List<RestaurantDto>();
        }

        public async Task<Dictionary<int, string>> GetRestaurantNamesAsync(IEnumerable<int> restaurantIds)
        {
            var names = new Dictionary<int, string>();

            foreach (var restaurantId in restaurantIds.Distinct())
            {
                try
                {
                    var restaurant = await GetByIdAsync(restaurantId);
                    names[restaurantId] = restaurant?.RestaurantName ?? $"Restaurant #{restaurantId}";
                }
                catch (HttpRequestException)
                {
                    names[restaurantId] = $"Restaurant #{restaurantId}";
                }
            }

            return names;
        }

        public async Task<List<MenuItemDto>> GetMenuItemsAsync(int restaurantId)
        {
            var response = await HttpClient.GetAsync($"api/Restaurant/{restaurantId}/menuitems");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<MenuItemDto>();
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<MenuItemDto>>() ?? new List<MenuItemDto>();
        }

        public async Task<(bool Success, string? Error)> CreateAsync(RestaurantCreateViewModel model)
        {
            var response = await HttpClient.PostAsJsonAsync("api/Restaurant", new
            {
                model.RestaurantName,
                model.RestaurantAddress,
                model.RestaurantPhone,
                model.RestaurantEmail,
                model.Password
            });

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }

            return (false, await ReadErrorMessage(response));
        }
    }
}

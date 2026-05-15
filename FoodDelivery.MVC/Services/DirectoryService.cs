using FoodDelivery.MVC.DTOs;
using FoodDelivery.MVC.ViewModel;

namespace FoodDelivery.MVC.Services
{
    public class DirectoryService : ApiClientBase
    {
        public DirectoryService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
            : base(httpClient, httpContextAccessor)
        {
        }

        public async Task<List<CustomerSummaryDto>> GetCustomersAsync()
        {
            AddBearerToken();
            var response = await HttpClient.GetAsync("api/Customer");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<CustomerSummaryDto>>>();
            return result?.Data ?? new List<CustomerSummaryDto>();
        }

        public async Task<List<DriverDto>> GetDriversAsync()
        {
            AddBearerToken();
            return await HttpClient.GetFromJsonAsync<List<DriverDto>>("api/Driver") ?? new List<DriverDto>();
        }

        public async Task<(bool Success, string? Error)> RegisterDriverAsync(DriverRegisterViewModel model)
        {
            var response = await HttpClient.PostAsJsonAsync("api/Auth/register/driver", new
            {
                model.DriverName,
                model.DriverPhone,
                model.DriverVehicle,
                model.DriverEmail,
                model.Password
            });

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }

            return (false, "Unable to register delivery driver. Please check the details and try again.");
        }
    }
}

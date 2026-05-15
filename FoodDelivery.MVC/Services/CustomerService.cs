using FoodDelivery.MVC.DTOs;

namespace FoodDelivery.MVC.Services
{
    public class CustomerService(HttpClient client)
    {
        private readonly HttpClient _client = client;

        public async Task<(bool Success, string? Error)> Register(CustomerRegisterDto dto)
        {
            var response = await _client.PostAsJsonAsync("", new
            {
                dto.CustomerName,
                dto.CustomerEmail,
                dto.CustomerPhone,
                dto.Password
            });

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }

            return (false, "Unable to register customer. Please check the details and try again.");
        }
    }
}

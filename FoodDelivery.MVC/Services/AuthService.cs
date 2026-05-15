using FoodDelivery.MVC.DTOs;
using FoodDelivery.MVC.ViewModel;

namespace FoodDelivery.MVC.Services
{
    public class AuthService(HttpClient client, IHttpContextAccessor contextAccessor)
    {
        private readonly HttpClient _client = client;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        public async Task<(bool Success, string? Error)> Login(LoginViewModel login)
        {
            var response = await _client.PostAsJsonAsync("api/auth/login", login);
            if (!response.IsSuccessStatusCode)
            {
                return (false, "Invalid email, password, or role.");
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDto>>();
            if (result?.Data == null || string.IsNullOrWhiteSpace(result.Data.Token))
            {
                return (false, result?.Message ?? "Login failed.");
            }

            var session = _contextAccessor.HttpContext?.Session;
            session?.SetString("token", result.Data.Token);
            session?.SetString("email", result.Data.Email);
            session?.SetString("role", result.Data.Role);
            session?.SetString("displayName", result.Data.Email);

            return (true, null);
        }

        public void Logout()
        {
            _contextAccessor.HttpContext?.Session.Clear();
        }
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}

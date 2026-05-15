using System.Net.Http.Headers;
using FoodDelivery.MVC.DTOs;

namespace FoodDelivery.MVC.Services
{
    public abstract class ApiClientBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected ApiClientBase(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            HttpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        protected HttpClient HttpClient { get; }

        protected void AddBearerToken()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
            HttpClient.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(token)
                ? null
                : new AuthenticationHeaderValue("Bearer", token);
        }

        protected static async Task<string> ReadErrorMessage(HttpResponseMessage response)
        {
            var apiError = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
            if (apiError?.Error?.Count > 0)
            {
                return string.Join(" ", apiError.Error);
            }

            return apiError?.Message ?? $"API request failed with status {(int)response.StatusCode}.";
        }
    }
}

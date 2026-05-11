using FoodDelivery.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace FoodDelivery.MVC.Controllers
{
    public class DriverController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DriverController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Dashboard()
        {
            var client = _httpClientFactory.CreateClient("FoodDeliveryApi");

            var drivers = await client.GetFromJsonAsync<List<DriverDto>>("/api/Driver")
                          ?? new List<DriverDto>();

            var deliveries = await client.GetFromJsonAsync<List<DeliveryDto>>("/api/Delivery")
                             ?? new List<DeliveryDto>();

            var model = new DashboardViewModel
            {
                TotalDrivers = drivers.Count,
                TotalDeliveries = deliveries.Count,
                DeliveredOrders = deliveries.Count(x =>
                    !string.IsNullOrWhiteSpace(x.OrderStatus) &&
                    x.OrderStatus.Equals("Delivered", StringComparison.OrdinalIgnoreCase)
                ),
                RecentDeliveries = deliveries.Take(5).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyDeliveries(int? driverId)
        {
            var model = new MyDeliveriesViewModel
            {
                DriverId = driverId
            };

            if (driverId.HasValue)
            {
                var client = _httpClientFactory.CreateClient("FoodDeliveryApi");

                var deliveries = await client.GetFromJsonAsync<List<DeliveryDto>>(
                    $"/api/Delivery/driver/{driverId.Value}"
                );

                model.Deliveries = deliveries ?? new List<DeliveryDto>();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeliveryDetails(int id)
        {
            var client = _httpClientFactory.CreateClient("FoodDeliveryApi");

            var delivery = await client.GetFromJsonAsync<DeliveryDto>($"/api/Delivery/{id}");

            if (delivery == null)
            {
                return NotFound("Delivery not found");
            }

            return View(delivery);
        }
    }
}
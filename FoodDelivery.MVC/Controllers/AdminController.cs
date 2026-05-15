using FoodDelivery.MVC.Services;
using FoodDelivery.MVC.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.MVC.Controllers
{
    public class AdminController(AuthService authService, DirectoryService directoryService, RestaurantService restaurantService) : Controller
    {
        private readonly AuthService _authService = authService;
        private readonly DirectoryService _directoryService = directoryService;
        private readonly RestaurantService _restaurantService = restaurantService;

        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Auth/Login.cshtml", new LoginViewModel { Role = "Admin" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            model.Role = "Admin";
            if (!ModelState.IsValid)
            {
                return View("~/Views/Auth/Login.cshtml", model);
            }

            var result = await _authService.Login(model);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "Login failed.");
                return View("~/Views/Auth/Login.cshtml", model);
            }

            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            if (!RequireRole())
            {
                return RedirectToAction(nameof(Login));
            }

            var model = new AdminDashboardViewModel();
            try
            {
                model.TotalCustomers = (await _directoryService.GetCustomersAsync()).Count;
                model.TotalRestaurants = (await _restaurantService.GetAllAsync()).Count;
                model.TotalDeliveryDrivers = (await _directoryService.GetDriversAsync()).Count;
            }
            catch (HttpRequestException)
            {
                model.ErrorMessage = "Unable to load all admin totals. Please make sure the FoodDelivery API is running.";
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Customers()
        {
            if (!RequireRole()) return RedirectToAction(nameof(Login));
            return View(await _directoryService.GetCustomersAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Restaurants()
        {
            if (!RequireRole()) return RedirectToAction(nameof(Login));
            return View(await _restaurantService.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Drivers()
        {
            if (!RequireRole()) return RedirectToAction(nameof(Login));
            return View(await _directoryService.GetDriversAsync());
        }

        [HttpGet]
        public IActionResult Orders()
        {
            if (!RequireRole()) return RedirectToAction(nameof(Login));
            return View();
        }

        private bool RequireRole()
        {
            return string.Equals(HttpContext.Session.GetString("role"), "Admin", StringComparison.OrdinalIgnoreCase);
        }
    }
}

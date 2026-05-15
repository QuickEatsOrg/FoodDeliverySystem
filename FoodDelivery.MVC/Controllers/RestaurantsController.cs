using System.Text.Json;
using FoodDelivery.MVC.DTOs;
using FoodDelivery.MVC.Services;
using FoodDelivery.MVC.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.MVC.Controllers
{
    public class RestaurantsController : Controller
    {
        private const string SelectedItemsSessionKey = "SelectedMenuItemQuantities";
        private readonly RestaurantService _restaurantService;
        private readonly MenuItemService _menuItemService;
        private readonly RatingDisplayService _ratingDisplayService;
        private readonly AuthService _authService;

        public RestaurantsController(RestaurantService restaurantService, MenuItemService menuItemService, RatingDisplayService ratingDisplayService, AuthService authService)
        {
            _restaurantService = restaurantService;
            _menuItemService = menuItemService;
            _ratingDisplayService = ratingDisplayService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var restaurants = await _restaurantService.GetAllAsync();
                var model = restaurants.Select(restaurant => new RestaurantCardViewModel
                {
                    RestaurantId = restaurant.RestaurantId,
                    RestaurantName = restaurant.RestaurantName,
                    RestaurantAddress = restaurant.RestaurantAddress
                }).ToList();

                return View(model);
            }
            catch (HttpRequestException)
            {
                var menuItems = await _menuItemService.GetAllAsync();
                var model = menuItems
                    .GroupBy(item => item.RestaurantId)
                    .Select(group => group.First())
                    .Select(item => new RestaurantCardViewModel
                    {
                        RestaurantId = item.RestaurantId,
                        RestaurantName = string.IsNullOrWhiteSpace(item.RestaurantName) ? $"Restaurant #{item.RestaurantId}" : item.RestaurantName,
                        RestaurantAddress = item.RestaurantAddress
                    })
                    .ToList();

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Auth/Login.cshtml", new LoginViewModel { Role = "Restaurant" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            model.Role = "Restaurant";
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
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var restaurant = await _restaurantService.GetByIdAsync(id);
                if (restaurant == null)
                {
                    return NotFound();
                }

                var quantities = GetQuantities();
                var menuItems = await _restaurantService.GetMenuItemsAsync(id);
                var rating = await _ratingDisplayService.GetRestaurantRatingAsync(id);

                var model = new RestaurantDetailsViewModel
                {
                    RestaurantId = restaurant.RestaurantId,
                    RestaurantName = restaurant.RestaurantName,
                    RestaurantAddress = restaurant.RestaurantAddress,
                    RestaurantPhone = restaurant.RestaurantPhone,
                    RestaurantEmail = restaurant.RestaurantEmail,
                    AverageRating = rating.AverageRating,
                    RatingCount = rating.RatingCount,
                    MenuItems = menuItems.Select(item => BuildCard(item, restaurant.RestaurantName, restaurant.RestaurantAddress, quantities, rating)).ToList()
                };

                return View(model);
            }
            catch (HttpRequestException)
            {
                return View(new RestaurantDetailsViewModel
                {
                    RestaurantId = id,
                    ErrorMessage = "Unable to load restaurant details. Please make sure the FoodDelivery API is running."
                });
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new RestaurantCreateViewModel());
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Create", new RestaurantCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RestaurantCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            (bool Success, string? Error) result;
            try
            {
                result = await _restaurantService.CreateAsync(model);
            }
            catch (HttpRequestException)
            {
                result = (false, "Unable to create restaurant. Please make sure the FoodDelivery API is running.");
            }

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "Unable to create restaurant.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Restaurant registered successfully.";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            if (!RequireRole("Restaurant"))
            {
                return RedirectToAction(nameof(Login));
            }

            var model = new RestaurantDashboardViewModel();
            try
            {
                var email = HttpContext.Session.GetString("email");
                var restaurant = (await _restaurantService.GetAllAsync())
                    .FirstOrDefault(item => string.Equals(item.RestaurantEmail, email, StringComparison.OrdinalIgnoreCase));

                model.Restaurant = restaurant;
                if (restaurant != null)
                {
                    var items = await _restaurantService.GetMenuItemsAsync(restaurant.RestaurantId);
                    model.MenuItems = items.Select(item => BuildCard(item, restaurant.RestaurantName, restaurant.RestaurantAddress, new Dictionary<int, int>(), (null, 0))).ToList();
                }
            }
            catch (HttpRequestException)
            {
                model.ErrorMessage = "Unable to load restaurant dashboard. Please make sure the FoodDelivery API is running.";
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (!RequireRole("Restaurant"))
            {
                return RedirectToAction(nameof(Login));
            }

            return View("Dashboard", await BuildDashboardModel());
        }

        [HttpGet]
        public IActionResult Orders()
        {
            if (!RequireRole("Restaurant"))
            {
                return RedirectToAction(nameof(Login));
            }

            return View();
        }

        private Dictionary<int, int> GetQuantities()
        {
            var value = HttpContext.Session.GetString(SelectedItemsSessionKey);
            return string.IsNullOrWhiteSpace(value)
                ? new Dictionary<int, int>()
                : JsonSerializer.Deserialize<Dictionary<int, int>>(value) ?? new Dictionary<int, int>();
        }

        private async Task<RestaurantDashboardViewModel> BuildDashboardModel()
        {
            var email = HttpContext.Session.GetString("email");
            var restaurant = (await _restaurantService.GetAllAsync())
                .FirstOrDefault(item => string.Equals(item.RestaurantEmail, email, StringComparison.OrdinalIgnoreCase));
            return new RestaurantDashboardViewModel { Restaurant = restaurant };
        }

        private bool RequireRole(string role)
        {
            return string.Equals(HttpContext.Session.GetString("role"), role, StringComparison.OrdinalIgnoreCase);
        }

        private static MenuItemCardViewModel BuildCard(
            MenuItemDto item,
            string restaurantName,
            string restaurantAddress,
            Dictionary<int, int> quantities,
            (double? AverageRating, int RatingCount) rating)
        {
            return new MenuItemCardViewModel
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                ItemDescription = item.ItemDescription,
                ItemPrice = item.ItemPrice,
                RestaurantId = item.RestaurantId,
                RestaurantName = string.IsNullOrWhiteSpace(item.RestaurantName) ? restaurantName : item.RestaurantName,
                RestaurantAddress = string.IsNullOrWhiteSpace(item.RestaurantAddress) ? restaurantAddress : item.RestaurantAddress,
                CuisineCategory = GetCuisineCategory(item),
                AverageRating = rating.AverageRating,
                RatingCount = rating.RatingCount,
                Quantity = quantities.GetValueOrDefault(item.ItemId)
            };
        }

        private static string GetCuisineCategory(MenuItemDto item)
        {
            var text = $"{item.ItemName} {item.ItemDescription}".ToLowerInvariant();

            if (text.Contains("pizza")) return "Pizza";
            if (text.Contains("burger")) return "Burger";
            if (text.Contains("pasta")) return "Pasta";
            if (text.Contains("roll") || text.Contains("wrap")) return "Rolls and Wraps";
            if (text.Contains("dessert") || text.Contains("cake") || text.Contains("sweet")) return "Dessert";
            if (text.Contains("coffee") || text.Contains("juice") || text.Contains("drink")) return "Drinks";
            if (text.Contains("salad")) return "Salad";

            return "Food";
        }
    }
}

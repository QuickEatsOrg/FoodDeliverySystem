using System.Text.Json;
using FoodDelivery.MVC.DTOs;
using FoodDelivery.MVC.Services;
using FoodDelivery.MVC.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.MVC.Controllers
{
    public class MenuItemsController : Controller
    {
        private const string SelectedItemsSessionKey = "SelectedMenuItemQuantities";
        private readonly MenuItemService _menuItemService;
        private readonly RestaurantService _restaurantService;
        private readonly RatingDisplayService _ratingDisplayService;

        public MenuItemsController(
            MenuItemService menuItemService,
            RestaurantService restaurantService,
            RatingDisplayService ratingDisplayService)
        {
            _menuItemService = menuItemService;
            _restaurantService = restaurantService;
            _ratingDisplayService = ratingDisplayService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search)
        {
            try
            {
                var quantities = GetQuantities();
                var items = await _menuItemService.GetAllAsync();
                var ratings = await _ratingDisplayService.GetRestaurantRatingsAsync(items.Select(item => item.RestaurantId));
                var cards = items.Select(item => BuildCard(item, quantities, ratings)).ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    cards = cards
                        .Where(item => MatchesSearch(item, search))
                        .ToList();
                }

                var model = new MenuItemsPageViewModel
                {
                    SearchTerm = search,
                    Items = cards
                };

                return View(model);
            }
            catch (HttpRequestException)
            {
                return View(new MenuItemsPageViewModel
                {
                    ErrorMessage = "Unable to load food items. Please make sure the FoodDelivery API is running."
                });
            }
        }

        [HttpGet("MenuItems/Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var item = await _menuItemService.GetByIdAsync(id);
                if (item == null)
                {
                    return NotFound();
                }

                var restaurant = await _restaurantService.GetByIdAsync(item.RestaurantId);
                var quantities = GetQuantities();
                var rating = await _ratingDisplayService.GetRestaurantRatingAsync(item.RestaurantId);
                var restaurantItems = await _restaurantService.GetMenuItemsAsync(item.RestaurantId);
                var ratingMap = new Dictionary<int, (double? AverageRating, int RatingCount)>
                {
                    [item.RestaurantId] = rating
                };

                var model = new MenuItemDetailsViewModel
                {
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    ItemDescription = item.ItemDescription,
                    ItemPrice = item.ItemPrice,
                    CuisineCategory = GetCuisineCategory(item),
                    Quantity = quantities.GetValueOrDefault(item.ItemId),
                    RestaurantId = item.RestaurantId,
                    RestaurantName = restaurant?.RestaurantName ?? item.RestaurantName,
                    RestaurantAddress = restaurant?.RestaurantAddress ?? item.RestaurantAddress,
                    RestaurantPhone = restaurant?.RestaurantPhone ?? string.Empty,
                    RestaurantEmail = restaurant?.RestaurantEmail ?? string.Empty,
                    AverageRating = rating.AverageRating,
                    RatingCount = rating.RatingCount,
                    OtherMenuItems = restaurantItems
                        .Where(menuItem => menuItem.ItemId != item.ItemId)
                        .Select(menuItem => BuildCard(menuItem, quantities, ratingMap))
                        .ToList()
                };

                return View(model);
            }
            catch (HttpRequestException)
            {
                return View(new MenuItemDetailsViewModel
                {
                    ItemId = id,
                    ErrorMessage = "Unable to load item details. Please make sure the FoodDelivery API is running."
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateQuantity(int itemId, int change, string? returnUrl)
        {
            var quantities = GetQuantities();
            var currentQuantity = quantities.GetValueOrDefault(itemId);
            var newQuantity = Math.Clamp(currentQuantity + change, 0, 10);

            if (newQuantity == 0)
            {
                quantities.Remove(itemId);
            }
            else
            {
                quantities[itemId] = newQuantity;
            }

            SaveQuantities(quantities);
            // TODO: Send selected menu item quantities to POST /api/Order when the Order API integration is ready.

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("MenuItems/Manage/{restaurantId:int}")]
        public async Task<IActionResult> Manage(int restaurantId)
        {
            try
            {
                var items = await _menuItemService.GetByRestaurantAsync(restaurantId);
                var model = new ManageMenuViewModel
                {
                    RestaurantId = restaurantId,
                    NewItem = new CreateMenuItemViewModel
                    {
                        RestaurantId = restaurantId
                    },
                    MenuItems = items.Select(item => new EditMenuItemViewModel
                    {
                        ItemId = item.ItemId,
                        ItemName = item.ItemName,
                        ItemDescription = item.ItemDescription,
                        ItemPrice = item.ItemPrice
                    }).ToList()
                };

                return View(model);
            }
            catch (HttpRequestException)
            {
                return View(new ManageMenuViewModel
                {
                    RestaurantId = restaurantId,
                    NewItem = new CreateMenuItemViewModel
                    {
                        RestaurantId = restaurantId
                    },
                    ErrorMessage = "Unable to load menu items. Please make sure the FoodDelivery API is running."
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMenuItem(CreateMenuItemViewModel newItem)
        {
            if (!ModelState.IsValid)
            {
                var model = await BuildManageModel(newItem.RestaurantId);
                model.NewItem = newItem;
                return View("Manage", model);
            }

            (bool Success, string? Error) result;
            try
            {
                result = await _menuItemService.CreateAsync(newItem);
            }
            catch (HttpRequestException)
            {
                result = (false, "Unable to add menu item. Please make sure the FoodDelivery API is running.");
            }

            if (!result.Success)
            {
                var model = await BuildManageModel(newItem.RestaurantId);
                model.NewItem = newItem;
                model.ErrorMessage = result.Error ?? "Unable to add menu item.";
                return View("Manage", model);
            }

            TempData["SuccessMessage"] = "Menu item added successfully.";
            return RedirectToAction(nameof(Manage), new { restaurantId = newItem.RestaurantId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMenuItem(int restaurantId, EditMenuItemViewModel item)
        {
            if (!ModelState.IsValid)
            {
                var model = await BuildManageModel(restaurantId);
                model.ErrorMessage = "Please correct the menu item details.";
                return View("Manage", model);
            }

            (bool Success, string? Error) result;
            try
            {
                result = await _menuItemService.UpdateAsync(item);
            }
            catch (HttpRequestException)
            {
                result = (false, "Unable to update menu item. Please make sure the FoodDelivery API is running.");
            }

            if (!result.Success)
            {
                var model = await BuildManageModel(restaurantId);
                model.ErrorMessage = result.Error ?? "Unable to update menu item.";
                return View("Manage", model);
            }

            TempData["SuccessMessage"] = "Menu item updated successfully.";
            return RedirectToAction(nameof(Manage), new { restaurantId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMenuItem(int restaurantId, int itemId)
        {
            (bool Success, string? Error) result;
            try
            {
                result = await _menuItemService.DeleteAsync(itemId);
            }
            catch (HttpRequestException)
            {
                result = (false, "Unable to delete menu item. Please make sure the FoodDelivery API is running.");
            }

            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] =
                result.Success ? "Menu item deleted successfully." : result.Error ?? "Unable to delete menu item.";

            return RedirectToAction(nameof(Manage), new { restaurantId });
        }

        private async Task<ManageMenuViewModel> BuildManageModel(int restaurantId)
        {
            List<MenuItemDto> items;
            try
            {
                items = await _menuItemService.GetByRestaurantAsync(restaurantId);
            }
            catch (HttpRequestException)
            {
                items = new List<MenuItemDto>();
            }

            return new ManageMenuViewModel
            {
                RestaurantId = restaurantId,
                NewItem = new CreateMenuItemViewModel
                {
                    RestaurantId = restaurantId
                },
                MenuItems = items.Select(item => new EditMenuItemViewModel
                {
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    ItemDescription = item.ItemDescription,
                    ItemPrice = item.ItemPrice
                }).ToList()
            };
        }

        private static MenuItemCardViewModel BuildCard(
            MenuItemDto item,
            Dictionary<int, int> quantities,
            Dictionary<int, (double? AverageRating, int RatingCount)> ratings)
        {
            var rating = ratings.GetValueOrDefault(item.RestaurantId);

            return new MenuItemCardViewModel
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                ItemDescription = item.ItemDescription,
                ItemPrice = item.ItemPrice,
                RestaurantId = item.RestaurantId,
                RestaurantName = string.IsNullOrWhiteSpace(item.RestaurantName) ? $"Restaurant #{item.RestaurantId}" : item.RestaurantName,
                RestaurantAddress = item.RestaurantAddress,
                CuisineCategory = GetCuisineCategory(item),
                AverageRating = rating.AverageRating,
                RatingCount = rating.RatingCount,
                Quantity = quantities.GetValueOrDefault(item.ItemId)
            };
        }

        private static bool MatchesSearch(MenuItemCardViewModel item, string search)
        {
            return Contains(item.ItemName, search)
                || Contains(item.RestaurantName, search)
                || Contains(item.CuisineCategory, search)
                || Contains(item.RestaurantAddress, search);
        }

        private static bool Contains(string? value, string search)
        {
            return !string.IsNullOrWhiteSpace(value)
                && value.Contains(search, StringComparison.OrdinalIgnoreCase);
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

        private Dictionary<int, int> GetQuantities()
        {
            var value = HttpContext.Session.GetString(SelectedItemsSessionKey);
            return string.IsNullOrWhiteSpace(value)
                ? new Dictionary<int, int>()
                : JsonSerializer.Deserialize<Dictionary<int, int>>(value) ?? new Dictionary<int, int>();
        }

        private void SaveQuantities(Dictionary<int, int> quantities)
        {
            HttpContext.Session.SetString(SelectedItemsSessionKey, JsonSerializer.Serialize(quantities));
        }
    }
}

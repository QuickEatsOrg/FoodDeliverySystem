using FoodDelivery.MVC.Services;
using FoodDelivery.MVC.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.MVC.Controllers
{
    public class AuthController(AuthService authService) : Controller
    {
        private readonly AuthService _authService = authService;

        [HttpGet]
        public IActionResult Login(string? role)
        {
            return View(new LoginViewModel { Role = role ?? "Customer" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.Login(model);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "Login failed.");
                return View(model);
            }

            return RedirectToRoleDashboard(model.Role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _authService.Logout();
            TempData["SuccessMessage"] = "You have been logged out.";
            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectToRoleDashboard(string role)
        {
            return role.ToLowerInvariant() switch
            {
                "admin" => RedirectToAction("Dashboard", "Admin"),
                "restaurant" => RedirectToAction("Dashboard", "Restaurants"),
                "deliverydriver" => RedirectToAction("Dashboard", "DeliveryDriver"),
                _ => RedirectToAction("Dashboard", "Customer")
            };
        }
    }
}

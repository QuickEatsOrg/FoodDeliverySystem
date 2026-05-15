using FoodDelivery.MVC.DTOs;
using FoodDelivery.MVC.Services;
using FoodDelivery.MVC.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.MVC.Controllers
{
    public class CustomerController(CustomerService customerService, AuthService authService) : Controller
    {
        private readonly CustomerService _customerService = customerService;
        private readonly AuthService _authService = authService;

        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Auth/Login.cshtml", new LoginViewModel { Role = "Customer" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            model.Role = "Customer";
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
        public IActionResult Register()
        {
            return View(new CustomerRegisterDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CustomerRegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var result = await _customerService.Register(dto);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "Unable to register customer.");
                return View(dto);
            }

            TempData["SuccessMessage"] = "Customer account created. Please sign in.";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            if (!RequireRole("Customer"))
            {
                return RedirectToAction(nameof(Login));
            }

            return View(new CustomerDashboardViewModel
            {
                Name = HttpContext.Session.GetString("displayName") ?? "Customer"
            });
        }

        private bool RequireRole(string role)
        {
            return string.Equals(HttpContext.Session.GetString("role"), role, StringComparison.OrdinalIgnoreCase);
        }
    }
}

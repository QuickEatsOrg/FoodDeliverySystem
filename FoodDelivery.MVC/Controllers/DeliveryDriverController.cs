using FoodDelivery.MVC.Services;
using FoodDelivery.MVC.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.MVC.Controllers
{
    public class DeliveryDriverController(AuthService authService, DirectoryService directoryService) : Controller
    {
        private readonly AuthService _authService = authService;
        private readonly DirectoryService _directoryService = directoryService;

        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Auth/Login.cshtml", new LoginViewModel { Role = "DeliveryDriver" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            model.Role = "DeliveryDriver";
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
            return View(new DriverRegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(DriverRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _directoryService.RegisterDriverAsync(model);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "Unable to register driver.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Delivery driver account created. Please sign in.";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            if (!RequireRole())
            {
                return RedirectToAction(nameof(Login));
            }

            return View(BuildDashboard());
        }

        [HttpGet]
        public IActionResult AssignedDeliveries()
        {
            if (!RequireRole())
            {
                return RedirectToAction(nameof(Login));
            }

            return View(BuildDashboard());
        }

        [HttpGet]
        public IActionResult Details(int id = 1)
        {
            if (!RequireRole())
            {
                return RedirectToAction(nameof(Login));
            }

            return View(new DeliveryCardViewModel
            {
                DeliveryId = id,
                Pickup = "QuickBite partner restaurant",
                Dropoff = "Customer delivery address",
                Status = "Pending"
            });
        }

        private DriverDashboardViewModel BuildDashboard()
        {
            return new DriverDashboardViewModel
            {
                DriverName = HttpContext.Session.GetString("displayName") ?? "Delivery Driver",
                Deliveries =
                {
                    new DeliveryCardViewModel { DeliveryId = 1001, Pickup = "Cafe Aroma", Dropoff = "Customer address", Status = "Pending" },
                    new DeliveryCardViewModel { DeliveryId = 1002, Pickup = "Urban Tandoor", Dropoff = "Customer address", Status = "Picked Up" }
                }
            };
        }

        private bool RequireRole()
        {
            return string.Equals(HttpContext.Session.GetString("role"), "DeliveryDriver", StringComparison.OrdinalIgnoreCase);
        }
    }
}

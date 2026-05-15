using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.MVC.Controllers
{
    public class OrdersController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("role")))
            {
                return RedirectToAction("Login", "Customer");
            }

            return View();
        }
    }
}

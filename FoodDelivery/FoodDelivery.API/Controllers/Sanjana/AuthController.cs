using FoodDelivery.API.DTOs.Sanjana;
using FoodDelivery.API.Services.Interfaces.Sanjana;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers.Sanjana;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var response = await _authService.LoginAsync(loginDto);
        if (response == null)
        {
            return Unauthorized(new { message = "Invalid email, password, or role" });
        }

        return Ok(response);
    }

    [HttpPost("register/customer")]
    public async Task<IActionResult> RegisterCustomer(CreateCustomerDto customerDto)
    {
        var result = await _authService.RegisterCustomerAsync(customerDto);
        if (!result) return BadRequest(new { message = "Registration failed or email already exists" });
        return Ok(new { message = "Customer registered successfully" });
    }

    [HttpPost("register/driver")]
    public async Task<IActionResult> RegisterDriver(RegisterDriverDto driverDto)
    {
        var result = await _authService.RegisterDriverAsync(driverDto);
        if (!result) return BadRequest(new { message = "Registration failed or email already exists" });
        return Ok(new { message = "Driver registered successfully" });
    }

    [HttpPost("register/restaurant")]
    public async Task<IActionResult> RegisterRestaurant(RegisterRestaurantDto restaurantDto)
    {
        var result = await _authService.RegisterRestaurantAsync(restaurantDto);
        if (!result) return BadRequest(new { message = "Registration failed or email already exists" });
        return Ok(new { message = "Restaurant registered successfully" });
    }
}

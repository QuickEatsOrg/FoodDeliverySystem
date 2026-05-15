using FoodDelivery.API.DTOs;
using FoodDelivery.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers;

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

    [HttpPost("register/driver")]
    public async Task<IActionResult> RegisterDriver(RegisterDriverDto driverDto)
    {
        var created = await _authService.RegisterDriverAsync(driverDto);
        if (!created)
        {
            return Conflict(new ApiResponse<object>
            {
                Success = false,
                Message = "Driver email already exists",
                TimeStamp = DateTime.Now
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Driver registered successfully",
            TimeStamp = DateTime.Now
        });
    }
}

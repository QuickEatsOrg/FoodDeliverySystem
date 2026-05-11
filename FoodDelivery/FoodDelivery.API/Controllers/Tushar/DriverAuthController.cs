using FoodDelivery.API.DTOs.Tushar;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Services.Interfaces.Tushar;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers.Tushar
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverAuthController : ControllerBase
    {
        private readonly IDriverAuthService _driverAuthService;

        public DriverAuthController(IDriverAuthService driverAuthService)
        {
            _driverAuthService = driverAuthService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] DriverLoginDto dto)
        {
            try
            {
                var result = await _driverAuthService.LoginAsync(dto);
                return Ok(result);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
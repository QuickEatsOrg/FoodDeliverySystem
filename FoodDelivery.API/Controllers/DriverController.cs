using FoodDelivery.API.DTOs;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;

        public DriverController(IDriverService driverService)
        {
            _driverService = driverService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDrivers()
        {
            var drivers = await _driverService.GetAllDriversAsync();
            return Ok(drivers);
        }

        [HttpGet("{driverId}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> GetDriverById(int driverId)
        {
            try
            {
                var driver = await _driverService.GetDriverByIdAsync(driverId);
                return Ok(driver);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateDriver([FromBody] CreateDriverDto dto)
        {
            var driver = await _driverService.CreateDriverAsync(dto);
            return Ok(driver);
        }

        [HttpPut("{driverId}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> UpdateDriver(int driverId, [FromBody] UpdateDriverDto dto)
        {
            try
            {
                var driver = await _driverService.UpdateDriverAsync(driverId, dto);
                return Ok(driver);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{driverId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDriver(int driverId)
        {
            try
            {
                var result = await _driverService.DeleteDriverAsync(driverId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
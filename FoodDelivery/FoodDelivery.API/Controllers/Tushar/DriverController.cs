using FoodDelivery.API.DTOs.Tushar;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Services.Interfaces.Tushar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers.Tushar
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "DeliveryDriver")]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;

        public DriverController(IDriverService driverService)
        {
            _driverService = driverService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDrivers()
        {
            var drivers = await _driverService.GetAllDriversAsync();
            return Ok(drivers);
        }

        [HttpGet("{driverId}")]
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
        public async Task<IActionResult> CreateDriver([FromBody] CreateDriverDto dto)
        {
            var driver = await _driverService.CreateDriverAsync(dto);
            return Ok(driver);
        }

        [HttpPut("{driverId}")]
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
        public async Task<IActionResult> DeleteDriver(int driverId)
        {
            try
            {
                await _driverService.DeleteDriverAsync(driverId);
                return Ok("Driver deleted successfully");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
using FoodDelivery.API.DTOs;
using FoodDelivery.API.Helpers;
using FoodDelivery.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CustomerController : ControllerBase
    {

        private readonly ICustomerService _service;


        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetAllCustomersAsync()
        {
            var customers = await _service.GetAllCustomersAsync();
            return Ok(new ApiResponse<IEnumerable<CustomerResponseDto>>
            {
                Message = "All Customers fetched successfully",
                Success = true,
                Data = customers,
                TimeStamp = DateTime.Now
            });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> GetCustomerByIdAsync(int id)
        {
            var (isAuthorized, errorResponse) = AuthorizationHelper.ValidateCustomerAccess(User, id);
            if (!isAuthorized)
            {
                return errorResponse ?? Forbid();
            }            

            var customer = await _service.GetCustomerByIdAsync(id);
            return Ok(new ApiResponse<CustomerResponseDto>
            {
                Message = "Data retrieved successfully",
                Success = true,
                Data = customer,
                TimeStamp = DateTime.Now
            });
        }

        [HttpGet("email/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCustomerByEmailAsync(string email)
        {
            var customer = await _service.GetCustomerByEmailAync(email);
            return Ok(new ApiResponse<CustomerResponseDto>
            {
                Message = "Data retrieved by email successfully",
                Success = true,
                Data = customer,
                TimeStamp = DateTime.Now
            });
        }

        [HttpGet("phone/{phone}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCustomerByPhoneAsync(string phone)
        {
            var customer = await _service.GetCustomerByPhoneAsync(phone);
            return Ok(new ApiResponse<CustomerResponseDto>
            {
                Message = "Data retrieved by phone successfully",
                Success = true,
                Data = customer,
                TimeStamp = DateTime.Now
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCustomerAsync(CreateCustomerDto dto)
        {
            var customer = await _service.CreateCustomerAsync(dto);
            return StatusCode(201, new ApiResponse<CustomerResponseDto>
            {
                Message = "Customer created successfully",
                Success = true,
                Data = customer,
                TimeStamp = DateTime.Now
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> UpdateCustomerAsync(int id, UpdateCustomerDto dto)
        {
            var (isAuthorized, errorResponse) = AuthorizationHelper.ValidateCustomerAccess(User, id);
            if (!isAuthorized)
            {
                return errorResponse ?? Forbid();
            }           

            var customer = await _service.UpdateCustomerAsync(id, dto);
            return Ok(new ApiResponse<CustomerResponseDto>
            {
                Message = "Details updated successfully",
                Success = true,
                Data = customer,
                TimeStamp = DateTime.Now
            });
        }










    }
}
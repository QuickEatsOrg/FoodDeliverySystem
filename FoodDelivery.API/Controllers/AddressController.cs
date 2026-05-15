using FoodDelivery.API.DTOs;
using FoodDelivery.API.Helpers;
using FoodDelivery.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FoodDelivery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin, Customer")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _service;
        public AddressController(IAddressService service)
        {
            _service = service;
        }

        [HttpPost("customer/{customerId}")]
        public async Task<IActionResult> CreateAddressAsync(int customerId, CreateAddressDto dto)
        {
            var (isAuthorized, errorResponse) = AuthorizationHelper.ValidateCustomerAccess(User, customerId);
            if (!isAuthorized)
            {
                return errorResponse ?? Forbid();
            }
            
            var address = await _service.CreateAddressAsync(customerId, dto);
            return StatusCode(201, new ApiResponse<AddressResponseDto>
            {
                Success = true,
                Message = "Address created successfully",
                Data = address,
                TimeStamp = DateTime.Now
            });
        }

        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetAddressByAddressIdAsync(int addressId)
        {

            var address = await _service.GetAddressByAddressIdAsync(addressId);
            var (isAuthorized, errorResponse) = AuthorizationHelper.ValidateCustomerAccess(User, address.CustomerId);
            if (!isAuthorized)
            {
                return errorResponse ?? Forbid();
            }

            return Ok(new ApiResponse<AddressResponseDto>
            {
                Success = true,
                Message = "Address retrieved successfully",
                Data = address,
                TimeStamp = DateTime.Now
            });
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetAddressesByCustomerIdAsync(int customerId)
        {
            var (isAuthorized, errorResponse) = AuthorizationHelper.ValidateCustomerAccess(User, customerId);
            if (!isAuthorized)
            {
                return errorResponse ?? Forbid();
            }
           

            var addresses = await _service.GetAllAddressesByCustomerIdAsync(customerId);
            return Ok(new ApiResponse<IEnumerable<AddressResponseDto>>
            {
                Success = true,
                Message = "Addresses retrieved successfully",
                Data = addresses,
                TimeStamp = DateTime.Now
            });
        }

        [HttpPut("{addressId}")]
        public async Task<IActionResult> UpdateAddressAsync(int addressId, UpdateAddressDto dto)
        {
            var address = await _service.GetAddressByAddressIdAsync(addressId);
            var (isAuthorized, errorResponse) = AuthorizationHelper.ValidateCustomerAccess(User, address.CustomerId);
            if (!isAuthorized)
            {
                return errorResponse ?? Forbid();
            }

            var updatedAddress = await _service.UpdateAddressAsync(addressId, dto);
            return Ok(new ApiResponse<AddressResponseDto>
            {
                Success = true,
                Message = "Address updated successfully",
                Data = updatedAddress,
                TimeStamp = DateTime.Now
            });
        }
    }
}
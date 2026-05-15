using AutoMapper;
using FoodDelivery.API.DTOs;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Models;
using FoodDelivery.API.Repositories;
using FoodDelivery.API.Exceptions;

namespace FoodDelivery.API.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<AddressService> _logger;
        private readonly IMapper _mapper;
        public AddressService(IAddressRepository addressRepository, ICustomerRepository customerRepository, IMapper mapper, ILogger<AddressService> logger)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _customerRepository = customerRepository;
            _logger = logger;
        }
        public async Task<AddressResponseDto> CreateAddressAsync(int customerId, CreateAddressDto dto)
        {
            _logger.LogInformation("Creating address for customer {CustomerId}", customerId);
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                _logger.LogWarning("Customer with id {CustomerId} not found", customerId);
                throw new NotFoundException($"Customer with id {customerId} is not found");
            }

            var address = _mapper.Map<DeliveryAddress>(dto);
            address.CustomerId = customerId;
            

            var created = await _addressRepository.CreateAddressAsync(address);
            _logger.LogInformation($"Created address: {created}");

            return _mapper.Map<AddressResponseDto>(created);
        }

        public async Task<AddressResponseDto> GetAddressByAddressIdAsync(int addressId)
        {
            _logger.LogInformation("Getting address with id {AddressId}", addressId);
            var address = await _addressRepository.GetAddressByAddressIdAsync(addressId)
                ?? throw new NotFoundException($"Address with id {addressId} is not found");

            return _mapper.Map<AddressResponseDto>(address);
        }


        public async Task<List<AddressResponseDto>> GetAllAddressesByCustomerIdAsync(int customerId)
        {
            _logger.LogInformation("Getting all addresses for customer with id {CustomerId}", customerId);
            var customerExists = await _customerRepository.GetCustomerByIdAsync(customerId);
            if (customerExists == null)
            {
                _logger.LogWarning("Customer with id {CustomerId} not found", customerId);
                throw new NotFoundException($"Customer with id {customerId} is not found");
            }

            var addresses = await _addressRepository.GetAllAddressesByCustomerIdAsync(customerId);
            return _mapper.Map<List<AddressResponseDto>>(addresses);
        }


        public async Task<AddressResponseDto> UpdateAddressAsync(int addressId, UpdateAddressDto dto)
        {
            _logger.LogInformation("Updating address with id {AddressId}", addressId);
            var address = await _addressRepository.GetAddressByAddressIdAsync(addressId)
                ?? throw new NotFoundException($"Address with id {addressId} is not found");
            _mapper.Map(dto, address);

            var updated = await _addressRepository.UpdateAddressAsync(address);
            _logger.LogInformation("Address with id {AddressId} updated successfully", addressId);

            return _mapper.Map<AddressResponseDto>(updated);
        }
    }
}
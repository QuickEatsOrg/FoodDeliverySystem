using AutoMapper;
using BCrypt.Net;
using FoodDelivery.API.DTOs.Sanjana;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Models;
using FoodDelivery.API.Repositories.Interfaces.Sanjana;
using FoodDelivery.API.Services.Interfaces.Sanjana;

namespace FoodDelivery.API.Services.Implementations.Sanjana
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            if (await _repository.EmailExistsAsync(createCustomerDto.CustomerEmail))
                throw new ConflictException($"{createCustomerDto.CustomerEmail} already exists");

            if (await _repository.PhoneNumberExistsAsync(createCustomerDto.CustomerPhone))
                throw new ConflictException($"{createCustomerDto.CustomerPhone} already exists");

            var customer = _mapper.Map<Customer>(createCustomerDto);
            customer.CustomerUnhashedPassword = BCrypt.Net.BCrypt.HashPassword(createCustomerDto.Password);

            customer.RoleId = 2;

            var created = await _repository.CreateCustomerAsync(customer);
            return await GetCustomerByIdAsync(created.CustomerId);
        }

        public async Task<IEnumerable<CustomerResponseDto>> GetAllCustomersAsync()
        {
            var customers = await _repository.GetAllCustomersAsync();
            var result = new List<CustomerResponseDto>();
            foreach (var customer in customers)
            {
                var response = _mapper.Map<CustomerResponseDto>(customer);
                response.TotalOrders = await _repository.GetCustomerOrderCountAsync(customer.CustomerId);
                response.Addresses = _mapper.Map<List<AddressResponseDto>>(customer.DeliveryAddresses);
                result.Add(response);
            }
            return result;
        }

        public async Task<CustomerResponseDto> GetCustomerByEmailAync(string email)
        {
            var existing = await _repository.GetCustomerByEmailAsync(email);
            if (existing == null)
                throw new NotFoundException($"{email} does not found");

            var response = _mapper.Map<CustomerResponseDto>(existing);
            response.TotalOrders = await _repository.GetCustomerOrderCountAsync(existing.CustomerId);
            response.Addresses = _mapper.Map<List<AddressResponseDto>>(existing.DeliveryAddresses);
            return response;
        }

        public async Task<CustomerResponseDto> GetCustomerByIdAsync(int id)
        {
            var existing = await _repository.GetCustomerByIdAsync(id);
            if (existing == null)
                throw new NotFoundException($"Customer with {id} does not exist");
            var response = _mapper.Map<CustomerResponseDto>(existing);
            response.TotalOrders = await _repository.GetCustomerOrderCountAsync(id);
            response.Addresses = _mapper.Map<List<AddressResponseDto>>(existing.DeliveryAddresses);
            return response;
        }

        public async Task<CustomerResponseDto> GetCustomerByPhoneAsync(string phone)
        {
            var existing = await _repository.GetCustomerByPhoneAsynnc(phone);
            if (existing == null)
                throw new NotFoundException($"{phone} does not exist");
            var response = _mapper.Map<CustomerResponseDto>(existing);
            response.TotalOrders = await _repository.GetCustomerOrderCountAsync(existing.CustomerId);
            response.Addresses = _mapper.Map<List<AddressResponseDto>>(existing.DeliveryAddresses);
            return response;
        }

        public async Task<CustomerResponseDto> UpdateCustomerAsync(int id, UpdateCustomerDto updateCustomerDto)
        {
            var existing = await _repository.GetCustomerByIdAsync(id);
            if (existing == null)
                throw new NotFoundException($"Customer with {id} does not exist");

            if (!string.IsNullOrWhiteSpace(updateCustomerDto.CustomerName))
                existing.CustomerName = updateCustomerDto.CustomerName;

            if (!string.IsNullOrWhiteSpace(updateCustomerDto.CustomerEmail))
            {
                if (await _repository.EmailExistsAsync(updateCustomerDto.CustomerEmail, id))
                    throw new ConflictException($"{updateCustomerDto.CustomerEmail} already exists");
                existing.CustomerEmail = updateCustomerDto.CustomerEmail;
            }

            if (!string.IsNullOrWhiteSpace(updateCustomerDto.CustomerPhone))
            {
                if (await _repository.PhoneNumberExistsAsync(updateCustomerDto.CustomerPhone, id))
                    throw new ConflictException($"{updateCustomerDto.CustomerPhone} already exists");
                existing.CustomerPhone = updateCustomerDto.CustomerPhone;
            }

            await _repository.UpdateCustomerAsync(existing);
            return await GetCustomerByIdAsync(existing.CustomerId);
        }
    }
}
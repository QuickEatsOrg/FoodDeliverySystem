using AutoMapper;
using FoodDelivery.API.DTOs;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Helpers;
using FoodDelivery.API.Models;
using FoodDelivery.API.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.API.Services;

public class AuthService : IAuthService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IDriverRepository _driverRepository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IMapper _mapper;
    private readonly JwtHelper _jwtHelper;

    private readonly IPasswordHasher<Customer> _customerPasswordHasher;
    private readonly IPasswordHasher<DeliveryDriver> _driverPasswordHasher;
    private readonly IPasswordHasher<Restaurant> _restaurantPasswordHasher;

    public AuthService(
        ICustomerRepository customerRepository,
        IDriverRepository driverRepository,
        IRestaurantRepository restaurantRepository,
        IMapper mapper,
        JwtHelper jwtHelper,
        IPasswordHasher<Customer> customerPasswordHasher,
        IPasswordHasher<DeliveryDriver> driverPasswordHasher,
        IPasswordHasher<Restaurant> restaurantPasswordHasher)
    {
        _customerRepository = customerRepository;
        _driverRepository = driverRepository;
        _restaurantRepository = restaurantRepository;
        _mapper = mapper;
        _jwtHelper = jwtHelper;
        _customerPasswordHasher = customerPasswordHasher;
        _driverPasswordHasher = driverPasswordHasher;
        _restaurantPasswordHasher = restaurantPasswordHasher;
    }

    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto)
    {
        var response = new ApiResponse<AuthResponseDto>();

        if (loginDto.Role.ToLower().Equals("admin"))
            response.Data = LoginAdmin(loginDto);

        if (loginDto.Role.ToLower().Equals("customer"))
            response.Data = await LoginCustomerAsync(loginDto);

        if (loginDto.Role.ToLower().Equals("deliverydriver"))
            response.Data =  await LoginDriverAsync(loginDto);

        if (loginDto.Role.ToLower().Equals("restaurant"))
            response.Data = await LoginRestaurantAsync(loginDto);

        response.Success = true;
        response.StatusCode = 200;
        response.Message = "Login Successful";
        response.Error = null;
        return response;
    }


    public async Task<bool> RegisterCustomerAsync(CreateCustomerDto customerDto)
    {
        if (await _customerRepository.EmailExistsAsync(customerDto.CustomerEmail))
            return false;

        var customer = _mapper.Map<Customer>(customerDto);

        if (!string.IsNullOrEmpty(customerDto.Password))
        {
            customer.CustomerHashedPassword = _customerPasswordHasher.HashPassword(customer, customerDto.Password);
            customer.CustomerUnhashedPassword = null;
        }

        await _customerRepository.CreateCustomerAsync(customer);
        return true;
    }

    public async Task<bool> RegisterDriverAsync(RegisterDriverDto driverDto)
    {
        var existing = await _driverRepository.GetDriverByEmailAsync(driverDto.DriverEmail);
        if (existing != null)
            return false;

        var driver = _mapper.Map<DeliveryDriver>(driverDto);

        if (!string.IsNullOrEmpty(driverDto.Password))
        {
            driver.DriverHashedPassword = _driverPasswordHasher.HashPassword(driver, driverDto.Password);
            driver.DriverUnhashedPassword = null;
        }

        await _driverRepository.CreateDriverAsync(driver);
        return true;
    }

    public async Task<bool> RegisterRestaurantAsync(RegisterRestaurantDto restaurantDto)
    {
        if (await _restaurantRepository.EmailExistsAsync(restaurantDto.RestaurantEmail))
            return false;

        var restaurant = _mapper.Map<Restaurant>(restaurantDto);

        if (!string.IsNullOrEmpty(restaurantDto.Password))
        {
            restaurant.RestaurantHashedPassword = _restaurantPasswordHasher.HashPassword(restaurant, restaurantDto.Password);
            restaurant.RestaurantUnhashedPassword = null;
        }

        await _restaurantRepository.AddAsync(restaurant);
        return true;
    }

    private AuthResponseDto? LoginAdmin(LoginDto loginDto)
    {
        if (loginDto.Email != "admin@gmail.com" || loginDto.Password != "Admin@123")
            return null;

        return new AuthResponseDto
        {
            Token = _jwtHelper.GenerateToken(0, "Admin", loginDto.Email, "Admin"),
            Email = loginDto.Email,
            Role = "Admin"
        };
    }

    private async Task<AuthResponseDto?> LoginCustomerAsync(LoginDto loginDto)
    {
        var customer = await _customerRepository.GetCustomerByEmailAsync(loginDto.Email);

        if (customer == null)
            throw new BadRequestException("Invalid email or password");

        var result = _customerPasswordHasher.VerifyHashedPassword(customer, customer.CustomerHashedPassword!, loginDto.Password);

        if (result == PasswordVerificationResult.Failed)
            throw new BadRequestException("Invalid email or password");

        return new AuthResponseDto
        {
            Token = _jwtHelper.GenerateToken(
                customer.CustomerId, customer.CustomerName,
                customer.CustomerEmail!, "Customer"),
            Email = customer.CustomerEmail!,
            Role = "Customer"
        };
    }

    private async Task<AuthResponseDto?> LoginDriverAsync(LoginDto loginDto)
    {
        var driver = await _driverRepository.GetDriverByEmailAsync(loginDto.Email);

        if (driver == null)
            throw new BadRequestException("Invalid email or password");

        var result = _driverPasswordHasher.VerifyHashedPassword(
            driver, driver.DriverHashedPassword!, loginDto.Password);

        if (result == PasswordVerificationResult.Failed)
            throw new BadRequestException("Invalid email or password");

        return new AuthResponseDto
        {
            Token = _jwtHelper.GenerateToken(
                driver.DriverId, driver.DriverName,
                driver.DriverEmail!, "DeliveryDriver"),
            Email = driver.DriverEmail!,
            Role = "DeliveryDriver"
        };
    }

    private async Task<AuthResponseDto?> LoginRestaurantAsync(LoginDto loginDto)
    {
        var restaurant = await _restaurantRepository.GetByEmailAsync(loginDto.Email);

        if (restaurant == null)
            throw new BadRequestException("Invalid email or password");

        var result = _restaurantPasswordHasher.VerifyHashedPassword(
            restaurant, restaurant.RestaurantHashedPassword!, loginDto.Password);

        if (result == PasswordVerificationResult.Failed)
            throw new BadRequestException("Invalid email or password");

        return new AuthResponseDto
        {
            Token = _jwtHelper.GenerateToken(
                restaurant.RestaurantId, restaurant.RestaurantName,
                restaurant.RestaurantEmail!, "Restaurant"),
            Email = restaurant.RestaurantEmail!,
            Role = "Restaurant"
        };
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using FoodDelivery.API.Data;
using FoodDelivery.API.DTOs.Sanjana;
using FoodDelivery.API.Models;
using FoodDelivery.API.Services.Interfaces.Sanjana;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FoodDelivery.API.Services.Implementations.Sanjana;

public class AuthService : IAuthService
{
    private readonly FoodDeliveryDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    private readonly IPasswordHasher<Customer> _customerPasswordHasher;
    private readonly IPasswordHasher<DeliveryDriver> _driverPasswordHasher;
    private readonly IPasswordHasher<Restaurant> _restaurantPasswordHasher;

    public AuthService(
        FoodDeliveryDbContext context,
        IConfiguration configuration,
        IMapper mapper,
        IPasswordHasher<Customer> customerPasswordHasher,
        IPasswordHasher<DeliveryDriver> driverPasswordHasher,
        IPasswordHasher<Restaurant> restaurantPasswordHasher)
    {
        _context = context;
        _configuration = configuration;
        _mapper = mapper;

        _customerPasswordHasher = customerPasswordHasher;
        _driverPasswordHasher = driverPasswordHasher;
        _restaurantPasswordHasher = restaurantPasswordHasher;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
    {
        if (loginDto.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            if (loginDto.Email == "admin@gmail.com"
                && loginDto.Password == "Admin@123")
            {
                string adminToken = GenerateJwtToken(
                    loginDto.Email,
                    "Admin",
                    "0");

                return new AuthResponseDto
                {
                    Token = adminToken,
                    Email = loginDto.Email,
                    Role = "Admin"
                };
            }

            return null;
        }

 

        if (loginDto.Role.Equals("Customer", StringComparison.OrdinalIgnoreCase))
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c =>
                    c.CustomerEmail == loginDto.Email);

            if (customer == null)
                return null;

            var passwordResult =
                _customerPasswordHasher.VerifyHashedPassword(
                    customer,
                    customer.CustomerHashedPassword!,
                    loginDto.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
                return null;

            string token = GenerateJwtToken(
                customer.CustomerEmail!,
                "Customer",
                customer.CustomerId.ToString());

            return new AuthResponseDto
            {
                Token = token,
                Email = customer.CustomerEmail!,
                Role = "Customer"
            };
        }

     

        if (loginDto.Role.Equals("Driver", StringComparison.OrdinalIgnoreCase))
        {
            var driver = await _context.DeliveryDrivers
                .FirstOrDefaultAsync(d =>
                    d.DriverEmail == loginDto.Email);

            if (driver == null)
                return null;

            var passwordResult =
                _driverPasswordHasher.VerifyHashedPassword(
                    driver,
                    driver.DriverHashedPassword!,
                    loginDto.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
                return null;

            string token = GenerateJwtToken(
                driver.DriverEmail!,
                "Driver",
                driver.DriverId.ToString());

            return new AuthResponseDto
            {
                Token = token,
                Email = driver.DriverEmail!,
                Role = "Driver"
            };
        }

        if (loginDto.Role.Equals("Restaurant", StringComparison.OrdinalIgnoreCase))
        {
            var restaurant = await _context.Restaurants
                .FirstOrDefaultAsync(r =>
                    r.RestaurantEmail == loginDto.Email);

            if (restaurant == null)
                return null;

            var passwordResult =
                _restaurantPasswordHasher.VerifyHashedPassword(
                    restaurant,
                    restaurant.RestaurantHashedPassword!,
                    loginDto.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
                return null;

            string token = GenerateJwtToken(
                restaurant.RestaurantEmail!,
                "Restaurant",
                restaurant.RestaurantId.ToString());

            return new AuthResponseDto
            {
                Token = token,
                Email = restaurant.RestaurantEmail!,
                Role = "Restaurant"
            };
        }

        return null;
    }

    public async Task<bool> RegisterCustomerAsync(
        CreateCustomerDto customerDto)
    {
        bool emailExists = await _context.Customers
            .AnyAsync(c =>
                c.CustomerEmail == customerDto.CustomerEmail);

        if (emailExists)
            return false;

        var customer = _mapper.Map<Customer>(customerDto);

        if (!string.IsNullOrEmpty(customerDto.Password))
        {
            customer.CustomerHashedPassword =
                _customerPasswordHasher.HashPassword(
                    customer,
                    customerDto.Password);

            customer.CustomerUnhashedPassword = null;
        }

        _context.Customers.Add(customer);

        return await _context.SaveChangesAsync() > 0;
    }


    public async Task<bool> RegisterDriverAsync(
        RegisterDriverDto driverDto)
    {
        bool emailExists = await _context.DeliveryDrivers
            .AnyAsync(d =>
                d.DriverEmail == driverDto.DriverEmail);

        if (emailExists)
            return false;

        var driver = _mapper.Map<DeliveryDriver>(driverDto);

        if (!string.IsNullOrEmpty(driverDto.Password))
        {
            driver.DriverHashedPassword =
                _driverPasswordHasher.HashPassword(
                    driver,
                    driverDto.Password);

            driver.DriverUnhashedPassword = null;
        }

        _context.DeliveryDrivers.Add(driver);

        return await _context.SaveChangesAsync() > 0;
    }



    public async Task<bool> RegisterRestaurantAsync(
        RegisterRestaurantDto restaurantDto)
    {
        bool emailExists = await _context.Restaurants
            .AnyAsync(r =>
                r.RestaurantEmail == restaurantDto.RestaurantEmail);

        if (emailExists)
            return false;

        var restaurant = _mapper.Map<Restaurant>(restaurantDto);

        if (!string.IsNullOrEmpty(restaurantDto.Password))
        {
            restaurant.RestaurantHashedPassword =
                _restaurantPasswordHasher.HashPassword(
                    restaurant,
                    restaurantDto.Password);

            restaurant.RestaurantUnhashedPassword = null;
        }

        _context.Restaurants.Add(restaurant);

        return await _context.SaveChangesAsync() > 0;
    }



    private string GenerateJwtToken(
        string email,
        string role,
        string userId)
    {
        var jwtSettings = _configuration.GetSection("Jwt");

        var key = Encoding.ASCII.GetBytes(
            jwtSettings["Key"]!);

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, email),

                new Claim(ClaimTypes.Role, role),

                new Claim(
                    ClaimTypes.NameIdentifier,
                    userId)
            }),

            Expires = DateTime.UtcNow.AddDays(7),

            Issuer = jwtSettings["Issuer"],

            Audience = jwtSettings["Audience"],

            SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FoodDelivery.API.Data;
using FoodDelivery.API.DTOs.Sanjana;
using FoodDelivery.API.Models;
using FoodDelivery.API.Services.Interfaces.Sanjana;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;

namespace FoodDelivery.API.Services.Implementations.Sanjana;

public class AuthService : IAuthService
{
    private readonly FoodDeliveryDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public AuthService(FoodDeliveryDbContext context, IConfiguration configuration, IMapper mapper)
    {
        _context = context;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
    {
        bool isValid = false;
        string userRole = loginDto.Role;
        string userId = string.Empty;

        // Hardcoded Admin
        if (loginDto.Role == "Admin")
        {
            if (loginDto.Email == "admin@fooddelivery.com" && loginDto.Password == "Admin@123")
            {
                isValid = true;
                userId = "admin";
            }
        }
        else if (loginDto.Role == "Customer")
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerEmail == loginDto.Email && c.Password == loginDto.Password);
            if (customer != null)
            {
                isValid = true;
                userId = customer.CustomerId.ToString();
            }
        }
        else if (loginDto.Role == "DeliveryDriver")
        {
            var driver = await _context.DeliveryDrivers.FirstOrDefaultAsync(d => d.DriverEmail == loginDto.Email && d.Password == loginDto.Password);
            if (driver != null)
            {
                isValid = true;
                userId = driver.DriverId.ToString();
            }
        }
        else if (loginDto.Role == "Restaurant")
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantEmail == loginDto.Email && r.Password == loginDto.Password);
            if (restaurant != null)
            {
                isValid = true;
                userId = restaurant.RestaurantId.ToString();
            }
        }

        if (isValid)
        {
            var token = GenerateJwtToken(loginDto.Email, userRole, userId);
            return new AuthResponseDto
            {
                Token = token,
                Email = loginDto.Email,
                Role = userRole
            };
        }

        return null;
    }

    public async Task<bool> RegisterCustomerAsync(CreateCustomerDto customerDto)
    {
        if (await _context.Customers.AnyAsync(c => c.CustomerEmail == customerDto.CustomerEmail))
            return false;

        var customer = _mapper.Map<Customer>(customerDto);
        _context.Customers.Add(customer);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RegisterDriverAsync(RegisterDriverDto driverDto)
    {
        if (await _context.DeliveryDrivers.AnyAsync(d => d.DriverEmail == driverDto.DriverEmail))
            return false;

        var driver = _mapper.Map<DeliveryDriver>(driverDto);
        _context.DeliveryDrivers.Add(driver);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RegisterRestaurantAsync(RegisterRestaurantDto restaurantDto)
    {
        if (await _context.Restaurants.AnyAsync(r => r.RestaurantEmail == restaurantDto.RestaurantEmail))
            return false;

        var restaurant = _mapper.Map<Restaurant>(restaurantDto);
        _context.Restaurants.Add(restaurant);
        return await _context.SaveChangesAsync() > 0;
    }

    private string GenerateJwtToken(string email, string role, string userId)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.NameIdentifier, userId)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

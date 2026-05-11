using FluentValidation;
using FluentValidation.AspNetCore;
using FoodDelivery.API.Data;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Mapper;
using FoodDelivery.API.Models;
using FoodDelivery.API.Repositories.Implementations.Sanjana;
using FoodDelivery.API.Repositories.Interfaces.Sanjana;
using FoodDelivery.API.Services.Implementations.Sanjana;
using FoodDelivery.API.Services.Interfaces.Sanjana;
using FoodDelivery.API.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

//sanjana{start
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<FoodDeliveryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new NotFoundException("Connection string not found")));

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IPasswordHasher<Customer>, PasswordHasher<Customer>>();

builder.Services.AddScoped<IPasswordHasher<DeliveryDriver>, PasswordHasher<DeliveryDriver>>();

builder.Services.AddScoped<IPasswordHasher<Restaurant>, PasswordHasher<Restaurant>>();

builder.Services.AddValidatorsFromAssemblyContaining<RegisterCustomerDtoValidator>();

builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddScoped<IAddressService, AddressService>();

builder.Services.AddScoped<IAddressRepository, AddressRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAutoMapper(options =>
{
    options.AddProfile<CustomerProfile>();
    options.AddProfile<AddressProfile>();
    options.AddProfile<AuthProfile>();
});

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"));
    options.AddPolicy("DriverOnly", policy => policy.RequireRole("DeliveryDriver"));
    options.AddPolicy("RestaurantOnly", policy => policy.RequireRole("Restaurant"));
});
//sanjana }end

// Swagger

builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
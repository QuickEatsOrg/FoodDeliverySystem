using System.Text;
using FoodDelivery.API.Data;
using FoodDelivery.API.Helpers;
using FoodDelivery.API.Repositories.Implementations;
using FoodDelivery.API.Repositories.Implementations.Tushar;
using FoodDelivery.API.Repositories.Interfaces.Tushar;
using FoodDelivery.API.Services.Implementations.Neha;
using FoodDelivery.API.Services.Implementations.Tushar;
using FoodDelivery.API.Services.Interfaces.Neha;
using FoodDelivery.API.Services.Interfaces.Tushar;
using FoodDelivery.API.Validations.Neha;
using FoodDelivery.API.Validators.Tushar;
using FoodService.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

using FoodDelivery.API.Repositories;
using FoodDelivery.API.Services;
using FoodDelivery.API.Mappings;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.API.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<FoodDeliveryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMvc", policy =>
    {
        policy.WithOrigins("https://localhost:7056")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateDriverDtoValidator>();

builder.Services.AddEndpointsApiExplorer();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<JwtHelper>();

builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();

builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
builder.Services.AddScoped<IDriverAuthService, DriverAuthService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IRatingService, RatingService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Database connection
builder.Services.AddDbContext<FoodDeliveryDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Dependency Injection
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();

 var app = builder.Build();

// Configure middleware

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("AllowMvc");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.UseAuthorization();

app.MapControllers();

 app.Run();

app.Run();


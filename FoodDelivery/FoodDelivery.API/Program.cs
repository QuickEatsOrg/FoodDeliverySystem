using FluentValidation;
using FluentValidation.AspNetCore;
using FoodDelivery.API.Data;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Filters;
using FoodDelivery.API.Helpers;
using FoodDelivery.API.Mappings;
using FoodDelivery.API.Middleware;
using FoodDelivery.API.Models;
using FoodDelivery.API.Repositories;
using FoodDelivery.API.Repositories.Implementations;
using FoodDelivery.API.Repositories.Implementations.Neha;
using FoodDelivery.API.Repositories.Implementations.Sahil;
using FoodDelivery.API.Repositories.Implementations.Tushar;
using FoodDelivery.API.Repositories.Interfaces.Sahil;
using FoodDelivery.API.Repositories.Interfaces.Tushar;
using FoodDelivery.API.Services;
using FoodDelivery.API.Services.Implementations.Neha;
using FoodDelivery.API.Services.Implementations.Sahil;
using FoodDelivery.API.Services.Implementations.Tushar;
using FoodDelivery.API.Services.Interfaces.Neha;
using FoodDelivery.API.Services.Interfaces.Sahil;
using FoodDelivery.API.Services.Interfaces.Tushar;
using FoodDelivery.API.Validations.Neha;
using FoodDelivery.API.Validators.Tushar;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

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
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// ── AutoMapper ────────────────────────────────────────────────────────────
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ── Helpers ───────────────────────────────────────────────────────────────
builder.Services.AddScoped<JwtHelper>();

// ── Session (required by CartRepository) ─────────────────────────────────
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ── HTTP Client (CartService / OrderService call other APIs) ──────────────
builder.Services.AddHttpClient();

// ── Tushar's services ─────────────────────────────────────────────────────
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
builder.Services.AddScoped<IDriverAuthService, DriverAuthService>();

// ── Neha's services ───────────────────────────────────────────────────────
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IRatingService, RatingService>();

// ── Anshika's services ────────────────────────────────────────────────────
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();

// ── SAHIL's services – Cart, Order, Payment ───────────────────────────────
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

// ─────────────────────────────────────────────────────────────────────────
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("AllowMvc");

// Session MUST come before Authentication
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

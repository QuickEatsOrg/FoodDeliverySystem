using FluentValidation;
using FluentValidation.AspNetCore;
using FoodDelivery.API.Mappings;
using FoodDelivery.API.Models;
using FoodDelivery.API.Repositories;
using FoodDelivery.API.Repositories.Implementations;
using FoodDelivery.API.Services.Implementations.Neha;
using FoodDelivery.API.Services.Interfaces.Neha;
using FoodDelivery.API.Validations.Neha;
using FoodService.Middleware;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

builder.Services.AddDbContext<FoodDeliveryDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICouponRepository, CouponRepository>();

builder.Services.AddScoped<ICouponService, CouponService>();

builder.Services.AddScoped<IRatingRepository, RatingRepository>();

builder.Services.AddScoped<IRatingService, RatingService>();

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<CreateCouponDtoValidator>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<CreateCouponDtoValidator>();
var app = builder.Build();

// Global Exception Handling

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;

        context.Response.ContentType = "text/plain";

        await context.Response.WriteAsync(
            "An unexpected error occurred.");
    });
});

// Configure middleware

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
using FoodDelivery.MVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSession();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7292/";

builder.Services.AddHttpClient<CustomerService>(
    client => client.BaseAddress = new Uri(new Uri(apiBaseUrl), "api/customer"));
builder.Services.AddHttpClient<AuthService>(
    client => client.BaseAddress = new Uri(apiBaseUrl));
builder.Services.AddHttpClient<RestaurantService>(
    client => client.BaseAddress = new Uri(apiBaseUrl));
builder.Services.AddHttpClient<MenuItemService>(
    client => client.BaseAddress = new Uri(apiBaseUrl));
builder.Services.AddHttpClient<RatingDisplayService>(
    client => client.BaseAddress = new Uri(apiBaseUrl));
builder.Services.AddHttpClient<DirectoryService>(
    client => client.BaseAddress = new Uri(apiBaseUrl));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

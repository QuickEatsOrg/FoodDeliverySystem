# This script creates the FoodDelivery solution structure with the exact root-level layout requested
# Run: Set-ExecutionPolicy -Scope Process Bypass; .\setup.ps1

$ProjectName = "FoodDelivery"

function Ensure-Directory {
    param([string]$Path)
    New-Item -ItemType Directory -Path $Path -Force | Out-Null
}

function Ensure-File {
    param([string]$Path)
    New-Item -ItemType File -Path $Path -Force | Out-Null
}

if (Test-Path $ProjectName) {
    Set-Location $ProjectName
} else {
    Ensure-Directory $ProjectName
    Set-Location $ProjectName
}

Ensure-File "README.md"
Ensure-File ".gitignore"
Ensure-File "FoodDeliveryDB_Sprint1.sql"

Ensure-Directory "docs/Sprint-Reports"
Ensure-File "docs/README.md"
Ensure-File "docs/API-Contracts.md"
Ensure-File "docs/Database-Schema.md"

dotnet new sln -n FoodDelivery --force | Out-Null

dotnet new webapi -n FoodDelivery.API -o FoodDelivery.API --force | Out-Null
dotnet new mvc -n FoodDelivery.MVC -o FoodDelivery.MVC --force | Out-Null
dotnet new xunit -n FoodDelivery.Tests -o FoodDelivery.Tests --force | Out-Null

dotnet sln add "FoodDelivery.API/FoodDelivery.API.csproj" | Out-Null
dotnet sln add "FoodDelivery.MVC/FoodDelivery.MVC.csproj" | Out-Null
dotnet sln add "FoodDelivery.Tests/FoodDelivery.Tests.csproj" | Out-Null

Push-Location "FoodDelivery.API"

$ApiFolders = @(
    "Controllers/Sahil",
    "Controllers/Tushar",
    "Controllers/Sanjana",
    "Controllers/Anshika",
    "Controllers/Neha",
    "Models",
    "DTOs/Sahil",
    "DTOs/Tushar",
    "DTOs/Sanjana",
    "DTOs/Anshika",
    "DTOs/Neha",
    "Repositories/Interfaces/Sahil",
    "Repositories/Interfaces/Tushar",
    "Repositories/Interfaces/Sanjana",
    "Repositories/Interfaces/Anshika",
    "Repositories/Interfaces/Neha",
    "Repositories/Implementations/Sahil",
    "Repositories/Implementations/Tushar",
    "Repositories/Implementations/Sanjana",
    "Repositories/Implementations/Anshika",
    "Repositories/Implementations/Neha",
    "Services/Interfaces/Sahil",
    "Services/Interfaces/Tushar",
    "Services/Interfaces/Sanjana",
    "Services/Interfaces/Anshika",
    "Services/Interfaces/Neha",
    "Services/Implementations/Sahil",
    "Services/Implementations/Tushar",
    "Services/Implementations/Sanjana",
    "Services/Implementations/Anshika",
    "Services/Implementations/Neha",
    "Helpers",
    "Middleware",
    "Hubs",
    "Exceptions",
    "Validators",
    "Configuration"
)

foreach ($folder in $ApiFolders) {
    Ensure-Directory $folder
}

$ApiFiles = @(
    "Controllers/Sahil/CartController.cs",
    "Controllers/Sahil/OrderController.cs",
    "Controllers/Sahil/PaymentController.cs",
    "Controllers/Tushar/DeliveryController.cs",
    "Controllers/Tushar/TrackingController.cs",
    "Controllers/Tushar/DriverController.cs",
    "Controllers/Sanjana/AuthController.cs",
    "Controllers/Sanjana/CustomerController.cs",
    "Controllers/Sanjana/AddressController.cs",
    "Controllers/Sanjana/NotificationController.cs",
    "Controllers/Anshika/RestaurantController.cs",
    "Controllers/Anshika/MenuItemController.cs",
    "Controllers/Neha/CouponController.cs",
    "Controllers/Neha/RatingController.cs",
    "Controllers/Neha/ReportController.cs",
    "Models/FoodDeliveryDbContext.cs",
    "Models/User.cs",
    "Models/DeliveryAddress.cs",
    "Models/Restaurant.cs",
    "Models/MenuItem.cs",
    "Models/Order.cs",
    "Models/OrderItem.cs",
    "Models/DeliveryDriver.cs",
    "Models/DeliveryAssignment.cs",
    "Models/Payment.cs",
    "Models/Coupon.cs",
    "Models/OrdersCoupon.cs",
    "Models/Rating.cs",
    "Models/Wishlist.cs",
    "Models/Notification.cs",
    "DTOs/Sahil/CartDtos.cs",
    "DTOs/Sahil/OrderDtos.cs",
    "DTOs/Sahil/PaymentDtos.cs",
    "DTOs/Tushar/DeliveryDtos.cs",
    "DTOs/Tushar/DriverDtos.cs",
    "DTOs/Sanjana/AuthDtos.cs",
    "DTOs/Sanjana/CustomerDtos.cs",
    "DTOs/Sanjana/AddressDtos.cs",
    "DTOs/Sanjana/NotificationDtos.cs",
    "DTOs/Anshika/RestaurantDtos.cs",
    "DTOs/Neha/CouponDtos.cs",
    "DTOs/Neha/RatingDtos.cs",
    "Repositories/Interfaces/Sahil/ICartRepository.cs",
    "Repositories/Interfaces/Sahil/IOrderRepository.cs",
    "Repositories/Interfaces/Sahil/IOrderItemRepository.cs",
    "Repositories/Interfaces/Sahil/IPaymentRepository.cs",
    "Repositories/Interfaces/Tushar/IDeliveryDriverRepository.cs",
    "Repositories/Interfaces/Tushar/IDeliveryAssignmentRepository.cs",
    "Repositories/Interfaces/Sanjana/IUserRepository.cs",
    "Repositories/Interfaces/Sanjana/IAddressRepository.cs",
    "Repositories/Interfaces/Sanjana/INotificationRepository.cs",
    "Repositories/Interfaces/Anshika/IRestaurantRepository.cs",
    "Repositories/Interfaces/Anshika/IMenuItemRepository.cs",
    "Repositories/Interfaces/Neha/ICouponRepository.cs",
    "Repositories/Interfaces/Neha/IOrdersCouponRepository.cs",
    "Repositories/Interfaces/Neha/IRatingRepository.cs",
    "Repositories/Implementations/Sahil/CartRepository.cs",
    "Repositories/Implementations/Sahil/OrderRepository.cs",
    "Repositories/Implementations/Sahil/OrderItemRepository.cs",
    "Repositories/Implementations/Sahil/PaymentRepository.cs",
    "Repositories/Implementations/Tushar/DeliveryDriverRepository.cs",
    "Repositories/Implementations/Tushar/DeliveryAssignmentRepository.cs",
    "Repositories/Implementations/Sanjana/UserRepository.cs",
    "Repositories/Implementations/Sanjana/AddressRepository.cs",
    "Repositories/Implementations/Sanjana/NotificationRepository.cs",
    "Repositories/Implementations/Anshika/RestaurantRepository.cs",
    "Repositories/Implementations/Anshika/MenuItemRepository.cs",
    "Repositories/Implementations/Neha/CouponRepository.cs",
    "Repositories/Implementations/Neha/OrdersCouponRepository.cs",
    "Repositories/Implementations/Neha/RatingRepository.cs",
    "Services/Interfaces/Sahil/IOrderService.cs",
    "Services/Interfaces/Sahil/ICartService.cs",
    "Services/Interfaces/Sahil/IPaymentService.cs",
    "Services/Interfaces/Tushar/IDeliveryService.cs",
    "Services/Interfaces/Tushar/IDriverService.cs",
    "Services/Interfaces/Sanjana/IAuthService.cs",
    "Services/Interfaces/Sanjana/IUserService.cs",
    "Services/Interfaces/Sanjana/IAddressService.cs",
    "Services/Interfaces/Sanjana/INotificationService.cs",
    "Services/Interfaces/Anshika/IRestaurantService.cs",
    "Services/Interfaces/Anshika/IMenuItemService.cs",
    "Services/Interfaces/Neha/ICouponService.cs",
    "Services/Interfaces/Neha/IRatingService.cs",
    "Services/Implementations/Sahil/OrderService.cs",
    "Services/Implementations/Sahil/CartService.cs",
    "Services/Implementations/Sahil/PaymentService.cs",
    "Services/Implementations/Tushar/DeliveryService.cs",
    "Services/Implementations/Tushar/DriverService.cs",
    "Services/Implementations/Sanjana/AuthService.cs",
    "Services/Implementations/Sanjana/UserService.cs",
    "Services/Implementations/Sanjana/AddressService.cs",
    "Services/Implementations/Sanjana/NotificationService.cs",
    "Services/Implementations/Anshika/RestaurantService.cs",
    "Services/Implementations/Anshika/MenuItemService.cs",
    "Services/Implementations/Neha/CouponService.cs",
    "Services/Implementations/Neha/RatingService.cs",
    "Helpers/JwtHelper.cs",
    "Helpers/PasswordHasher.cs",
    "Helpers/UserNumberGenerator.cs",
    "Helpers/OrderNumberGenerator.cs",
    "Helpers/PaymentNumberGenerator.cs",
    "Helpers/RestaurantNumberGenerator.cs",
    "Helpers/DriverNumberGenerator.cs",
    "Helpers/CouponCodeGenerator.cs",
    "Helpers/DistanceCalculator.cs",
    "Helpers/FileValidationHelper.cs",
    "Helpers/DateTimeHelper.cs",
    "Middleware/GlobalExceptionMiddleware.cs",
    "Middleware/JwtCookieMiddleware.cs",
    "Middleware/RequestLoggingMiddleware.cs",
    "Hubs/TrackingHub.cs",
    "Exceptions/NotFoundException.cs",
    "Exceptions/BadRequestException.cs",
    "Exceptions/ForbiddenException.cs",
    "Exceptions/UnauthorizedException.cs",
    "Exceptions/PaymentFailedException.cs",
    "Validators/RegisterDtoValidator.cs",
    "Validators/CreateOrderDtoValidator.cs",
    "Validators/CreateRestaurantDtoValidator.cs",
    "Validators/CreateCouponDtoValidator.cs",
    "Validators/InitiatePaymentDtoValidator.cs",
    "Configuration/JwtSettings.cs",
    "Configuration/AppSettings.cs",
    "Configuration/SignalRSettings.cs"
)

foreach ($file in $ApiFiles) {
    Ensure-File $file
}

Pop-Location

Push-Location "FoodDelivery.MVC"

$MvcFolders = @(
    "Controllers",
    "Views/Shared",
    "Views/Account",
    "Views/Customer",
    "Views/Order",
    "Views/Restaurant",
    "Views/Driver",
    "Views/Admin",
    "Views/Rating",
    "wwwroot/css",
    "wwwroot/js",
    "wwwroot/images",
    "wwwroot/uploads"
)

foreach ($folder in $MvcFolders) {
    Ensure-Directory $folder
}

$MvcFiles = @(
    "Views/Shared/_Layout.cshtml",
    "Views/Shared/_NotificationBell.cshtml",
    "Views/Shared/Error.cshtml",
    "Views/Account/Login.cshtml",
    "Views/Account/Register.cshtml",
    "Views/Account/ForgotPassword.cshtml",
    "Views/Account/ResetPassword.cshtml",
    "Views/Customer/Profile.cshtml",
    "Views/Customer/Addresses.cshtml",
    "Views/Customer/Notifications.cshtml",
    "Views/Customer/EditAddress.cshtml",
    "Views/Order/Cart.cshtml",
    "Views/Order/Checkout.cshtml",
    "Views/Order/OrderHistory.cshtml",
    "Views/Order/OrderDetails.cshtml",
    "Views/Order/Payment.cshtml",
    "Views/Restaurant/Index.cshtml",
    "Views/Restaurant/Details.cshtml",
    "Views/Restaurant/Dashboard.cshtml",
    "Views/Restaurant/Menu.cshtml",
    "Views/Restaurant/EditMenuItem.cshtml",
    "Views/Restaurant/CreateMenuItem.cshtml",
    "Views/Driver/Dashboard.cshtml",
    "Views/Driver/MyDeliveries.cshtml",
    "Views/Driver/DeliveryDetails.cshtml",
    "Views/Driver/Earnings.cshtml",
    "Views/Admin/Dashboard.cshtml",
    "Views/Admin/Users.cshtml",
    "Views/Admin/Coupons.cshtml",
    "Views/Admin/Reports.cshtml",
    "Views/Admin/Restaurants.cshtml",
    "Views/Admin/Drivers.cshtml",
    "Views/Admin/Payments.cshtml",
    "Views/Rating/RateOrder.cshtml",
    "Views/Rating/MyRatings.cshtml",
    "wwwroot/css/site.css",
    "wwwroot/js/site.js"
)

foreach ($file in $MvcFiles) {
    Ensure-File $file
}

Pop-Location

Push-Location "FoodDelivery.Tests"

$TestFolders = @(
    "UnitTests/Services",
    "UnitTests/Helpers",
    "IntegrationTests",
    "Helpers"
)

foreach ($folder in $TestFolders) {
    Ensure-Directory $folder
}

$TestFiles = @(
    "UnitTests/Services/OrderServiceTests.cs",
    "UnitTests/Services/CartServiceTests.cs",
    "UnitTests/Services/PaymentServiceTests.cs",
    "UnitTests/Services/DeliveryServiceTests.cs",
    "UnitTests/Services/DriverServiceTests.cs",
    "UnitTests/Services/AuthServiceTests.cs",
    "UnitTests/Services/UserServiceTests.cs",
    "UnitTests/Services/AddressServiceTests.cs",
    "UnitTests/Services/RestaurantServiceTests.cs",
    "UnitTests/Services/MenuItemServiceTests.cs",
    "UnitTests/Services/CouponServiceTests.cs",
    "UnitTests/Services/RatingServiceTests.cs",
    "UnitTests/Helpers/JwtHelperTests.cs",
    "UnitTests/Helpers/DistanceCalculatorTests.cs",
    "IntegrationTests/AuthFlowTests.cs",
    "IntegrationTests/OrderFlowTests.cs",
    "IntegrationTests/DeliveryFlowTests.cs",
    "IntegrationTests/PaymentFlowTests.cs",
    "IntegrationTests/RestaurantFlowTests.cs",
    "Helpers/TestDataFactory.cs",
    "Helpers/MockDbContextFactory.cs"
)

foreach ($file in $TestFiles) {
    Ensure-File $file
}

Pop-Location

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "FoodDelivery Solution Created Successfully" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Open FoodDelivery.sln from the FoodDelivery folder" -ForegroundColor Green
Write-Host ""

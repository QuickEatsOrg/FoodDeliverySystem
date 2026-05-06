# This script creates the FoodDelivery solution structure with team-based modular architecture
# Run: Set-ExecutionPolicy -Scope Process Bypass; .\setup.ps1
# ==========================================
# FOOD DELIVERY SOLUTION GENERATOR
# TEAM BASED MODULAR ARCHITECTURE
# ==========================================

$ProjectName = "FoodDelivery"

# ==========================================
# ROOT
# ==========================================

New-Item -ItemType Directory -Path $ProjectName -Force | Out-Null
Set-Location $ProjectName

New-Item README.md -ItemType File -Force | Out-Null
New-Item .gitignore -ItemType File -Force | Out-Null
New-Item FoodDeliveryDB_Sprint1.sql -ItemType File -Force | Out-Null

# ==========================================
# DOCS
# ==========================================

New-Item -ItemType Directory -Path "docs/Sprint-Reports" -Force | Out-Null

New-Item "docs/README.md" -ItemType File -Force | Out-Null
New-Item "docs/API-Contracts.md" -ItemType File -Force | Out-Null
New-Item "docs/Database-Schema.md" -ItemType File -Force | Out-Null

# ==========================================
# SOLUTION
# ==========================================

dotnet new sln -n FoodDelivery --force | Out-Null

# ==========================================
# PROJECTS
# ==========================================

dotnet new webapi -n FoodDelivery.API --force | Out-Null
dotnet new mvc -n FoodDelivery.MVC --force | Out-Null
dotnet new xunit -n FoodDelivery.Tests --force | Out-Null

# ==========================================
# ADD TO SOLUTION
# ==========================================

dotnet sln add FoodDelivery.API/FoodDelivery.API.csproj | Out-Null
dotnet sln add FoodDelivery.MVC/FoodDelivery.MVC.csproj | Out-Null
dotnet sln add FoodDelivery.Tests/FoodDelivery.Tests.csproj | Out-Null

# ==========================================
# API PROJECT
# ==========================================

Set-Location FoodDelivery.API

# ==========================================
# API FOLDERS
# ==========================================

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
    New-Item -ItemType Directory -Path $folder -Force | Out-Null
}

# ==========================================
# API FILES
# ==========================================

$ApiFiles = @(

# CONTROLLERS

"Controllers/Sahil/CartController.cs",
"Controllers/Sahil/OrderController.cs",

"Controllers/Tushar/DeliveryController.cs",
"Controllers/Tushar/TrackingController.cs",
"Controllers/Tushar/PaymentController.cs",

"Controllers/Sanjana/AuthController.cs",
"Controllers/Sanjana/CustomerController.cs",
"Controllers/Sanjana/AddressController.cs",
"Controllers/Sanjana/WishlistController.cs",
"Controllers/Sanjana/NotificationController.cs",
"Controllers/Sanjana/AnalyticsController.cs",
"Controllers/Sanjana/AdminUserController.cs",

"Controllers/Anshika/RestaurantController.cs",
"Controllers/Anshika/MenuItemController.cs",

"Controllers/Neha/CouponController.cs",
"Controllers/Neha/RatingController.cs",
"Controllers/Neha/ReportController.cs",

# DTOS

"DTOs/Sahil/CartDtos.cs",
"DTOs/Sahil/OrderDtos.cs",

"DTOs/Tushar/DeliveryDtos.cs",
"DTOs/Tushar/PaymentDtos.cs",

"DTOs/Sanjana/AuthDtos.cs",
"DTOs/Sanjana/CustomerDtos.cs",
"DTOs/Sanjana/AddressDtos.cs",
"DTOs/Sanjana/WishlistDtos.cs",
"DTOs/Sanjana/NotificationDtos.cs",
"DTOs/Sanjana/AnalyticsDtos.cs",
"DTOs/Sanjana/AdminUserDtos.cs",

"DTOs/Anshika/RestaurantDtos.cs",

"DTOs/Neha/CouponDtos.cs",
"DTOs/Neha/RatingDtos.cs",

# REPOSITORY INTERFACES

"Repositories/Interfaces/Sahil/ICartRepository.cs",
"Repositories/Interfaces/Sahil/IOrderRepository.cs",
"Repositories/Interfaces/Sahil/IOrderItemRepository.cs",

"Repositories/Interfaces/Tushar/IDeliveryDriverRepository.cs",
"Repositories/Interfaces/Tushar/IDeliveryAssignmentRepository.cs",
"Repositories/Interfaces/Tushar/IPaymentRepository.cs",

"Repositories/Interfaces/Sanjana/IUserRepository.cs",
"Repositories/Interfaces/Sanjana/IAddressRepository.cs",
"Repositories/Interfaces/Sanjana/IWishlistRepository.cs",
"Repositories/Interfaces/Sanjana/INotificationRepository.cs",
"Repositories/Interfaces/Sanjana/IActivityLogRepository.cs",

"Repositories/Interfaces/Anshika/IRestaurantRepository.cs",
"Repositories/Interfaces/Anshika/IMenuItemRepository.cs",

"Repositories/Interfaces/Neha/ICouponRepository.cs",
"Repositories/Interfaces/Neha/IOrdersCouponRepository.cs",
"Repositories/Interfaces/Neha/IRatingRepository.cs",

# REPOSITORY IMPLEMENTATIONS

"Repositories/Implementations/Sahil/CartRepository.cs",
"Repositories/Implementations/Sahil/OrderRepository.cs",
"Repositories/Implementations/Sahil/OrderItemRepository.cs",

"Repositories/Implementations/Tushar/DeliveryDriverRepository.cs",
"Repositories/Implementations/Tushar/DeliveryAssignmentRepository.cs",
"Repositories/Implementations/Tushar/PaymentRepository.cs",

"Repositories/Implementations/Sanjana/UserRepository.cs",
"Repositories/Implementations/Sanjana/AddressRepository.cs",
"Repositories/Implementations/Sanjana/WishlistRepository.cs",
"Repositories/Implementations/Sanjana/NotificationRepository.cs",
"Repositories/Implementations/Sanjana/ActivityLogRepository.cs",

"Repositories/Implementations/Anshika/RestaurantRepository.cs",
"Repositories/Implementations/Anshika/MenuItemRepository.cs",

"Repositories/Implementations/Neha/CouponRepository.cs",
"Repositories/Implementations/Neha/OrdersCouponRepository.cs",
"Repositories/Implementations/Neha/RatingRepository.cs",

# SERVICE INTERFACES

"Services/Interfaces/Sahil/IOrderService.cs",
"Services/Interfaces/Sahil/ICartService.cs",

"Services/Interfaces/Tushar/IDeliveryService.cs",
"Services/Interfaces/Tushar/IDriverAssignmentService.cs",
"Services/Interfaces/Tushar/IPaymentService.cs",

"Services/Interfaces/Sanjana/IAuthService.cs",
"Services/Interfaces/Sanjana/IUserService.cs",
"Services/Interfaces/Sanjana/IAddressService.cs",
"Services/Interfaces/Sanjana/IWishlistService.cs",
"Services/Interfaces/Sanjana/INotificationService.cs",
"Services/Interfaces/Sanjana/IAnalyticsService.cs",

"Services/Interfaces/Anshika/IRestaurantService.cs",
"Services/Interfaces/Anshika/IMenuItemService.cs",

"Services/Interfaces/Neha/ICouponService.cs",
"Services/Interfaces/Neha/IRatingService.cs",

# SERVICE IMPLEMENTATIONS

"Services/Implementations/Sahil/OrderService.cs",
"Services/Implementations/Sahil/CartService.cs",

"Services/Implementations/Tushar/DeliveryService.cs",
"Services/Implementations/Tushar/DriverAssignmentService.cs",
"Services/Implementations/Tushar/PaymentService.cs",

"Services/Implementations/Sanjana/AuthService.cs",
"Services/Implementations/Sanjana/UserService.cs",
"Services/Implementations/Sanjana/AddressService.cs",
"Services/Implementations/Sanjana/WishlistService.cs",
"Services/Implementations/Sanjana/NotificationService.cs",
"Services/Implementations/Sanjana/AnalyticsService.cs",

"Services/Implementations/Anshika/RestaurantService.cs",
"Services/Implementations/Anshika/MenuItemService.cs",

"Services/Implementations/Neha/CouponService.cs",
"Services/Implementations/Neha/RatingService.cs",

# HELPERS

"Helpers/JwtHelper.cs",
"Helpers/PasswordHasher.cs",
"Helpers/UserNumberGenerator.cs",
"Helpers/OrderNumberGenerator.cs",
"Helpers/PaymentNumberGenerator.cs",
"Helpers/DistanceCalculator.cs",
"Helpers/FileValidationHelper.cs",
"Helpers/RestaurantNumberGenerator.cs",
"Helpers/DriverNumberGenerator.cs",
"Helpers/CouponCodeGenerator.cs",
"Helpers/DateTimeHelper.cs",

# MIDDLEWARE

"Middleware/GlobalExceptionMiddleware.cs",
"Middleware/JwtMiddleware.cs",
"Middleware/RequestLoggingMiddleware.cs",

# HUBS

"Hubs/TrackingHub.cs",

# EXCEPTIONS

"Exceptions/NotFoundException.cs",
"Exceptions/BadRequestException.cs",
"Exceptions/ForbiddenException.cs",
"Exceptions/UnauthorizedException.cs",
"Exceptions/PaymentFailedException.cs",

# VALIDATORS

"Validators/RegisterDtoValidator.cs",
"Validators/CreateOrderDtoValidator.cs",
"Validators/CreateRestaurantDtoValidator.cs",
"Validators/CreateCouponDtoValidator.cs",
"Validators/InitiatePaymentDtoValidator.cs",

# CONFIGURATION

"Configuration/JwtSettings.cs",
"Configuration/AppSettings.cs",
"Configuration/PaymentGatewaySettings.cs",
"Configuration/SignalRSettings.cs"
)

foreach ($file in $ApiFiles) {
    New-Item -ItemType File -Path $file -Force | Out-Null
}

Set-Location ..

# ==========================================
# MVC PROJECT
# ==========================================

Set-Location FoodDelivery.MVC

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

"ViewModels",
"Services",
"Helpers",

"wwwroot/css",
"wwwroot/js",
"wwwroot/images",
"wwwroot/uploads/restaurants",
"wwwroot/uploads/menu",
"wwwroot/uploads/drivers"
)

foreach ($folder in $MvcFolders) {
    New-Item -ItemType Directory -Path $folder -Force | Out-Null
}

$MvcFiles = @(

# SHARED
"Views/Shared/_Layout.cshtml",
"Views/Shared/_NotificationBell.cshtml",
"Views/Shared/_ValidationScriptsPartial.cshtml",
"Views/Shared/Error.cshtml",

# ACCOUNT
"Views/Account/Login.cshtml",
"Views/Account/Register.cshtml",
"Views/Account/ForgotPassword.cshtml",
"Views/Account/ResetPassword.cshtml",

# CUSTOMER
"Views/Customer/Profile.cshtml",
"Views/Customer/Addresses.cshtml",
"Views/Customer/EditAddress.cshtml",
"Views/Customer/Wishlist.cshtml",
"Views/Customer/Notifications.cshtml",
"Views/Customer/Analytics.cshtml",

# ORDER
"Views/Order/Cart.cshtml",
"Views/Order/Checkout.cshtml",
"Views/Order/OrderHistory.cshtml",
"Views/Order/OrderDetails.cshtml",
"Views/Order/Payment.cshtml",

# RESTAURANT
"Views/Restaurant/Index.cshtml",
"Views/Restaurant/Details.cshtml",
"Views/Restaurant/Dashboard.cshtml",
"Views/Restaurant/Menu.cshtml",
"Views/Restaurant/EditMenuItem.cshtml",
"Views/Restaurant/CreateMenuItem.cshtml",

# DRIVER
"Views/Driver/Dashboard.cshtml",
"Views/Driver/MyDeliveries.cshtml",
"Views/Driver/DeliveryDetails.cshtml",
"Views/Driver/Earnings.cshtml",

# ADMIN
"Views/Admin/Dashboard.cshtml",
"Views/Admin/Users.cshtml",
"Views/Admin/Coupons.cshtml",
"Views/Admin/Reports.cshtml",
"Views/Admin/Restaurants.cshtml",
"Views/Admin/Drivers.cshtml",
"Views/Admin/Payments.cshtml",

# RATING
"Views/Rating/RateOrder.cshtml",
"Views/Rating/MyRatings.cshtml",

# CSS
"wwwroot/css/site.css",
"wwwroot/css/admin.css",
"wwwroot/css/driver.css",

# JS
"wwwroot/js/site.js",
"wwwroot/js/cart.js",
"wwwroot/js/tracking.js",
"wwwroot/js/notification.js",
"wwwroot/js/payment.js",
"wwwroot/js/admin.js"
)

foreach ($file in $MvcFiles) {
    New-Item -ItemType File -Path $file -Force | Out-Null
}

Set-Location ..

# ==========================================
# TEST PROJECT
# ==========================================

Set-Location FoodDelivery.Tests

$TestFolders = @(
"UnitTests/Services",
"UnitTests/Repositories",
"UnitTests/Helpers",
"IntegrationTests",
"Helpers"
)

foreach ($folder in $TestFolders) {
    New-Item -ItemType Directory -Path $folder -Force | Out-Null
}

$TestFiles = @(

# SERVICE TESTS
"UnitTests/Services/AuthServiceTests.cs",
"UnitTests/Services/OrderServiceTests.cs",
"UnitTests/Services/DeliveryServiceTests.cs",
"UnitTests/Services/PaymentServiceTests.cs",
"UnitTests/Services/RestaurantServiceTests.cs",
"UnitTests/Services/CouponServiceTests.cs",
"UnitTests/Services/RatingServiceTests.cs",

# REPOSITORY TESTS
"UnitTests/Repositories/UserRepositoryTests.cs",
"UnitTests/Repositories/OrderRepositoryTests.cs",
"UnitTests/Repositories/PaymentRepositoryTests.cs",
"UnitTests/Repositories/DeliveryDriverRepositoryTests.cs",
"UnitTests/Repositories/RestaurantRepositoryTests.cs",
"UnitTests/Repositories/CouponRepositoryTests.cs",

# HELPER TESTS
"UnitTests/Helpers/JwtHelperTests.cs",
"UnitTests/Helpers/DistanceCalculatorTests.cs",
"UnitTests/Helpers/PaymentNumberGeneratorTests.cs",

# INTEGRATION TESTS
"IntegrationTests/AuthFlowTests.cs",
"IntegrationTests/OrderFlowTests.cs",
"IntegrationTests/DeliveryFlowTests.cs",
"IntegrationTests/PaymentFlowTests.cs",
"IntegrationTests/RestaurantFlowTests.cs",
"IntegrationTests/CouponFlowTests.cs",

# HELPERS
"Helpers/TestDataFactory.cs",
"Helpers/MockDbContextFactory.cs"
)

foreach ($file in $TestFiles) {
    New-Item -ItemType File -Path $file -Force | Out-Null
}

Set-Location ..

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "FoodDelivery Solution Created Successfully" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Open FoodDelivery.sln from the FoodDelivery folder"
Write-Host ""

using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodDelivery.API.Helpers
{
    public static class AuthorizationHelper
    {

        public static (bool IsValid, IActionResult? ErrorResponse) ValidateCustomerAccess(
           ClaimsPrincipal user,
           int resourceOwnerId)
        {
        
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = user.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userId) ||
        !int.TryParse(userId, out var parsedUserId))
            {
                return (false, new ForbidResult());
            }

            if (userRole == "Admin")
            {
                return (true, null);
            }

            if (parsedUserId != resourceOwnerId)
            {
                return (false, new ForbidResult());
            }

            return (true, null);

        }

        
        public static int? GetCurrentUserId(ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            if (int.TryParse(userId, out var parsedId))
            {
                return parsedId;
            }

            return null;
        }

       
        public static string? GetCurrentUserRole(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)?.Value;
        }

        public static bool IsAdmin(ClaimsPrincipal user)
        {
            var role = GetCurrentUserRole(user);
            return role == "Admin";
        }

        public static bool IsCustomer(ClaimsPrincipal user)
        {
            var role = GetCurrentUserRole(user);
            return role == "Customer";
        }

        public static bool IsOwnAccount(ClaimsPrincipal user, int targetUserId)
        {
            var currentUserId = GetCurrentUserId(user);
            var userRole = GetCurrentUserRole(user);

            
            if (userRole == "Admin")
            {
                return true;
            }

            
            if (userRole == "Customer" && currentUserId != targetUserId)
            {
                return false;
            }

            return currentUserId == targetUserId;
        }

        public static string? GetCurrentUserEmail(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value;
        }
    }

}

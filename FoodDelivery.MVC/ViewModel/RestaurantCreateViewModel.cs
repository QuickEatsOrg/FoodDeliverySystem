using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.MVC.ViewModel
{
    public class RestaurantCreateViewModel
    {
        [Required]
        [Display(Name = "Restaurant Name")]
        public string RestaurantName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Address")]
        public string RestaurantAddress { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Phone")]
        public string RestaurantPhone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string RestaurantEmail { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.MVC.ViewModel
{
    public class EditMenuItemViewModel
    {
        public int ItemId { get; set; }

        [Required]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? ItemDescription { get; set; }

        [Required]
        [Range(0.01, 100000, ErrorMessage = "Price must be greater than 0.")]
        [Display(Name = "Price")]
        public decimal ItemPrice { get; set; }
    }
}

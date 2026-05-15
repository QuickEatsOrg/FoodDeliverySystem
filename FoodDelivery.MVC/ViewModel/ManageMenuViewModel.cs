namespace FoodDelivery.MVC.ViewModel
{
    public class ManageMenuViewModel
    {
        public int RestaurantId { get; set; }

        public List<EditMenuItemViewModel> MenuItems { get; set; } = new();

        public CreateMenuItemViewModel NewItem { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public string? SuccessMessage { get; set; }
    }
}

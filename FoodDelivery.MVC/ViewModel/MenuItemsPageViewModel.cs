namespace FoodDelivery.MVC.ViewModel
{
    public class MenuItemsPageViewModel
    {
        public List<MenuItemCardViewModel> Items { get; set; } = new();

        public string? SearchTerm { get; set; }

        public string? ErrorMessage { get; set; }
    }
}

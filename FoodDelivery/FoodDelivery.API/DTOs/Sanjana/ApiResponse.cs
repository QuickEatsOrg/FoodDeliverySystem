namespace FoodDelivery.API.DTOs.Sanjana
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Operation successful";
        public T? Data { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}


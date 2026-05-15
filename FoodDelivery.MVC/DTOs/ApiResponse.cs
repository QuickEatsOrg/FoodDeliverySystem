namespace FoodDelivery.MVC.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public int StatusCode { get; set; }
        public string Message { get; set; } = "Operation successful";
        public T? Data { get; set; }
        public List<string>? Error { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}

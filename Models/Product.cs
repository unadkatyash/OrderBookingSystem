namespace OrderBookingSystem.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}

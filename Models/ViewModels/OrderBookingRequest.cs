namespace OrderBookingSystem.Models.ViewModels
{
    public class OrderItemDto
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderBookingRequest
    {
        public int CustomerID { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public string PaymentMethod { get; set; } = "CreditCard";
    }

}

namespace OrderBookingSystem.Models
{
    public class PaymentMethod
    {
        public int PaymentMethodID { get; set; }
        public string MethodName { get; set; } = string.Empty;
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }

}

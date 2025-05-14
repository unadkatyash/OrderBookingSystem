namespace OrderBookingSystem.HelperClasses
{
    public class ApiResponse
    {
        public int Code { get; set; }
        public List<string> Messages { get; set; }

        public ApiResponse()
        {
            Messages = new List<string>();
        }
    }
}

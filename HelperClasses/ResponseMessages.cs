namespace OrderBookingSystem.HelperClasses
{
    public static class ResponseMessages
    {
        public const string OrderBookedSuccessfully = "Order booked successfully.";
        public const string InternalServerError = "An unexpected error occurred. Please try again later.";
        public const string OutOfStock = "Product '{0}' is out of stock.";
        public const string ProductNotFound = "Product with ID '{0}' not found.";
        public const string EmptyProductList = "Please add some products in the list.";
        public const string InvalidCustomer = "Invalid customer. Please check the customer information.";
    }
}

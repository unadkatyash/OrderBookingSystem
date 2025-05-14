using FluentValidation;
using OrderBookingSystem.HelperClasses;

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
        public int PaymentMethodID { get; set; }
        public bool AllowBooking { get; set; } = false;
    }

    public class OrderBookingRequestValidator : AbstractValidator<OrderBookingRequest>
    {
        public OrderBookingRequestValidator()
        {
            RuleFor(x => x.CustomerID)
            .GreaterThan(0)
            .WithMessage(ResponseMessages.InvalidCustomer);

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage(ResponseMessages.EmptyProductList);

            RuleFor(x => x.AllowBooking)
                .Equal(true)
                .WithMessage(ResponseMessages.AllowBooking);

            RuleFor(x => x.PaymentMethodID)
                .GreaterThan(0)
                .WithMessage(ResponseMessages.PaymentMethodRequire);
        }
    }
}

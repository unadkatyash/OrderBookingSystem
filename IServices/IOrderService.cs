using OrderBookingSystem.HelperClasses;
using OrderBookingSystem.Models.ViewModels;

namespace OrderBookingSystem.IServices
{
    public interface IOrderService
    {
        Task<ApiResponse> BookOrderAsync(OrderBookingRequest request);
    }

}

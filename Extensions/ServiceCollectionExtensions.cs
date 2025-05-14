using Microsoft.Extensions.DependencyInjection;
using OrderBookingSystem.IServices;
using OrderBookingSystem.Services;

namespace OrderBookingSystem.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
        }
    }
}

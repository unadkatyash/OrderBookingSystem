using OrderBookingSystem.Context;
using OrderBookingSystem.IServices;
using OrderBookingSystem.Models.ViewModels;
using OrderBookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using OrderBookingSystem.HelperClasses;

namespace OrderBookingSystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext _context;

        public OrderService(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse> BookOrderAsync(OrderBookingRequest request)
        {
            var response = new ApiResponse();
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var customerExists = await _context.Customers.AnyAsync(c => c.CustomerID == request.CustomerID);
                if (!customerExists)
                {
                    response.Code = StatusCodes.Status400BadRequest;
                    response.Messages.Add(ResponseMessages.InvalidCustomer);
                    return response;
                }

                if (request.Items == null || !request.Items.Any())
                {
                    response.Code = StatusCodes.Status400BadRequest;
                    response.Messages.Add(ResponseMessages.EmptyProductList);
                    return response;
                }

                var productIds = request.Items.Select(i => i.ProductID).ToList();
                var products = await _context.Products.Where(p => productIds.Contains(p.ProductID)).ToListAsync();

                if (products.Count != productIds.Count)
                {
                    var missingProductIds = productIds.Except(products.Select(p => p.ProductID)).ToList();
                    foreach (var productId in missingProductIds)
                    {
                        response.Messages.Add(string.Format(ResponseMessages.ProductNotFound, productId));
                    }

                    response.Code = StatusCodes.Status404NotFound;
                    return response;
                }

                decimal totalAmount = 0;

                foreach (var item in request.Items)
                {
                    var product = products.FirstOrDefault(p => p.ProductID == item.ProductID);

                    if (product == null)
                    {
                        response.Code = StatusCodes.Status404NotFound;
                        response.Messages.Add(string.Format(ResponseMessages.ProductNotFound, item.ProductID));
                        return response;
                    }

                    if (item.Quantity <= 0)
                    {
                        response.Code = StatusCodes.Status400BadRequest;
                        response.Messages.Add(string.Format(ResponseMessages.InvalidQuantity, product.ProductName));
                        return response;
                    }

                    if (product.Stock < item.Quantity)
                    {
                        response.Code = StatusCodes.Status400BadRequest;
                        response.Messages.Add(string.Format(ResponseMessages.OutOfStock, product.ProductName));
                        return response;
                    }

                    totalAmount += item.Quantity * product.Price;
                }

                var order = new Order
                {
                    CustomerID = request.CustomerID,
                    TotalAmount = totalAmount
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var item in request.Items)
                {
                    var product = products.First(p => p.ProductID == item.ProductID);

                    _context.OrderItems.Add(new OrderItem
                    {
                        OrderID = order.OrderID,
                        ProductID = product.ProductID,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price
                    });

                    product.Stock -= item.Quantity;
                    _context.Products.Update(product);
                }

                var paymentMethodExists = await _context.PaymentMethods.AnyAsync(pm => pm.PaymentMethodID == request.PaymentMethodID);

                if (!paymentMethodExists)
                {
                    response.Code = StatusCodes.Status400BadRequest;
                    response.Messages.Add(string.Format(ResponseMessages.InvalidPaymentMethod, request.PaymentMethodID));
                    return response;
                }

                _context.Payments.Add(new Payment
                {
                    OrderID = order.OrderID,
                    Amount = totalAmount,
                    PaymentMethodID = request.PaymentMethodID
                });


                await _context.SaveChangesAsync();
                if (!request.AllowBooking)
                {
                    throw new InvalidOperationException(string.Format(ResponseMessages.AllowBooking));
                }
                await transaction.CommitAsync();

                response.Code = StatusCodes.Status200OK;
                response.Messages.Add(ResponseMessages.OrderBookedSuccessfully);

                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.Code = StatusCodes.Status500InternalServerError;
                response.Messages.Add(ResponseMessages.InternalServerError);
                return response;
            }
        }
    }
}

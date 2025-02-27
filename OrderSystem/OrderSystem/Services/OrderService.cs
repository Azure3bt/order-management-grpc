using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using OrderModels;
using OrderSystem;
using OrderSystem.ExceptionHandling;
using OrderSystem.Persistence;

namespace OrderSystem.Services
{
    public class OrderService : OrderSystem.OrderService.OrderServiceBase
    {
        private readonly OrderDbContext _orderDbContext;
        private readonly ILogger<OrderService> _logger;

        public OrderService(OrderDbContext orderDbContext, ILogger<OrderService> logger)
        {
            _orderDbContext = orderDbContext;
            _logger = logger;
        }

        public async override Task<OrderFilterResponse> GetAllOrder(OrderFilterRequest request, ServerCallContext context)
        {
            var orderState = (OrderModels.OrderState)((int)request.State);

            OrderFilterResponse orderFilterResponse = new OrderFilterResponse();
            orderFilterResponse.Orders.AddRange(await _orderDbContext.Orders
                .Where(order => 
                    order.UserId == request.UserId ||
                    order.ProductId == request.ProductId ||
                    order.State == orderState
                )
                .Select(order => new Order
                {
                    Quantity = order.Quantity,
                    Amount = order.Amount,
                    UserId = order.UserId,
                    ProductId = order.ProductId,
                    State = (OrderState)order.State
                }).ToListAsync());
            return orderFilterResponse;
        }

        public override async Task<Order> CreateOrder(OrderRequest request, ServerCallContext context)
        {
            var findProduct = await _orderDbContext.Products.FindAsync(request.ProductId);
            ThrowException.ThrowIfNull(findProduct);
            var findUser = await _orderDbContext.Users.FindAsync(request.UserId);
            ThrowException.ThrowIfNull(findUser);

            _orderDbContext.Orders.Add(new OrderModels.Order()
            {
                ProductId = request.ProductId,
                UserId = request.UserId,
                Quantity = request.Quantity,
                Amount = request.Quantity * findProduct.Price,
                State = OrderModels.OrderState.Created
            });

            await _orderDbContext.SaveChangesAsync();
            return new Order()
            {
                ProductId = request.ProductId,
                UserId = request.UserId,
                Quantity = request.Quantity,
                Amount = request.Quantity * findProduct.Price
            };
        }

        public override async Task<Order> ModifyOrder(OrderRequest request, ServerCallContext context)
        {
            var findOrder = await _orderDbContext.Orders.FindAsync(request.OrderId);
            ThrowException.ThrowIfNull(findOrder);
            var findProduct = await _orderDbContext.Products.FindAsync(request.ProductId);
            ThrowException.ThrowIfNull(findProduct);

            findOrder.Quantity = request.Quantity;
            findOrder.Amount = request.Quantity * findProduct.Price;
            findOrder.Product = findProduct;

            await _orderDbContext.SaveChangesAsync();
            return new Order()
            {
                ProductId = request.ProductId,
                UserId = request.UserId,
                Quantity = request.Quantity,
                Amount = request.Quantity * findProduct.Price
            };
        }

        public override async Task<OrderDeletedResponse> DeleteOrder(OrderDeletedRequest request, ServerCallContext context)
        {
            var order = await _orderDbContext.Orders.FindAsync(request.OrderId);
            ThrowException.ThrowIfNull(order);
            if (order.State == OrderModels.OrderState.Cancelled)
                throw new RpcException(new Status(StatusCode.Cancelled, $"Order {request.OrderId} is cancelled, can't delete this"));

            _orderDbContext.Orders.Remove(order);
            await _orderDbContext.SaveChangesAsync();
            return new OrderDeletedResponse()
            {
                Deleted = true
            };
        }

        public override async Task<OrderCanceledResponse> CancelOrder(OrderCanceledRequest request, ServerCallContext context)
        {
            var order = await _orderDbContext.Orders.FindAsync(request.OrderId);
            ThrowException.ThrowIfNull(order);
            if (order.State == OrderModels.OrderState.Cancelled)
                return new OrderCanceledResponse()
                {
                    IsCanceled = false,
                    Message = $"Order {request.OrderId} is already cancelled"
                };

            order.State = OrderModels.OrderState.Cancelled;
            await _orderDbContext.SaveChangesAsync();
            return new OrderCanceledResponse()
            {
                IsCanceled = true,
                Message = $"Order {request.OrderId} cancelled success"
            };
        }
    }
}

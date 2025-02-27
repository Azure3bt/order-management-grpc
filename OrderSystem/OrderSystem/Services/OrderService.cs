using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using OrderSystem;
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
            var findUser = await _orderDbContext.Users.FindAsync(request.UserId);
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
            var findProduct = await _orderDbContext.Products.FindAsync(request.ProductId);
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
            if(order is null)
                throw new RpcException(Status.DefaultSuccess, $"Order {request.OrderId} not found");
            if (order.State == OrderModels.OrderState.Cancelled)
                throw new RpcException(Status.DefaultCancelled, $"Order {request.OrderId} is cancelled, can't delete this");

            _orderDbContext.Orders.Remove(order);
            await _orderDbContext.SaveChangesAsync();
            return new OrderDeletedResponse()
            {
                Deleted = true
            };
        }
    }
}

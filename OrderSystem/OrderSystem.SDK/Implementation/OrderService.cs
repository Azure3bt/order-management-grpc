using OrderSystem.SDK.Contract;
using OrderSystem.SDK.RequestModel;

namespace OrderSystem.SDK.Implementation;

internal class OrderService : IOrderService
{
    private readonly OrderSystem.OrderService.OrderServiceClient _orderServiceClient;

    public OrderService(OrderSystem.OrderService.OrderServiceClient orderServiceClient)
    {
        _orderServiceClient = orderServiceClient;
    }

    public async Task<Order> CreateOrder(CreateOrderRequest createOrderRequest)
    {

        var orderRequest = new OrderRequest
        {
            UserId = createOrderRequest.UserId,
            ProductId = createOrderRequest.ProductId,
            Quantity = createOrderRequest.Quantity
        };

        return await _orderServiceClient.CreateOrderAsync(orderRequest);
    }

    public async Task<OrderDeletedResponse> DeleteOrder(int orderId)
    {
        return await _orderServiceClient.DeleteOrderAsync(new OrderDeletedRequest { OrderId = orderId });
    }

    public async Task<OrderFilterResponse> GetAllOrder(GetOrderRequest getOrderRequest)
    {
        return await _orderServiceClient.GetAllOrderAsync(new OrderFilterRequest { UserId = getOrderRequest.UserId, ProductId = getOrderRequest.ProductId, State = (OrderState)((int)getOrderRequest.State)});
    }

    public async Task<Order> ModifyOrder(EditOrderRequest editOrderRequest)
    {
        return await _orderServiceClient.ModifyOrderAsync(new OrderRequest { UserId = editOrderRequest.UserId, ProductId = editOrderRequest.ProductId, Quantity = editOrderRequest.Quantity, OrderId = editOrderRequest.OrderId });
    }

    public async Task<OrderCanceledResponse> CancelOrder(int orderId)
    {
        return await _orderServiceClient.CancelOrderAsync(new OrderCanceledRequest { OrderId = orderId });
    }

}

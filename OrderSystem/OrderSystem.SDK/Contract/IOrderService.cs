using OrderSystem.SDK.RequestModel;

namespace OrderSystem.SDK.Contract;

public interface IOrderService
{
    Task<OrderFilterResponse> GetAllOrder(GetOrderRequest getOrderRequest, CancellationToken cancellationToken);

    Task<Order> CreateOrder(CreateOrderRequest createOrderRequest, CancellationToken cancellationToken);

    Task<Order> ModifyOrder(EditOrderRequest editOrderRequest, CancellationToken cancellationToken);
    
    Task<OrderDeletedResponse> DeleteOrder(int orderId, CancellationToken cancellationToken);

    Task<OrderCanceledResponse> CancelOrder(int orderId, CancellationToken cancellationToken);
}

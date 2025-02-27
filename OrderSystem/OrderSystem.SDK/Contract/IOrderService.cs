using Grpc.Core;
using OrderSystem.SDK.RequestModel;

namespace OrderSystem.SDK.Contract;

public interface IOrderService
{
    Task<OrderFilterResponse> GetAllOrder(GetOrderRequest getOrderRequest);

    Task<Order> CreateOrder(CreateOrderRequest createOrderRequest);

    Task<Order> ModifyOrder(EditOrderRequest editOrderRequest);
    
    Task<OrderDeletedResponse> DeleteOrder(int orderId);

    Task<OrderCanceledResponse> CancelOrder(int orderId);
}


namespace OrderSystem.SDK.RequestModel;

public abstract record OrderRequest(int UserId, int ProductId, int Quantity);

public record CreateOrderRequest(int UserId, int ProductId, int Quantity) : OrderRequest(UserId, ProductId, Quantity);
public record EditOrderRequest(int OrderId, int UserId, int ProductId, int Quantity) : OrderRequest(UserId, ProductId, Quantity);
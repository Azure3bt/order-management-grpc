namespace OrderSystem.SDK.RequestModel;

public record GetOrderRequest(int UserId, int ProductId, OrderModels.OrderState State);

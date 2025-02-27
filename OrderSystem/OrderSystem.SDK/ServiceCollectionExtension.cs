using Microsoft.Extensions.DependencyInjection;
using OrderSystem.SDK.Contract;

namespace OrderSystem.SDK;

public static class ServiceCollectionExtension
{
    public static void AddOrderServiceSdk(this IServiceCollection services)
    {
        services.AddGrpcClient<OrderService.OrderServiceClient>();
        services.AddScoped<IOrderService, Implementation.OrderService>();
    }
}

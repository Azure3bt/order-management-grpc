using Microsoft.Extensions.DependencyInjection;
using OrderSystem.SDK.Contract;
using OrderSystem.SDK.Interceptor;

namespace OrderSystem.SDK;

public static class ServiceCollectionExtension
{
    public static void AddOrderServiceSdk(this IServiceCollection services)
    {
        services.AddSingleton<ErrorHandlerInterceptor>();
        services.AddGrpcClient<OrderService.OrderServiceClient>(options =>
        {
            options.Address = new Uri("https://localhost:7190");
        })
        .AddInterceptor<ErrorHandlerInterceptor>();
        services.AddScoped<IOrderService, Implementation.OrderService>();
    }
}

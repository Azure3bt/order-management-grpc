﻿using Grpc.Core;

namespace OrderSystem.Interceptor;

public class OrderServiceInterceptor : Grpc.Core.Interceptors.Interceptor
{
    private readonly ILogger<OrderServiceInterceptor> _logger;

    public OrderServiceInterceptor(ILogger<OrderServiceInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
                TRequest request,
                ServerCallContext context,
                UnaryServerMethod<TRequest, TResponse> continuation)
    {
        LogCall<TRequest, TResponse>(MethodType.Unary, context);

        try
        {
            return await continuation(request, context);
        }
        catch (System.Exception ex)
        {
            // Note: The gRPC framework also logs exceptions thrown by handlers to .NET Core logging.
            _logger.LogError(ex, $"Error thrown by {context.Method}.");

            throw;
        }
    }

    public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        ServerCallContext context,
        ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        LogCall<TRequest, TResponse>(MethodType.ClientStreaming, context);
        return base.ClientStreamingServerHandler(requestStream, context, continuation);
    }

    public override Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        LogCall<TRequest, TResponse>(MethodType.ServerStreaming, context);
        return base.ServerStreamingServerHandler(request, responseStream, context, continuation);
    }

    public override Task DuplexStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        LogCall<TRequest, TResponse>(MethodType.DuplexStreaming, context);
        return base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);
    }

    private void LogCall<TRequest, TResponse>(MethodType methodType, ServerCallContext context)
        where TRequest : class
        where TResponse : class
    {
        _logger.LogWarning($"Starting call. Type: {methodType}. Request: {typeof(TRequest)}. Response: {typeof(TResponse)}");
        WriteMetadata(context.RequestHeaders, "caller-user");
        WriteMetadata(context.RequestHeaders, "caller-machine");
        WriteMetadata(context.RequestHeaders, "caller-os");

        void WriteMetadata(Metadata headers, string key)
        {
            var headerValue = headers.GetValue(key) ?? "(unknown)";
            _logger.LogWarning($"{key}: {headerValue}");
        }
    }
}

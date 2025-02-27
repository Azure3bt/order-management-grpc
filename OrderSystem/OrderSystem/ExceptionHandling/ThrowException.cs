using Grpc.Core;

namespace OrderSystem.ExceptionHandling;

public static class ThrowException
{
    public static void ThrowIfNull<T>(T? obj)
    {
        throw new RpcException(new Status(StatusCode.NotFound, $"{nameof(T)} {obj} not found"));
    }
}

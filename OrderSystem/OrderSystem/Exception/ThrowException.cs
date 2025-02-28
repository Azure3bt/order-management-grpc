using Grpc.Core;

namespace OrderSystem.Exception;

public static class ThrowException
{
    public static void ThrowIfNull<T>(T? obj)
    {
        throw new RpcException(new Status(StatusCode.NotFound, $"{nameof(T)} {obj} not found"));
    }
}

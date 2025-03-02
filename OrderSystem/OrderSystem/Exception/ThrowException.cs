using Google.Protobuf.WellKnownTypes;
using Google.Rpc;
using Grpc.Core;
using OrderModels.School;
using System.Runtime.CompilerServices;

namespace OrderSystem.Exception;

public static class ThrowException
{
    public static void ThrowIfNull<T>(T? obj, [CallerArgumentExpression(nameof(obj))] string? expression = null)

    {
        var status = new Google.Rpc.Status
        {
            Code = (int)Code.NotFound,
            Message = $"item from {typeof(T).Name} dosen't exist with {expression}"
        };

        throw status.ToRpcException();
    }

    public static void ThrowIfInvalidNationalId(string nationalId)
    {
        //throw new RpcException(new Status(StatusCode.InvalidArgument, $"{nationalId} is invalid"));

        var status = new Google.Rpc.Status
        {
            Code = (int)Code.InvalidArgument,
            Message = "Bad request",
            Details =
                {
                    Any.Pack(new Google.Rpc.BadRequest
                    {
                        FieldViolations =
                        {
                            new Google.Rpc.BadRequest.Types.FieldViolation { Field = nameof(Student.NationalId), Description = "Value is invalid" }
                        }
                    })
                }
        };
        throw status.ToRpcException();

    }
}

using Google.Protobuf.WellKnownTypes;
using Google.Rpc;
using Grpc.Core;
using OrderModels.School;

namespace OrderSystem.Exception;

public static class ThrowException
{
    public static void ThrowIfNull<T>(T? obj)
    {
        throw new RpcException(new Grpc.Core.Status(StatusCode.NotFound, $"{nameof(T)} {obj} not found"));
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

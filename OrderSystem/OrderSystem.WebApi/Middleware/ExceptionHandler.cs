

using Google.Rpc;
using Grpc.Core;
using System.Net;
using System.Text.Json;

namespace OrderSystem.WebApi.Middleware;

public class ExceptionHandler
{
    private readonly RequestDelegate _nextRequestOnPipeline;
    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(RequestDelegate nextRequestOnPipeline, ILogger<ExceptionHandler> logger)
    {
        _nextRequestOnPipeline = nextRequestOnPipeline;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _nextRequestOnPipeline(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (exception is RpcException rpcException)
        {
            ErrorModel errorModel = new();
            errorModel.StatusCode = rpcException.StatusCode.ToString();
            errorModel.Message = rpcException.Status.Detail;
            var badRequest = rpcException.GetRpcStatus()?.GetDetail<BadRequest>();
            if (badRequest != null)
            {
                foreach (var fieldViolation in badRequest.FieldViolations)
                {
                    errorModel.FieldViolations.Add($"{fieldViolation.Field} : {fieldViolation.Description}");
                }
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)rpcException.StatusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorModel));
            return;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync(exception.Message);

    }
}
public record ErrorModel
{
    public string StatusCode { get; set; } = default!;
    public string Message { get; set; } = default!;

    public List<string> FieldViolations { get; set; } = new();
}
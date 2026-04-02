using System.Net;
using System.Text.Json;
using Planeja_.Domain.Exceptions;

namespace Planeja_.Middleware;

public sealed class DomainExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public DomainExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var payload = new
            {
                error = ex.Message,
                status = (int)HttpStatusCode.BadRequest
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}

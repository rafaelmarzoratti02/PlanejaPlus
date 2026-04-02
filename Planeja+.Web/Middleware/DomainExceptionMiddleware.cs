using System.Net;
using System.Text.Json;
using Planeja_.Application.Exceptions;
using Planeja_.Domain.Exceptions;

namespace Planeja_.Web.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            await WriteResponseAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (NotFoundException ex)
        {
            await WriteResponseAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            await WriteResponseAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }

    private static async Task WriteResponseAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var payload = new { error = message, status = (int)statusCode };
        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}

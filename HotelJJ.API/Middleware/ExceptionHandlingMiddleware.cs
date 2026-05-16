using System.Net;
using System.Text.Json;
using HotelJJ.API.Models.Common;
using HotelJJ.Business.Exceptions;
using HotelJJ.DataAccess.Http.Common;

namespace HotelJJ.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, code, message) = exception switch
        {
            IntegrationValidationException ex => (HttpStatusCode.BadRequest, ex.Code, ex.Message),
            IntegrationUnauthorizedException ex => (HttpStatusCode.Unauthorized, ex.Code, ex.Message),
            IntegrationNotFoundException ex => (HttpStatusCode.NotFound, ex.Code, ex.Message),
            IntegrationConflictException ex => (HttpStatusCode.Conflict, ex.Code, ex.Message),
            IntegrationBusinessException ex => (HttpStatusCode.BadGateway, ex.Code, ex.Message),
            _ => (HttpStatusCode.InternalServerError, "MID-ERR-001", "Ocurrio un error inesperado en el middleware.")
        };

        DownstreamApiException? downstream = ExtractDownstreamException(exception);

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled middleware error.");
        }
        else
        {
            _logger.LogWarning(exception, "Handled middleware error {Code}.", code);
        }

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var payload = new ApiErrorResponse
        {
            Code = code,
            Message = message,
            Service = downstream?.ServiceName,
            DownstreamStatusCode = downstream is null ? null : (int)downstream.StatusCode,
            Path = downstream?.Path,
            TraceId = context.TraceIdentifier
        };

        if (_environment.IsDevelopment() && downstream is not null && !string.IsNullOrWhiteSpace(downstream.ResponseBody))
        {
            _logger.LogInformation(
                "Downstream error detail {Service} {StatusCode} {Path} Body: {Body}",
                downstream.ServiceName,
                (int)downstream.StatusCode,
                downstream.Path,
                downstream.ResponseBody);
        }

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload, JsonSerializerOptions.Web));
    }

    private static DownstreamApiException? ExtractDownstreamException(Exception exception)
    {
        if (exception is DownstreamApiException directDownstream)
        {
            return directDownstream;
        }

        return exception.InnerException as DownstreamApiException;
    }
}

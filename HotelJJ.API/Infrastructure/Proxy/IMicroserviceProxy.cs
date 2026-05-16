namespace HotelJJ.API.Infrastructure.Proxy;

public interface IMicroserviceProxy
{
    Task ProxyAsync(
        string microserviceName,
        HttpContext httpContext,
        string? overridePathAndQuery = null,
        CancellationToken cancellationToken = default);
}

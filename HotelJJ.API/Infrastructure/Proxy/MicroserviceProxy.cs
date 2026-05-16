using System.Net.Http.Headers;
using HotelJJ.Business.Exceptions;

namespace HotelJJ.API.Infrastructure.Proxy;

public class MicroserviceProxy : IMicroserviceProxy
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ProxyRouteResolver _routeResolver;
    private readonly ProxyResponseWriter _responseWriter;

    public MicroserviceProxy(
        IHttpClientFactory httpClientFactory,
        ProxyRouteResolver routeResolver,
        ProxyResponseWriter responseWriter)
    {
        _httpClientFactory = httpClientFactory;
        _routeResolver = routeResolver;
        _responseWriter = responseWriter;
    }

    public async Task ProxyAsync(
        string microserviceName,
        HttpContext httpContext,
        string? overridePathAndQuery = null,
        CancellationToken cancellationToken = default)
    {
        var baseUri = _routeResolver.ResolveBaseUri(microserviceName);
        var destinationPathAndQuery = overridePathAndQuery ?? BuildPathAndQueryFromRequest(httpContext.Request);
        var destinationUri = new Uri(baseUri, destinationPathAndQuery);

        using var outboundRequest = new HttpRequestMessage(new HttpMethod(httpContext.Request.Method), destinationUri);
        CopyHeaders(httpContext.Request, outboundRequest);

        if (HasBody(httpContext.Request.Method))
        {
            outboundRequest.Content = new StreamContent(httpContext.Request.Body);
            if (!string.IsNullOrWhiteSpace(httpContext.Request.ContentType))
            {
                outboundRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(httpContext.Request.ContentType);
            }
        }

        using var client = _httpClientFactory.CreateClient();

        HttpResponseMessage downstreamResponse;
        try
        {
            downstreamResponse = await client.SendAsync(
                outboundRequest,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new IntegrationBusinessException(
                "MID-PROXY-503",
                $"No fue posible contactar al microservicio {microserviceName}.",
                ex);
        }
        catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            throw new IntegrationBusinessException(
                "MID-PROXY-504",
                $"Tiempo de espera agotado al llamar al microservicio {microserviceName}.",
                ex);
        }

        using (downstreamResponse)
        {
            await _responseWriter.WriteAsync(httpContext, downstreamResponse, cancellationToken);
        }
    }

    private static string BuildPathAndQueryFromRequest(HttpRequest request)
    {
        return $"{request.Path}{request.QueryString}";
    }

    private static bool HasBody(string method)
    {
        return method.Equals(HttpMethods.Post, StringComparison.OrdinalIgnoreCase)
            || method.Equals(HttpMethods.Put, StringComparison.OrdinalIgnoreCase)
            || method.Equals(HttpMethods.Patch, StringComparison.OrdinalIgnoreCase);
    }

    private static void CopyHeaders(HttpRequest source, HttpRequestMessage destination)
    {
        foreach (var header in source.Headers)
        {
            if (header.Key.Equals("Host", StringComparison.OrdinalIgnoreCase) ||
                header.Key.Equals("Content-Length", StringComparison.OrdinalIgnoreCase) ||
                header.Key.Equals("Connection", StringComparison.OrdinalIgnoreCase) ||
                header.Key.Equals("Transfer-Encoding", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (!destination.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()) && destination.Content is not null)
            {
                destination.Content.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }
    }
}

namespace HotelJJ.API.Infrastructure.Proxy;

public class ProxyResponseWriter
{
    public async Task WriteAsync(HttpContext context, HttpResponseMessage response, CancellationToken cancellationToken)
    {
        context.Response.StatusCode = (int)response.StatusCode;

        foreach (var header in response.Headers)
        {
            if (ShouldSkipHeader(header.Key))
            {
                continue;
            }

            context.Response.Headers[header.Key] = header.Value.ToArray();
        }

        if (response.Content is null)
        {
            return;
        }

        foreach (var header in response.Content.Headers)
        {
            if (ShouldSkipHeader(header.Key))
            {
                continue;
            }

            context.Response.Headers[header.Key] = header.Value.ToArray();
        }

        await response.Content.CopyToAsync(context.Response.Body, cancellationToken);
    }

    private static bool ShouldSkipHeader(string headerName)
    {
        return headerName.Equals("transfer-encoding", StringComparison.OrdinalIgnoreCase);
    }
}

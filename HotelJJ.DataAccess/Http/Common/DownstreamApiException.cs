using System.Net;

namespace HotelJJ.DataAccess.Http.Common;

public class DownstreamApiException : Exception
{
    public DownstreamApiException(
        string serviceName,
        HttpStatusCode statusCode,
        string message,
        string? responseBody = null,
        string? path = null)
        : base(message)
    {
        ServiceName = serviceName;
        StatusCode = statusCode;
        ResponseBody = responseBody;
        Path = path;
    }

    public string ServiceName { get; }
    public HttpStatusCode StatusCode { get; }
    public string? ResponseBody { get; }
    public string? Path { get; }
}

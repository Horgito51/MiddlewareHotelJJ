namespace HotelJJ.API.Models.Common;

public class ApiErrorResponse
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Service { get; set; }
    public int? DownstreamStatusCode { get; set; }
    public string? Path { get; set; }
    public string? TraceId { get; set; }
}

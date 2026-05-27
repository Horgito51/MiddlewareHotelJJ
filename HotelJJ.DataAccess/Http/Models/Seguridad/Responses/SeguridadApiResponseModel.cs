namespace HotelJJ.DataAccess.Http.Models.Seguridad.Responses;

public class SeguridadApiResponseModel<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T Data { get; set; } = default!;
    public object? Errors { get; set; }
}

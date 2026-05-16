namespace HotelJJ.DataAccess.Http.Models.Seguridad.Requests;

public class LogoutSeguridadRequestModel
{
    public string RefreshToken { get; set; } = string.Empty;
}

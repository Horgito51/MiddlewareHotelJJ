namespace HotelJJ.API.Models.Requests.Auth;

public class CambiarPasswordIntegrationRequest
{
    public string PasswordActual { get; set; } = string.Empty;
    public string PasswordNuevo { get; set; } = string.Empty;
}

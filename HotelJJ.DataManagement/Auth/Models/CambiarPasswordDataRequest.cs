namespace HotelJJ.DataManagement.Auth.Models;

public class CambiarPasswordDataRequest
{
    public string PasswordActual { get; set; } = string.Empty;
    public string PasswordNuevo { get; set; } = string.Empty;
}

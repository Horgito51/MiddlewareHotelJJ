namespace HotelJJ.DataAccess.Http.Models.Seguridad.Requests;

public class CambiarPasswordSeguridadRequestModel
{
    public string PasswordActual { get; set; } = string.Empty;
    public string PasswordNuevo { get; set; } = string.Empty;
}

namespace HotelJJ.DataAccess.Http.Models.Alojamiento.Responses;

public class AlojamientoReviewResponseModel
{
    public Guid ValoracionGuid { get; set; }
    public decimal Puntuacion { get; set; }
    public string? ComentarioPositivo { get; set; }
    public string? ComentarioNegativo { get; set; }
    public string? TipoViaje { get; set; }
    public DateTime Fecha { get; set; }
    public string NombreVisibleCliente { get; set; } = string.Empty;
    public string? RespuestaPropiedad { get; set; }
}

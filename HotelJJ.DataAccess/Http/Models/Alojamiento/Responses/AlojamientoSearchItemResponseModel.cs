namespace HotelJJ.DataAccess.Http.Models.Alojamiento.Responses;

public class AlojamientoSearchItemResponseModel
{
    public Guid SucursalGuid { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Ciudad { get; set; }
    public string? Provincia { get; set; }
    public string? Pais { get; set; }
    public string? Direccion { get; set; }
    public string? Descripcion { get; set; }
    public string? Categoria { get; set; }
    public int? Estrellas { get; set; }
    public string? TipoAlojamiento { get; set; }
    public decimal? PrecioDesde { get; set; }
    public string Moneda { get; set; } = "USD";
    public string? ImagenPrincipalUrl { get; set; }
    public decimal? PromedioValoracion { get; set; }
    public int TotalValoraciones { get; set; }
    public int HabitacionesDisponibles { get; set; }
    public List<string> ServiciosDestacados { get; set; } = new();
    public string? HoraCheckIn { get; set; }
    public string? HoraCheckOut { get; set; }
    public bool AceptaNinos { get; set; }
    public bool PermiteMascotas { get; set; }
}

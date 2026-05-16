namespace HotelJJ.API.Models.Requests.Alojamiento;

public class AlojamientoSearchRequest
{
    public string? Destino { get; set; }
    public DateTime? FechaEntrada { get; set; }
    public DateTime? FechaSalida { get; set; }
    public int? NumAdultos { get; set; }
    public int? NumNinos { get; set; }
    public int? NumHabitaciones { get; set; }
    public string? TipoAlojamiento { get; set; }
    public decimal? PrecioMin { get; set; }
    public decimal? PrecioMax { get; set; }
    public string? CategoriaViaje { get; set; }
    public string? OrdenarPor { get; set; }
    public int Pagina { get; set; } = 1;
    public int Limite { get; set; } = 20;
}

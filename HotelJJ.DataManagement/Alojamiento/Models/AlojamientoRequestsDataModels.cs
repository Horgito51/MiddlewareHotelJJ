namespace HotelJJ.DataManagement.Alojamiento.Models;

public class AlojamientoSearchDataRequest
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

public class AlojamientoDetailDataRequest
{
    public DateTime? FechaEntrada { get; set; }
    public DateTime? FechaSalida { get; set; }
}

public class AlojamientoReviewsDataRequest
{
    public int Pagina { get; set; } = 1;
    public int Limite { get; set; } = 10;
}

public class AlojamientoHabitacionesDataRequest
{
    public Guid? TipoHabitacionGuid { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
}

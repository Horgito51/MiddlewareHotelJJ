namespace HotelJJ.DataAccess.Http.Models.Alojamiento.Responses;

public class AlojamientoDetailResponseModel : AlojamientoSearchItemResponseModel
{
    public string? DescripcionCompleta { get; set; }
    public List<AlojamientoRoomTypeResponseModel> TiposHabitacion { get; set; } = new();
    public List<AlojamientoTariffResponseModel> TarifasActivas { get; set; } = new();
    public List<string> Amenities { get; set; } = new();
    public List<string> Imagenes { get; set; } = new();
    public AlojamientoPolicyResponseModel Politicas { get; set; } = new();
    public AlojamientoAvailabilityResponseModel? Disponibilidad { get; set; }
}

public class AlojamientoRoomTypeResponseModel
{
    public Guid TipoHabitacionGuid { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? TipoCama { get; set; }
    public int CapacidadAdultos { get; set; }
    public int CapacidadNinos { get; set; }
    public decimal? AreaM2 { get; set; }
    public decimal PrecioBase { get; set; }
    public List<string> Imagenes { get; set; } = new();
    public int? DisponiblesEnRango { get; set; }
}

public class AlojamientoTariffResponseModel
{
    public Guid TarifaGuid { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal PrecioPorNoche { get; set; }
    public string Moneda { get; set; } = "USD";
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int? MinNoches { get; set; }
    public Guid? TipoHabitacionGuid { get; set; }
}

public class AlojamientoPolicyResponseModel
{
    public string? HoraCheckIn { get; set; }
    public string? HoraCheckOut { get; set; }
    public bool AceptaNinos { get; set; }
    public bool PermiteMascotas { get; set; }
    public string? Politicas { get; set; }
}

public class AlojamientoAvailabilityResponseModel
{
    public DateTime FechaEntrada { get; set; }
    public DateTime FechaSalida { get; set; }
    public List<AlojamientoAvailabilityByRoomTypeResponseModel> PorTipoHabitacion { get; set; } = new();
}

public class AlojamientoAvailabilityByRoomTypeResponseModel
{
    public Guid TipoHabitacionGuid { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Disponibles { get; set; }
}

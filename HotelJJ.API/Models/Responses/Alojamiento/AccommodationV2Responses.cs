namespace HotelJJ.API.Models.Responses.Alojamiento;

public sealed class AccommodationRoomTypeV2Response
{
    public Guid TipoHabitacionGuid { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? TipoCama { get; set; }
    public int CapacidadAdultos { get; set; }
    public int CapacidadNinos { get; set; }
    public decimal? AreaM2 { get; set; }
    public decimal PrecioBase { get; set; }
    public decimal PrecioNocheAplicado { get; set; }
    public Guid? TarifaGuid { get; set; }
    public string? OrigenPrecio { get; set; }
    public int? DisponiblesEnRango { get; set; }
    public IReadOnlyList<string> Imagenes { get; set; } = Array.Empty<string>();
}

public sealed class AccommodationAvailabilityV2Response
{
    public Guid SucursalGuid { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int TotalDisponibles { get; set; }
    public IReadOnlyList<AccommodationAvailabilityByRoomTypeV2Response> PorTipoHabitacion { get; set; } =
        Array.Empty<AccommodationAvailabilityByRoomTypeV2Response>();
}

public sealed class AccommodationAvailabilityByRoomTypeV2Response
{
    public Guid TipoHabitacionGuid { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Disponibles { get; set; }
    public decimal PrecioNocheAplicado { get; set; }
    public Guid? TarifaGuid { get; set; }
    public string? OrigenPrecio { get; set; }
}


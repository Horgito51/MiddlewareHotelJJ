namespace HotelJJ.Business.DTOs.Alojamiento;

public class AlojamientoSearchItemDTO
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

public class AlojamientoDetailDTO : AlojamientoSearchItemDTO
{
    public string? DescripcionCompleta { get; set; }
    public List<AlojamientoRoomTypeDTO> TiposHabitacion { get; set; } = new();
    public List<AlojamientoTariffDTO> TarifasActivas { get; set; } = new();
    public List<string> Amenities { get; set; } = new();
    public List<string> Imagenes { get; set; } = new();
    public AlojamientoPolicyDTO Politicas { get; set; } = new();
}

public class AlojamientoRoomTypeDTO
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
    public List<string> Imagenes { get; set; } = new();
    public int? DisponiblesEnRango { get; set; }
}

public class AlojamientoTariffDTO
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

public class AlojamientoPolicyDTO
{
    public string? HoraCheckIn { get; set; }
    public string? HoraCheckOut { get; set; }
    public bool AceptaNinos { get; set; }
    public bool PermiteMascotas { get; set; }
    public string? Politicas { get; set; }
}

public class AlojamientoAvailabilityDTO
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public List<AlojamientoAvailabilityByRoomTypeDTO> PorTipoHabitacion { get; set; } = new();
}

public class AlojamientoAvailabilityByRoomTypeDTO
{
    public Guid TipoHabitacionGuid { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Disponibles { get; set; }
}

public class AlojamientoReviewDTO
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

public class AlojamientoHabitacionDTO
{
    public Guid HabitacionGuid { get; set; }
    public Guid TipoHabitacionGuid { get; set; }
    public string TipoNombre { get; set; } = string.Empty;
    public string NumeroHabitacion { get; set; } = string.Empty;
    public int? Piso { get; set; }
    public int CapacidadAdultos { get; set; }
    public int CapacidadNinos { get; set; }
    public decimal PrecioBase { get; set; }
    public string Moneda { get; set; } = "USD";
    public string EstadoHabitacion { get; set; } = string.Empty;
    public bool? DisponibleEnRango { get; set; }
}

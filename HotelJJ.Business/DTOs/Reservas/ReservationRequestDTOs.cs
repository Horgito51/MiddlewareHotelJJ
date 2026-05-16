namespace HotelJJ.Business.DTOs.Reservas;

public class ReservationCreateDTO
{
    public Guid ClienteGuid { get; set; }
    public Guid SucursalGuid { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal DescuentoAplicado { get; set; }
    public string? Observaciones { get; set; }
    public bool EsWalkin { get; set; }
    public string? OrigenCanalReserva { get; set; }
    public ReservationClientDTO? Cliente { get; set; }
    public IReadOnlyList<ReservationRoomCreateDTO> Habitaciones { get; set; } = Array.Empty<ReservationRoomCreateDTO>();
}

public class ReservationClientDTO
{
    public string TipoIdentificacion { get; set; } = string.Empty;
    public string NumeroIdentificacion { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string? Apellidos { get; set; }
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string? Direccion { get; set; }
}

public class ReservationRoomCreateDTO
{
    public Guid HabitacionGuid { get; set; }
    public Guid TipoHabitacionGuid { get; set; }
    public int NumHabitaciones { get; set; } = 1;
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int NumAdultos { get; set; } = 1;
    public int NumNinos { get; set; }
    public decimal DescuentoLinea { get; set; }
}

public class ReservationPriceRequestDTO
{
    public Guid HabitacionGuid { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string? Canal { get; set; }
}

public class CancelReservationDTO
{
    public string? Motivo { get; set; }
}

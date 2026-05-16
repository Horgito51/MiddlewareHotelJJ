namespace HotelJJ.API.Models.Requests.Reservas;

public class CreateReservationRequest
{
    public Guid ClienteGuid { get; set; }
    public Guid SucursalGuid { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal DescuentoAplicado { get; set; }
    public string? Observaciones { get; set; }
    public bool EsWalkin { get; set; }
    public string? OrigenCanalReserva { get; set; }
    public ReservationClientRequest? Cliente { get; set; }
    public List<ReservationRoomRequest> Habitaciones { get; set; } = new();
}

public class ReservationClientRequest
{
    public string TipoIdentificacion { get; set; } = string.Empty;
    public string NumeroIdentificacion { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string? Apellidos { get; set; }
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string? Direccion { get; set; }
}

public class ReservationRoomRequest
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

public class ReservationPriceRequest
{
    public Guid HabitacionGuid { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string? Canal { get; set; }
}

public class CancelReservationRequest
{
    public string? Motivo { get; set; }
}

public class PublicCancelarReservaRequest
{
    public string? Motivo { get; set; }
}

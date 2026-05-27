namespace HotelJJ.API.Models.Requests.Reservas;

public class CreateReservationRequest
{
    public Guid SucursalGuid { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string? OrigenCanalReserva { get; set; }
    public string? Observaciones { get; set; }
    public bool EsWalkin { get; set; }
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
    public Guid TipoHabitacionGuid { get; set; }
    public int NumHabitaciones { get; set; } = 1;
    public int NumAdultos { get; set; } = 1;
    public int NumNinos { get; set; }
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

public class ClienteCreateRequest
{
    public string TipoIdentificacion { get; set; } = string.Empty;
    public string NumeroIdentificacion { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string? RazonSocial { get; set; }
    public string Correo { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public string Estado { get; set; } = "ACT";
}

public class ClienteUpdateRequest
{
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string? RazonSocial { get; set; }
    public string Correo { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public string Estado { get; set; } = "ACT";
}

public class InternalReservaCreateRequest
{
    public Guid ClienteGuid { get; set; }
    public ClienteCreateRequest? Cliente { get; set; }
    public Guid SucursalGuid { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal DescuentoAplicado { get; set; }
    public string? Observaciones { get; set; }
    public bool EsWalkin { get; set; }
    public string? OrigenCanalReserva { get; set; } = "INTERNO";
    public List<InternalReservaHabitacionTipoRequest> Habitaciones { get; set; } = new();
}

public class InternalReservaHabitacionTipoRequest
{
    public Guid TipoHabitacionGuid { get; set; }
    public int NumHabitaciones { get; set; } = 1;
    public int NumAdultos { get; set; } = 1;
    public int NumNinos { get; set; }
    public decimal DescuentoLinea { get; set; }
}

public class ReservaUpdateRequest
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal SubtotalReserva { get; set; }
    public decimal ValorIva { get; set; }
    public decimal TotalReserva { get; set; }
    public decimal DescuentoAplicado { get; set; }
    public decimal SaldoPendiente { get; set; }
    public string EstadoReserva { get; set; } = "PEN";
    public string? Observaciones { get; set; }
}

namespace HotelJJ.DataAccess.Http.Models.Reservas.Requests;

public class CreateReservaRequestModel
{
    public Guid SucursalGuid { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string? OrigenCanalReserva { get; set; }
    public string? Observaciones { get; set; }
    public bool EsWalkin { get; set; }
    public ReservaClienteRequestModel? Cliente { get; set; }
    public List<ReservaHabitacionRequestModel> Habitaciones { get; set; } = new();
}

public class ReservaClienteRequestModel
{
    public string TipoIdentificacion { get; set; } = string.Empty;
    public string NumeroIdentificacion { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string? Apellidos { get; set; }
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string? Direccion { get; set; }
}

public class ReservaHabitacionRequestModel
{
    public Guid TipoHabitacionGuid { get; set; }
    public int NumHabitaciones { get; set; } = 1;
    public int NumAdultos { get; set; } = 1;
    public int NumNinos { get; set; }
}

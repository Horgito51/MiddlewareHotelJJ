namespace HotelJJ.DataAccess.Http.Models.Reservas.Responses;

public class InternalReservaResponseModel
{
    public int IdReserva { get; set; }
    public Guid GuidReserva { get; set; }
    public string CodigoReserva { get; set; } = string.Empty;
    public int IdCliente { get; set; }
    public int IdSucursal { get; set; }
    public Guid? SucursalGuid { get; set; }
    public DateTime FechaReservaUtc { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal SubtotalReserva { get; set; }
    public decimal ValorIva { get; set; }
    public decimal TotalReserva { get; set; }
    public decimal DescuentoAplicado { get; set; }
    public decimal SaldoPendiente { get; set; }
    public string OrigenCanalReserva { get; set; } = string.Empty;
    public string EstadoReserva { get; set; } = string.Empty;
    public DateTime? FechaConfirmacionUtc { get; set; }
    public DateTime? FechaCancelacionUtc { get; set; }
    public string? MotivoCancelacion { get; set; }
    public string? Observaciones { get; set; }
    public bool EsWalkin { get; set; }
    public List<InternalReservaHabitacionResponseModel> Habitaciones { get; set; } = new();
}

public class InternalReservaHabitacionResponseModel
{
    public int IdReservaHabitacion { get; set; }
    public Guid ReservaHabitacionGuid { get; set; }
    public int IdReserva { get; set; }
    public int IdHabitacion { get; set; }
    public Guid? HabitacionGuid { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int NumAdultos { get; set; }
    public int NumNinos { get; set; }
    public decimal PrecioNocheAplicado { get; set; }
    public decimal SubtotalLinea { get; set; }
    public decimal ValorIvaLinea { get; set; }
    public decimal DescuentoLinea { get; set; }
    public decimal TotalLinea { get; set; }
    public string EstadoDetalle { get; set; } = string.Empty;
}

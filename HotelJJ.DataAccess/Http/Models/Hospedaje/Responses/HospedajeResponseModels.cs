namespace HotelJJ.DataAccess.Http.Models.Hospedaje.Responses;

public class EstadiaHospedajeResponseModel
{
    public int IdEstadia { get; set; }
    public Guid EstadiaGuid { get; set; }
    public int IdReservaHabitacion { get; set; }
    public int IdCliente { get; set; }
    public int IdHabitacion { get; set; }
    public DateTime? CheckinUtc { get; set; }
    public DateTime? CheckoutUtc { get; set; }
    public string EstadoEstadia { get; set; } = string.Empty;
    public string? ObservacionesCheckin { get; set; }
    public string? ObservacionesCheckout { get; set; }
    public bool RequiereMantenimiento { get; set; }
    public DateTime FechaRegistroUtc { get; set; }
    public string? CreadoPorUsuario { get; set; }
    public string? ModificadoPorUsuario { get; set; }
    public DateTime? FechaModificacionUtc { get; set; }
    public string? ServicioOrigen { get; set; }
    public List<CargoHospedajeResponseModel> Cargos { get; set; } = new();
}

public class CargoHospedajeResponseModel
{
    public int IdCargoEstadia { get; set; }
    public Guid CargoGuid { get; set; }
    public int IdEstadia { get; set; }
    public int? IdCatalogo { get; set; }
    public string DescripcionCargo { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ValorIva { get; set; }
    public decimal TotalCargo { get; set; }
    public DateTime FechaConsumoUtc { get; set; }
    public string EstadoCargo { get; set; } = string.Empty;
    public DateTime FechaRegistroUtc { get; set; }
    public string? CreadoPorUsuario { get; set; }
    public string? ModificadoPorUsuario { get; set; }
    public DateTime? FechaModificacionUtc { get; set; }
    public string? ServicioOrigen { get; set; }
}

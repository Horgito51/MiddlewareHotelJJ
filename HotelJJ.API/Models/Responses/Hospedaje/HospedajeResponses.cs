namespace HotelJJ.API.Models.Responses.Hospedaje;

public class EstadiaResponse
{
    public Guid EstadiaGuid { get; set; }
    public DateTime? CheckinUtc { get; set; }
    public DateTime? CheckoutUtc { get; set; }
    public string EstadoEstadia { get; set; } = string.Empty;
    public string? ObservacionesCheckin { get; set; }
    public string? ObservacionesCheckout { get; set; }
    public bool RequiereMantenimiento { get; set; }
    public DateTime FechaRegistroUtc { get; set; }
    public IReadOnlyList<CargoEstadiaResponse> Cargos { get; set; } = Array.Empty<CargoEstadiaResponse>();
}

public class CargoEstadiaResponse
{
    public Guid CargoGuid { get; set; }
    public string DescripcionCargo { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ValorIva { get; set; }
    public decimal TotalCargo { get; set; }
    public DateTime FechaConsumoUtc { get; set; }
    public string EstadoCargo { get; set; } = string.Empty;
    public DateTime FechaRegistroUtc { get; set; }
}

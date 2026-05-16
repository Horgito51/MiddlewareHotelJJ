namespace HotelJJ.Business.DTOs.Hospedaje;

public class EstadiaDTO
{
    public Guid EstadiaGuid { get; set; }
    public DateTime? CheckinUtc { get; set; }
    public DateTime? CheckoutUtc { get; set; }
    public string EstadoEstadia { get; set; } = string.Empty;
    public string? ObservacionesCheckin { get; set; }
    public string? ObservacionesCheckout { get; set; }
    public bool RequiereMantenimiento { get; set; }
    public DateTime FechaRegistroUtc { get; set; }
    public IReadOnlyList<CargoEstadiaDTO> Cargos { get; set; } = Array.Empty<CargoEstadiaDTO>();
}

public class CargoEstadiaDTO
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

public class CheckOutHospedajeDTO
{
    public string? Observaciones { get; set; }
    public bool RequiereMantenimiento { get; set; }
}

public class CargoHospedajeCreateDTO
{
    public int? IdCatalogo { get; set; }
    public string DescripcionCargo { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal ValorIva { get; set; }
}

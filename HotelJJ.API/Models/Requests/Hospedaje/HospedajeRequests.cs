namespace HotelJJ.API.Models.Requests.Hospedaje;

public class CheckOutHospedajeRequest
{
    public string? Observaciones { get; set; }
    public bool RequiereMantenimiento { get; set; }
}

public class CargoHospedajeCreateRequest
{
    public int? IdCatalogo { get; set; }
    public string DescripcionCargo { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal ValorIva { get; set; }
}

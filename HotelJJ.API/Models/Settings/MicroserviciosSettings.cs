namespace HotelJJ.API.Models.Settings;

public class MicroserviciosSettings
{
    public MicroservicioEndpointSettings Seguridad { get; set; } = new();
    public MicroservicioEndpointSettings Alojamiento { get; set; } = new();
    public MicroservicioEndpointSettings Reservas { get; set; } = new();
    public MicroservicioEndpointSettings Hospedaje { get; set; } = new();
    public MicroservicioEndpointSettings Facturacion { get; set; } = new();
}

public class MicroservicioEndpointSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public string? GrpcUrl { get; set; }
    public int TimeoutSeconds { get; set; } = 15;
}

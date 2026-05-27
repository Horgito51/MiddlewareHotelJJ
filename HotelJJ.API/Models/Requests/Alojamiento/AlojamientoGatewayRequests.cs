namespace HotelJJ.API.Models.Requests.Alojamiento;

public sealed class ImagenRequest
{
    public Guid ImagenGuid { get; set; }
    public string UrlImagen { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int Orden { get; set; }
    public bool EsPrincipal { get; set; }
}

public sealed class SucursalUpsertRequest
{
    public string CodigoSucursal { get; set; } = string.Empty;
    public string NombreSucursal { get; set; } = string.Empty;
    public string? DescripcionSucursal { get; set; }
    public string? DescripcionCorta { get; set; }
    public string? TipoAlojamiento { get; set; }
    public int? Estrellas { get; set; }
    public string? CategoriaViaje { get; set; }
    public string? Pais { get; set; }
    public string? Provincia { get; set; }
    public string? Ciudad { get; set; }
    public string? Ubicacion { get; set; }
    public string? Direccion { get; set; }
    public string? CodigoPostal { get; set; }
    public string? Telefono { get; set; }
    public string? Correo { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public string? HoraCheckin { get; set; }
    public string? HoraCheckout { get; set; }
    public bool CheckinAnticipado { get; set; }
    public bool CheckoutTardio { get; set; }
    public bool AceptaNinos { get; set; }
    public int? EdadMinimaHuesped { get; set; }
    public bool PermiteMascotas { get; set; }
    public bool SePermiteFumar { get; set; }
    public string EstadoSucursal { get; set; } = "ACT";
    public List<ImagenRequest>? Imagenes { get; set; }
}

public sealed class SucursalPoliticasPatchRequest
{
    public string? HoraCheckin { get; set; }
    public string? HoraCheckout { get; set; }
    public bool PermiteMascotas { get; set; }
    public bool SePermiteFumar { get; set; }
    public bool AceptaNinos { get; set; }
    public bool CheckinAnticipado { get; set; }
    public bool CheckoutTardio { get; set; }
}

public sealed class InhabilitarRequest
{
    public string Motivo { get; set; } = string.Empty;
}

public sealed class TipoHabitacionUpsertRequest
{
    public string CodigoTipoHabitacion { get; set; } = string.Empty;
    public string NombreTipoHabitacion { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int CapacidadAdultos { get; set; }
    public int CapacidadNinos { get; set; }
    public int CapacidadTotal { get; set; }
    public string TipoCama { get; set; } = string.Empty;
    public decimal? AreaM2 { get; set; }
    public bool PermiteEventos { get; set; }
    public bool PermiteReservaPublica { get; set; }
    public string EstadoTipoHabitacion { get; set; } = "ACT";
    public List<ImagenRequest>? Imagenes { get; set; }
}

public class HabitacionCreateRequest
{
    public int IdSucursal { get; set; }
    public int IdTipoHabitacion { get; set; }
    public string NumeroHabitacion { get; set; } = string.Empty;
    public int? Piso { get; set; }
    public int CapacidadHabitacion { get; set; }
    public decimal PrecioBase { get; set; }
    public string? DescripcionHabitacion { get; set; }
    public string EstadoHabitacion { get; set; } = "DIS";
}

public sealed class HabitacionUpdateRequest : HabitacionCreateRequest
{
}

public sealed class HabitacionEstadoRequest
{
    public string NuevoEstado { get; set; } = string.Empty;
}

public sealed class TarifaUpsertRequest
{
    public string CodigoTarifa { get; set; } = string.Empty;
    public int IdSucursal { get; set; }
    public int IdTipoHabitacion { get; set; }
    public string NombreTarifa { get; set; } = string.Empty;
    public string CanalTarifa { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal PrecioPorNoche { get; set; }
    public decimal PorcentajeIva { get; set; }
    public int MinNoches { get; set; }
    public int? MaxNoches { get; set; }
    public bool PermitePortalPublico { get; set; }
    public int Prioridad { get; set; }
    public string EstadoTarifa { get; set; } = "ACT";
}

public sealed class CatalogoServicioUpsertRequest
{
    public int? IdSucursal { get; set; }
    public string CodigoCatalogo { get; set; } = string.Empty;
    public string NombreCatalogo { get; set; } = string.Empty;
    public string TipoCatalogo { get; set; } = string.Empty;
    public string? CategoriaCatalogo { get; set; }
    public string? DescripcionCatalogo { get; set; }
    public decimal PrecioBase { get; set; }
    public bool AplicaIva { get; set; }
    public bool Disponible24h { get; set; }
    public TimeSpan? HoraInicio { get; set; }
    public TimeSpan? HoraFin { get; set; }
    public string? IconoUrl { get; set; }
    public string EstadoCatalogo { get; set; } = "ACT";
}

public sealed class ValoracionCreateRequest
{
    public int IdEstadia { get; set; }
    public int IdCliente { get; set; }
    public int IdSucursal { get; set; }
    public int? IdHabitacion { get; set; }
    public decimal PuntuacionGeneral { get; set; }
    public decimal? PuntuacionLimpieza { get; set; }
    public decimal? PuntuacionConfort { get; set; }
    public decimal? PuntuacionUbicacion { get; set; }
    public decimal? PuntuacionInstalaciones { get; set; }
    public decimal? PuntuacionPersonal { get; set; }
    public decimal? PuntuacionCalidadPrecio { get; set; }
    public string? ComentarioPositivo { get; set; }
    public string? ComentarioNegativo { get; set; }
    public string? TipoViaje { get; set; }
}

public sealed class ValoracionModeracionRequest
{
    public string NuevoEstado { get; set; } = string.Empty;
    public string? Motivo { get; set; }
}

public sealed class ValoracionRespuestaRequest
{
    public string Respuesta { get; set; } = string.Empty;
}

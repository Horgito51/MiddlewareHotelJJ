namespace HotelJJ.DataAccess.Http.Routes;

public static class FacturacionRoutes
{
    public const string PublicPagos = "api/v1/public/pagos";
    public const string PublicPagoSimular = "api/v1/public/pagos/simular";
    public const string PagosSimular = "api/v1/pagos/simular";

    public const string InternalFacturas = "api/v1/internal/facturas";
    public const string InternalFacturaByIdTemplate = "api/v1/internal/facturas/{0}";
    public const string InternalFacturaGenerarReservaByIdTemplate = "api/v1/internal/facturas/generar-reserva/{0}";
    public const string InternalFacturaGenerarFinalByIdTemplate = "api/v1/internal/facturas/generar-final/{0}";
    public const string InternalFacturaAnularByIdTemplate = "api/v1/internal/facturas/{0}/anular";

    public const string InternalPagos = "api/v1/internal/pagos";
    public const string InternalPagosByFacturaIdTemplate = "api/v1/internal/pagos/factura/{0}";
    public const string InternalPagoByIdTemplate = "api/v1/internal/pagos/{0}";
    public const string InternalPagoEstadoByIdTemplate = "api/v1/internal/pagos/{0}/estado";
}

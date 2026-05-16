namespace HotelJJ.DataAccess.Http.Routes;

public static class ReservasRoutes
{
    public const string AccommodationsReservas = "api/v1/accommodations/reservas";
    public const string AccommodationsReservaByGuidTemplate = "api/v1/accommodations/reservas/{0}";

    public const string PublicReservas = "api/v1/public/reservas";
    public const string PublicReservaByGuidTemplate = "api/v1/public/reservas/{0}";
    public const string PublicReservaCancelarByGuidTemplate = "api/v1/public/reservas/{0}/cancelar";
    public const string PublicReservasCalcularPrecio = "api/v1/public/reservas/calcular-precio";

    public const string PublicClientes = "api/v1/public/clientes";
    public const string PublicClientesByEmail = "api/v1/public/clientes/by-email";
    public const string PublicClienteByGuidTemplate = "api/v1/public/clientes/{0}";

    public const string InternalReservas = "api/v1/internal/reservas";
    public const string InternalReservaByIdTemplate = "api/v1/internal/reservas/{0}";
    public const string InternalReservasCalcularPrecio = "api/v1/internal/reservas/calcular-precio";

    public const string InternalClientes = "api/v1/internal/clientes";
    public const string InternalClienteByIdTemplate = "api/v1/internal/clientes/{0}";
}

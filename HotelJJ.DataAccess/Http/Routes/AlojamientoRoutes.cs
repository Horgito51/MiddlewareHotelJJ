namespace HotelJJ.DataAccess.Http.Routes;

public static class AlojamientoRoutes
{
    public const string AccommodationsBase = "api/v1/accommodations";
    public const string AccommodationsSearch = "api/v1/accommodations/search";
    public const string AccommodationsBySucursalTemplate = "api/v1/accommodations/{0}";
    public const string AccommodationsReviewsTemplate = "api/v1/accommodations/{0}/reviews";
    public const string AccommodationsHabitacionesTemplate = "api/v1/accommodations/sucursales/{0}/habitaciones";

    public const string PublicHabitaciones = "api/v1/public/habitaciones";
    public const string PublicHabitacionesByGuidTemplate = "api/v1/public/habitaciones/{0}";
    public const string PublicSucursalesByGuidTemplate = "api/v1/public/sucursales/{0}";
    public const string PublicTiposHabitacionByGuidTemplate = "api/v1/public/tipos-habitacion/{0}";

    public const string InternalSucursales = "api/v1/internal/sucursales";
    public const string InternalHabitaciones = "api/v1/internal/habitaciones";
    public const string InternalTiposHabitacion = "api/v1/internal/tipos-habitacion";
    public const string InternalTarifas = "api/v1/internal/tarifas";
    public const string InternalCatalogoServicios = "api/v1/internal/catalogo-servicios";
    public const string InternalValoraciones = "api/v1/internal/valoraciones";
}

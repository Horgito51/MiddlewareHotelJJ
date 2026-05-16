namespace HotelJJ.DataAccess.Http.Routes;

public static class HospedajeRoutes
{
    public const string InternalEstadias = "api/v1/internal/estadias";
    public const string InternalEstadiaByIdTemplate = "api/v1/internal/estadias/{0}";
    public const string InternalCheckInByReservaIdTemplate = "api/v1/internal/estadias/checkin/{0}";
    public const string InternalCheckOutByEstadiaIdTemplate = "api/v1/internal/estadias/{0}/checkout";
    public const string InternalCargosByEstadiaIdTemplate = "api/v1/internal/estadias/{0}/cargos";
    public const string InternalAnularCargoByIdTemplate = "api/v1/internal/cargos-estadia/{0}/anular";
}

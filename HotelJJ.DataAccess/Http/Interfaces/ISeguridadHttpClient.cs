using HotelJJ.DataAccess.Http.Models.Seguridad.Requests;
using HotelJJ.DataAccess.Http.Models.Seguridad.Responses;

namespace HotelJJ.DataAccess.Http.Interfaces;

public interface ISeguridadHttpClient
{
    Task<LoginSeguridadResponseModel> LoginAsync(LoginSeguridadRequestModel request, CancellationToken cancellationToken = default);
    Task<LoginSeguridadResponseModel> RegisterClienteAsync(LoginSeguridadRequestModel request, CancellationToken cancellationToken = default);
    Task<RefreshTokenSeguridadResponseModel> RefreshTokenAsync(RefreshTokenSeguridadRequestModel request, CancellationToken cancellationToken = default);
    Task LogoutAsync(LogoutSeguridadRequestModel request, CancellationToken cancellationToken = default);
    Task CambiarPasswordAsync(
        CambiarPasswordSeguridadRequestModel request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);
}

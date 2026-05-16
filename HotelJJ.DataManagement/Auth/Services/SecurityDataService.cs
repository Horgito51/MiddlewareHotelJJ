using HotelJJ.DataAccess.Http.Interfaces;
using HotelJJ.DataManagement.Auth.Interfaces;
using HotelJJ.DataManagement.Auth.Mappers;
using HotelJJ.DataManagement.Auth.Models;

namespace HotelJJ.DataManagement.Auth.Services;

public class SecurityDataService : ISecurityDataService
{
    private readonly ISeguridadHttpClient _seguridadHttpClient;

    public SecurityDataService(ISeguridadHttpClient seguridadHttpClient)
    {
        _seguridadHttpClient = seguridadHttpClient;
    }

    public async Task<TokenDataModel> LoginAsync(LoginDataRequest request, CancellationToken cancellationToken = default)
    {
        var seguridadRequest = SecurityDataMapper.ToSeguridadRequest(request);
        var seguridadResponse = await _seguridadHttpClient.LoginAsync(seguridadRequest, cancellationToken);

        return SecurityDataMapper.ToTokenDataModel(seguridadResponse);
    }

    public async Task<TokenDataModel> RefreshTokenAsync(RefreshTokenDataRequest request, CancellationToken cancellationToken = default)
    {
        var seguridadRequest = SecurityDataMapper.ToSeguridadRefreshRequest(request);
        var seguridadResponse = await _seguridadHttpClient.RefreshTokenAsync(seguridadRequest, cancellationToken);

        return SecurityDataMapper.ToTokenDataModel(seguridadResponse);
    }

    public async Task<TokenDataModel> RegisterClienteAsync(RegisterClienteDataRequest request, CancellationToken cancellationToken = default)
    {
        var seguridadRequest = SecurityDataMapper.ToSeguridadRegisterRequest(request);
        var seguridadResponse = await _seguridadHttpClient.RegisterClienteAsync(seguridadRequest, cancellationToken);

        return SecurityDataMapper.ToTokenDataModel(seguridadResponse);
    }

    public Task LogoutAsync(LogoutDataRequest request, CancellationToken cancellationToken = default)
    {
        var seguridadRequest = SecurityDataMapper.ToSeguridadLogoutRequest(request);
        return _seguridadHttpClient.LogoutAsync(seguridadRequest, cancellationToken);
    }

    public Task CambiarPasswordAsync(
        CambiarPasswordDataRequest request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var seguridadRequest = SecurityDataMapper.ToSeguridadCambiarPasswordRequest(request);
        return _seguridadHttpClient.CambiarPasswordAsync(seguridadRequest, authorizationHeader, cancellationToken);
    }
}

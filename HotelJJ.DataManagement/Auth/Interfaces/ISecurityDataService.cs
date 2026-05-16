using HotelJJ.DataManagement.Auth.Models;

namespace HotelJJ.DataManagement.Auth.Interfaces;

public interface ISecurityDataService
{
    Task<TokenDataModel> LoginAsync(LoginDataRequest request, CancellationToken cancellationToken = default);
    Task<TokenDataModel> RegisterClienteAsync(RegisterClienteDataRequest request, CancellationToken cancellationToken = default);
    Task<TokenDataModel> RefreshTokenAsync(RefreshTokenDataRequest request, CancellationToken cancellationToken = default);
    Task LogoutAsync(LogoutDataRequest request, CancellationToken cancellationToken = default);
    Task CambiarPasswordAsync(
        CambiarPasswordDataRequest request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);
}

using HotelJJ.Business.DTOs.Auth;

namespace HotelJJ.Business.Interfaces.Auth;

public interface ISecurityOrchestrationService
{
    Task<TokenIntegrationDTO> LoginAsync(LoginIntegrationDTO request, CancellationToken cancellationToken = default);
    Task<TokenIntegrationDTO> RegisterClienteAsync(RegisterClienteIntegrationDTO request, CancellationToken cancellationToken = default);
    Task<TokenIntegrationDTO> RefreshTokenAsync(RefreshTokenIntegrationDTO request, CancellationToken cancellationToken = default);
    Task LogoutAsync(LogoutIntegrationDTO request, CancellationToken cancellationToken = default);
    Task CambiarPasswordAsync(
        CambiarPasswordIntegrationDTO request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);
}

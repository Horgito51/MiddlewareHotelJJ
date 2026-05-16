using System.Net;
using HotelJJ.Business.DTOs.Auth;
using HotelJJ.Business.Exceptions;
using HotelJJ.Business.Interfaces.Auth;
using HotelJJ.Business.Mappers.Auth;
using HotelJJ.Business.Validators.Auth;
using HotelJJ.DataAccess.Http.Common;
using HotelJJ.DataManagement.Auth.Interfaces;

namespace HotelJJ.Business.Services.Auth;

public class SecurityOrchestrationService : ISecurityOrchestrationService
{
    private readonly ISecurityDataService _securityDataService;

    public SecurityOrchestrationService(ISecurityDataService securityDataService)
    {
        _securityDataService = securityDataService;
    }

    public async Task<TokenIntegrationDTO> LoginAsync(LoginIntegrationDTO request, CancellationToken cancellationToken = default)
    {
        LoginIntegrationValidator.Validate(request);

        return await ExecuteSecurityTokenOperationAsync(
            () => _securityDataService.LoginAsync(AuthBusinessMapper.ToDataRequest(request), cancellationToken));
    }

    public async Task<TokenIntegrationDTO> RefreshTokenAsync(RefreshTokenIntegrationDTO request, CancellationToken cancellationToken = default)
    {
        RefreshTokenIntegrationValidator.Validate(request);

        return await ExecuteSecurityTokenOperationAsync(
            () => _securityDataService.RefreshTokenAsync(AuthBusinessMapper.ToDataRequest(request), cancellationToken));
    }

    public async Task<TokenIntegrationDTO> RegisterClienteAsync(RegisterClienteIntegrationDTO request, CancellationToken cancellationToken = default)
    {
        RegisterClienteIntegrationValidator.Validate(request);

        return await ExecuteSecurityTokenOperationAsync(
            () => _securityDataService.RegisterClienteAsync(AuthBusinessMapper.ToDataRequest(request), cancellationToken));
    }

    public async Task LogoutAsync(LogoutIntegrationDTO request, CancellationToken cancellationToken = default)
    {
        LogoutIntegrationValidator.Validate(request);

        await ExecuteSecurityNoContentOperationAsync(
            () => _securityDataService.LogoutAsync(AuthBusinessMapper.ToDataRequest(request), cancellationToken));
    }

    public async Task CambiarPasswordAsync(
        CambiarPasswordIntegrationDTO request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        CambiarPasswordIntegrationValidator.Validate(request);

        await ExecuteSecurityNoContentOperationAsync(
            () => _securityDataService.CambiarPasswordAsync(
                AuthBusinessMapper.ToDataRequest(request),
                authorizationHeader,
                cancellationToken));
    }

    private static async Task<TokenIntegrationDTO> ExecuteSecurityTokenOperationAsync(Func<Task<DataManagement.Auth.Models.TokenDataModel>> operation)
    {
        try
        {
            var token = await operation();
            return AuthBusinessMapper.ToIntegrationToken(token);
        }
        catch (DownstreamApiException ex) when (ex.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
        {
            throw new IntegrationUnauthorizedException("MID-AUTH-005", "Credenciales invalidas.", ex);
        }
        catch (DownstreamApiException ex)
        {
            throw new IntegrationBusinessException("MID-AUTH-006", ex.Message, ex);
        }
        catch (HttpRequestException ex)
        {
            throw new IntegrationBusinessException("MID-AUTH-007", "No fue posible conectar con el microservicio Seguridad.", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new IntegrationBusinessException("MID-AUTH-008", "La solicitud al microservicio Seguridad excedio el tiempo de espera.", ex);
        }
    }

    private static async Task ExecuteSecurityNoContentOperationAsync(Func<Task> operation)
    {
        try
        {
            await operation();
        }
        catch (DownstreamApiException ex) when (ex.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
        {
            throw new IntegrationUnauthorizedException("MID-AUTH-019", "No autorizado para ejecutar la operacion de seguridad.", ex);
        }
        catch (DownstreamApiException ex) when (ex.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.UnprocessableEntity)
        {
            throw new IntegrationValidationException("MID-AUTH-020", ex.Message);
        }
        catch (DownstreamApiException ex)
        {
            throw new IntegrationBusinessException("MID-AUTH-021", ex.Message, ex);
        }
        catch (HttpRequestException ex)
        {
            throw new IntegrationBusinessException("MID-AUTH-022", "No fue posible conectar con el microservicio Seguridad.", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new IntegrationBusinessException("MID-AUTH-023", "La solicitud al microservicio Seguridad excedio el tiempo de espera.", ex);
        }
    }
}

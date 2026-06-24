using Asp.Versioning;
using HotelJJ.API.Models.Common;
using HotelJJ.API.Models.Requests.Auth;
using HotelJJ.API.Models.Responses.Auth;
using HotelJJ.Business.DTOs.Auth;
using HotelJJ.Business.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HotelJJ.API.Controllers.V1.Auth;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
[Route("api/v{version:apiVersion}/internal/auth")]
public class AuthIntegrationController : ControllerBase
{
    private readonly ISecurityOrchestrationService _securityOrchestrationService;
    private readonly ILogger<AuthIntegrationController> _logger;

    public AuthIntegrationController(
        ISecurityOrchestrationService securityOrchestrationService,
        ILogger<AuthIntegrationController> logger)
    {
        _securityOrchestrationService = securityOrchestrationService;
        _logger = logger;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiSuccessResponse<LoginIntegrationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiSuccessResponse<LoginIntegrationResponse>>> Login(
        [FromBody] LoginIntegrationRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogWarning("[IAST SIM] Endpoint ejecutado: POST /api/v1/auth/login");
        _logger.LogWarning("[IAST SIM] Username recibido: {Username}", request.Username);
        _logger.LogWarning("[IAST SIM] Longitud de password recibido: {PasswordLength}", request.Password?.Length ?? 0);

        if (!string.IsNullOrWhiteSpace(request.Username) &&
            (
                request.Username.Contains("'") ||
                request.Username.Contains("--") ||
                request.Username.Contains(" OR ", StringComparison.OrdinalIgnoreCase) ||
                request.Username.Contains("1=1")
            ))
        {
            _logger.LogWarning("[IAST SIM] Posible payload sospechoso detectado en Username: {Username}", request.Username);
        }

        _logger.LogWarning("[IAST SIM] Enviando credenciales al servicio de autenticación");

        var token = await _securityOrchestrationService.LoginAsync(new LoginIntegrationDTO
        {
            Username = request.Username,
            Password = request.Password
        }, cancellationToken);

        _logger.LogWarning("[IAST SIM] Login procesado correctamente para usuario: {Username}", request.Username);

        return Ok(new ApiSuccessResponse<LoginIntegrationResponse>(ToResponse(token), "Autenticación exitosa"));
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiSuccessResponse<LoginIntegrationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiSuccessResponse<LoginIntegrationResponse>>> Refresh(
        [FromBody] RefreshTokenIntegrationRequest request,
        CancellationToken cancellationToken)
    {
        var token = await _securityOrchestrationService.RefreshTokenAsync(new RefreshTokenIntegrationDTO
        {
            RefreshToken = request.RefreshToken
        }, cancellationToken);

        return Ok(new ApiSuccessResponse<LoginIntegrationResponse>(ToResponse(token), "Token actualizado exitosamente"));
    }

    [HttpPost("register-cliente")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiSuccessResponse<LoginIntegrationResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiSuccessResponse<LoginIntegrationResponse>>> RegisterCliente(
        [FromBody] RegisterClienteIntegrationRequest request,
        CancellationToken cancellationToken)
    {
        var token = await _securityOrchestrationService.RegisterClienteAsync(new RegisterClienteIntegrationDTO
        {
            Username = request.Username,
            Password = request.Password
        }, cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            new ApiSuccessResponse<LoginIntegrationResponse>(ToResponse(token), "Registro exitoso"));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        [FromBody] LogoutIntegrationRequest request,
        CancellationToken cancellationToken)
    {
        await _securityOrchestrationService.LogoutAsync(new LogoutIntegrationDTO
        {
            RefreshToken = request.RefreshToken
        }, cancellationToken);

        return NoContent();
    }

    [HttpPost("cambiar-password")]
    public async Task<IActionResult> CambiarPassword(
        [FromBody] CambiarPasswordIntegrationRequest request,
        CancellationToken cancellationToken)
    {
        await _securityOrchestrationService.CambiarPasswordAsync(
            new CambiarPasswordIntegrationDTO
            {
                PasswordActual = request.PasswordActual,
                PasswordNuevo = request.PasswordNuevo
            },
            Request.Headers.Authorization.ToString(),
            cancellationToken);

        return NoContent();
    }

    private static LoginIntegrationResponse ToResponse(TokenIntegrationDTO token)
    {
        return new LoginIntegrationResponse
        {
            Token = token.Token,
            RefreshToken = token.RefreshToken,
            Expiration = token.Expiration,
            UsuarioId = token.UsuarioId,
            UsuarioGuid = token.UsuarioGuid,
            Username = token.Username,
            Email = token.Email,
            Roles = token.Roles
        };
    }
}
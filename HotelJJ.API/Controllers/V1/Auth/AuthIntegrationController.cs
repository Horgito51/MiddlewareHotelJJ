using Asp.Versioning;
using HotelJJ.API.Models.Requests.Auth;
using HotelJJ.API.Models.Responses.Auth;
using HotelJJ.Business.DTOs.Auth;
using HotelJJ.Business.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelJJ.API.Controllers.V1.Auth;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
[Route("api/v{version:apiVersion}/internal/auth")]
public class AuthIntegrationController : ControllerBase
{
    private readonly ISecurityOrchestrationService _securityOrchestrationService;

    public AuthIntegrationController(ISecurityOrchestrationService securityOrchestrationService)
    {
        _securityOrchestrationService = securityOrchestrationService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginIntegrationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginIntegrationResponse>> Login(
        [FromBody] LoginIntegrationRequest request,
        CancellationToken cancellationToken)
    {
        var token = await _securityOrchestrationService.LoginAsync(new LoginIntegrationDTO
        {
            Username = request.Username,
            Password = request.Password
        }, cancellationToken);

        return Ok(ToResponse(token));
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginIntegrationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginIntegrationResponse>> Refresh(
        [FromBody] RefreshTokenIntegrationRequest request,
        CancellationToken cancellationToken)
    {
        var token = await _securityOrchestrationService.RefreshTokenAsync(new RefreshTokenIntegrationDTO
        {
            RefreshToken = request.RefreshToken
        }, cancellationToken);

        return Ok(ToResponse(token));
    }

    [HttpPost("register-cliente")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginIntegrationResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<LoginIntegrationResponse>> RegisterCliente(
        [FromBody] RegisterClienteIntegrationRequest request,
        CancellationToken cancellationToken)
    {
        var token = await _securityOrchestrationService.RegisterClienteAsync(new RegisterClienteIntegrationDTO
        {
            Username = request.Username,
            Password = request.Password
        }, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, ToResponse(token));
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
            AccessToken = token.AccessToken,
            RefreshToken = token.RefreshToken,
            ExpiresIn = token.ExpiresIn,
            ExpiresAt = token.ExpiresAt,
            IdCliente = token.IdCliente,
            ClienteGuid = token.ClienteGuid,
            UsuarioGuid = token.UsuarioGuid,
            Username = token.Username,
            Correo = token.Correo,
            NombreCompleto = token.NombreCompleto,
            Roles = token.Roles
        };
    }
}

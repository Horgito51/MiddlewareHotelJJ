using Asp.Versioning;
using HotelJJ.API.Infrastructure.Proxy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelJJ.API.Controllers.V1.Gateway;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class SeguridadGatewayController : ControllerBase
{
    private readonly IMicroserviceProxy _microserviceProxy;

    public SeguridadGatewayController(IMicroserviceProxy microserviceProxy)
    {
        _microserviceProxy = microserviceProxy;
    }

    [HttpGet("/api/v{version:apiVersion}/internal/usuarios")]
    public Task<IActionResult> GetUsuarios(CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/usuarios/{id:int}")]
    public Task<IActionResult> GetUsuarioById(int id, CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/usuarios/{usuarioGuid:guid}")]
    public Task<IActionResult> GetUsuarioByGuid(Guid usuarioGuid, CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpPost("/api/v{version:apiVersion}/internal/usuarios")]
    public Task<IActionResult> CreateUsuario(CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpPut("/api/v{version:apiVersion}/internal/usuarios/{id:int}")]
    public Task<IActionResult> UpdateUsuarioById(int id, CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpPut("/api/v{version:apiVersion}/internal/usuarios/{usuarioGuid:guid}")]
    public Task<IActionResult> UpdateUsuarioByGuid(Guid usuarioGuid, CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpPatch("/api/v{version:apiVersion}/internal/usuarios/{id:int}/inhabilitar")]
    public Task<IActionResult> InhabilitarUsuario(int id, CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpDelete("/api/v{version:apiVersion}/internal/usuarios/{id:int}")]
    public Task<IActionResult> DeleteUsuarioById(int id, CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpDelete("/api/v{version:apiVersion}/internal/usuarios/{usuarioGuid:guid}")]
    public Task<IActionResult> DeleteUsuarioByGuid(Guid usuarioGuid, CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/roles")]
    public Task<IActionResult> GetRoles(CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/roles/{id:int}")]
    public Task<IActionResult> GetRolById(int id, CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/roles/{rolGuid:guid}")]
    public Task<IActionResult> GetRolByGuid(Guid rolGuid, CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpPost("/api/v{version:apiVersion}/internal/roles")]
    public Task<IActionResult> CreateRol(CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpPut("/api/v{version:apiVersion}/internal/roles/{id:int}")]
    public Task<IActionResult> UpdateRolById(int id, CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpPut("/api/v{version:apiVersion}/internal/roles/{rolGuid:guid}")]
    public Task<IActionResult> UpdateRolByGuid(Guid rolGuid, CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpDelete("/api/v{version:apiVersion}/internal/roles/{id:int}")]
    public Task<IActionResult> DeleteRolById(int id, CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpDelete("/api/v{version:apiVersion}/internal/roles/{rolGuid:guid}")]
    public Task<IActionResult> DeleteRolByGuid(Guid rolGuid, CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpPost("/api/v{version:apiVersion}/internal/roles/{rolGuid:guid}/permisos")]
    public Task<IActionResult> AsignarPermisosRol(Guid rolGuid, CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/permisos")]
    public Task<IActionResult> GetPermisos(CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/auditoria")]
    public Task<IActionResult> GetAuditoria(CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/auditoria/{auditoriaGuid:guid}")]
    public Task<IActionResult> GetAuditoriaByGuid(Guid auditoriaGuid, CancellationToken cancellationToken) => ProxyToSeguridadAsync(cancellationToken);

    private async Task<IActionResult> ProxyToSeguridadAsync(CancellationToken cancellationToken)
    {
        await _microserviceProxy.ProxyAsync("Seguridad", HttpContext, cancellationToken: cancellationToken);
        return new EmptyResult();
    }
}

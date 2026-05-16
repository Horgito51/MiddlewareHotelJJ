using Asp.Versioning;
using HotelJJ.API.Infrastructure.Proxy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelJJ.API.Controllers.V1.Gateway;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class AlojamientoGatewayController : ControllerBase
{
    private readonly IMicroserviceProxy _microserviceProxy;

    public AlojamientoGatewayController(IMicroserviceProxy microserviceProxy)
    {
        _microserviceProxy = microserviceProxy;
    }

    [HttpGet("/api/v{version:apiVersion}/public/habitaciones")]
    [AllowAnonymous]
    public Task<IActionResult> GetPublicHabitaciones(CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/public/habitaciones/{habitacionGuid:guid}")]
    [AllowAnonymous]
    public Task<IActionResult> GetPublicHabitacionByGuid(Guid habitacionGuid, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/public/sucursales/{sucursalGuid:guid}")]
    [AllowAnonymous]
    public Task<IActionResult> GetPublicSucursalByGuid(Guid sucursalGuid, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/public/tipos-habitacion/{tipoHabitacionGuid:guid}")]
    [AllowAnonymous]
    public Task<IActionResult> GetPublicTipoHabitacionByGuid(Guid tipoHabitacionGuid, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/sucursales")]
    public Task<IActionResult> GetSucursales(CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/sucursales/{id:int}")]
    public Task<IActionResult> GetSucursalById(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/sucursales/{sucursalGuid:guid}")]
    public Task<IActionResult> GetSucursalByGuid(Guid sucursalGuid, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpPost("/api/v{version:apiVersion}/internal/sucursales")]
    public Task<IActionResult> CreateSucursal(CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpPut("/api/v{version:apiVersion}/internal/sucursales/{id:int}")]
    public Task<IActionResult> UpdateSucursalById(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpPut("/api/v{version:apiVersion}/internal/sucursales/{sucursalGuid:guid}")]
    public Task<IActionResult> UpdateSucursalByGuid(Guid sucursalGuid, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpPatch("/api/v{version:apiVersion}/internal/sucursales/{sucursalGuid:guid}/politicas")]
    public Task<IActionResult> UpdateSucursalPoliticas(Guid sucursalGuid, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpPatch("/api/v{version:apiVersion}/internal/sucursales/{sucursalGuid:guid}/inhabilitar")]
    public Task<IActionResult> InhabilitarSucursal(Guid sucursalGuid, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpDelete("/api/v{version:apiVersion}/internal/sucursales/{id:int}")]
    public Task<IActionResult> DeleteSucursalById(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpDelete("/api/v{version:apiVersion}/internal/sucursales/{sucursalGuid:guid}")]
    public Task<IActionResult> DeleteSucursalByGuid(Guid sucursalGuid, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/habitaciones")]
    public Task<IActionResult> GetHabitaciones(CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/habitaciones/{id:int}")]
    public Task<IActionResult> GetHabitacionById(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/habitaciones/{habitacionGuid:guid}")]
    public Task<IActionResult> GetHabitacionByGuid(Guid habitacionGuid, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpPost("/api/v{version:apiVersion}/internal/habitaciones")]
    public Task<IActionResult> CreateHabitacion(CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpPut("/api/v{version:apiVersion}/internal/habitaciones/{id:int}")]
    public Task<IActionResult> UpdateHabitacionById(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpPatch("/api/v{version:apiVersion}/internal/habitaciones/{id:int}/estado")]
    public Task<IActionResult> ChangeHabitacionEstado(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpDelete("/api/v{version:apiVersion}/internal/habitaciones/{id:int}")]
    public Task<IActionResult> DeleteHabitacionById(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/tipos-habitacion")]
    public Task<IActionResult> GetTiposHabitacion(CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/tipos-habitacion/{id:int}")]
    public Task<IActionResult> GetTipoHabitacionById(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/tipos-habitacion/{tipoGuid:guid}")]
    public Task<IActionResult> GetTipoHabitacionByGuid(Guid tipoGuid, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpPost("/api/v{version:apiVersion}/internal/tipos-habitacion")]
    public Task<IActionResult> CreateTipoHabitacion(CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpPut("/api/v{version:apiVersion}/internal/tipos-habitacion/{id:int}")]
    public Task<IActionResult> UpdateTipoHabitacionById(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpPut("/api/v{version:apiVersion}/internal/tipos-habitacion/{tipoGuid:guid}")]
    public Task<IActionResult> UpdateTipoHabitacionByGuid(Guid tipoGuid, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpDelete("/api/v{version:apiVersion}/internal/tipos-habitacion/{id:int}")]
    public Task<IActionResult> DeleteTipoHabitacionById(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/tarifas")]
    public Task<IActionResult> GetTarifas(CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/tarifas/{id:int}")]
    public Task<IActionResult> GetTarifaById(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpPost("/api/v{version:apiVersion}/internal/tarifas")]
    public Task<IActionResult> CreateTarifa(CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpPut("/api/v{version:apiVersion}/internal/tarifas/{id:int}")]
    public Task<IActionResult> UpdateTarifaById(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpPatch("/api/v{version:apiVersion}/internal/tarifas/{id:int}/desactivar")]
    public Task<IActionResult> DesactivarTarifa(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpDelete("/api/v{version:apiVersion}/internal/tarifas/{id:int}")]
    public Task<IActionResult> DeleteTarifaById(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/catalogo-servicios")]
    public Task<IActionResult> GetCatalogoServicios(CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/catalogo-servicios/{id:int}")]
    public Task<IActionResult> GetCatalogoServicioById(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpPost("/api/v{version:apiVersion}/internal/catalogo-servicios")]
    public Task<IActionResult> CreateCatalogoServicio(CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpPut("/api/v{version:apiVersion}/internal/catalogo-servicios/{id:int}")]
    public Task<IActionResult> UpdateCatalogoServicioById(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpDelete("/api/v{version:apiVersion}/internal/catalogo-servicios/{id:int}")]
    public Task<IActionResult> DeleteCatalogoServicioById(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/valoraciones")]
    public Task<IActionResult> GetValoraciones(CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/valoraciones/{id:int}")]
    public Task<IActionResult> GetValoracionById(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpPost("/api/v{version:apiVersion}/internal/valoraciones")]
    public Task<IActionResult> CreateValoracion(CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpPatch("/api/v{version:apiVersion}/internal/valoraciones/{id:int}/moderar")]
    public Task<IActionResult> ModerarValoracion(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpPatch("/api/v{version:apiVersion}/internal/valoraciones/{id:int}/responder")]
    public Task<IActionResult> ResponderValoracion(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    [HttpDelete("/api/v{version:apiVersion}/internal/valoraciones/{id:int}")]
    public Task<IActionResult> DeleteValoracionById(int id, CancellationToken cancellationToken) => ProxyToAlojamientoAsync(cancellationToken);

    private async Task<IActionResult> ProxyToAlojamientoAsync(CancellationToken cancellationToken)
    {
        await _microserviceProxy.ProxyAsync("Alojamiento", HttpContext, cancellationToken: cancellationToken);
        return new EmptyResult();
    }
}

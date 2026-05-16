using Asp.Versioning;
using HotelJJ.API.Infrastructure.Proxy;
using HotelJJ.API.Models.Requests.Hospedaje;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelJJ.API.Controllers.V1.Gateway;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class HospedajeGatewayController : ControllerBase
{
    private readonly IMicroserviceProxy _microserviceProxy;

    public HospedajeGatewayController(IMicroserviceProxy microserviceProxy)
    {
        _microserviceProxy = microserviceProxy;
    }

    [HttpGet("/api/v{version:apiVersion}/internal/estadias")]
    public Task<IActionResult> GetEstadias(CancellationToken cancellationToken) => ProxyToHospedajeAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/estadias/{id:int}")]
    public Task<IActionResult> GetEstadiaById(int id, CancellationToken cancellationToken) => ProxyToHospedajeAsync(cancellationToken);

    [HttpPost("/api/v{version:apiVersion}/internal/estadias/checkin/{id_reserva:int}")]
    public Task<IActionResult> CheckIn(int id_reserva, CancellationToken cancellationToken) => ProxyToHospedajeAsync(cancellationToken);

    [HttpPatch("/api/v{version:apiVersion}/internal/estadias/{id:int}/checkout")]
    public Task<IActionResult> CheckOut(
        int id,
        [FromBody] CheckOutHospedajeRequest request,
        CancellationToken cancellationToken) => ProxyToHospedajeAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/estadias/{id:int}/cargos")]
    public Task<IActionResult> GetCargosEstadia(int id, CancellationToken cancellationToken) => ProxyToHospedajeAsync(cancellationToken);

    [HttpPost("/api/v{version:apiVersion}/internal/estadias/{id:int}/cargos")]
    public Task<IActionResult> AddCargoEstadia(
        int id,
        [FromBody] CargoHospedajeCreateRequest request,
        CancellationToken cancellationToken) => ProxyToHospedajeAsync(cancellationToken);

    [HttpPatch("/api/v{version:apiVersion}/internal/cargos-estadia/{id:int}/anular")]
    public Task<IActionResult> AnularCargoEstadia(int id, CancellationToken cancellationToken) => ProxyToHospedajeAsync(cancellationToken);

    private async Task<IActionResult> ProxyToHospedajeAsync(CancellationToken cancellationToken)
    {
        await _microserviceProxy.ProxyAsync("Hospedaje", HttpContext, cancellationToken: cancellationToken);
        return new EmptyResult();
    }
}

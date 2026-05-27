using Asp.Versioning;
using HotelJJ.API.Infrastructure.Proxy;
using HotelJJ.API.Models.Requests.Reservas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelJJ.API.Controllers.V1.Gateway;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class ReservasGatewayController : ControllerBase
{
    private readonly IMicroserviceProxy _microserviceProxy;

    public ReservasGatewayController(IMicroserviceProxy microserviceProxy)
    {
        _microserviceProxy = microserviceProxy;
    }

    [HttpGet("/api/v{version:apiVersion}/public/reservas")]
    [AllowAnonymous]
    public Task<IActionResult> GetPublicReservas(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 50,
        [FromQuery] string? estado = null,
        CancellationToken cancellationToken = default) => ProxyToReservasAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/public/clientes/by-email")]
    [AllowAnonymous]
    public Task<IActionResult> GetPublicClienteByEmail(
        [FromQuery] string correo,
        CancellationToken cancellationToken) => ProxyToReservasAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/public/clientes/{clienteGuid:guid}")]
    [AllowAnonymous]
    public Task<IActionResult> GetPublicClienteByGuid(Guid clienteGuid, CancellationToken cancellationToken) => ProxyToReservasAsync(cancellationToken);

    [HttpPost("/api/v{version:apiVersion}/public/clientes")]
    [AllowAnonymous]
    public Task<IActionResult> CreatePublicCliente(
        [FromBody] ClienteCreateRequest request,
        CancellationToken cancellationToken) => ProxyToReservasAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/reservas")]
    public Task<IActionResult> GetInternalReservas(CancellationToken cancellationToken) => ProxyToReservasAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/reservas/{id:int}")]
    public Task<IActionResult> GetInternalReservaById(int id, CancellationToken cancellationToken) => ProxyToReservasAsync(cancellationToken);

    [HttpPost("/api/v{version:apiVersion}/internal/reservas")]
    public Task<IActionResult> CreateInternalReserva(
        [FromBody] InternalReservaCreateRequest request,
        CancellationToken cancellationToken) => ProxyToReservasAsync(cancellationToken);

    [HttpPost("/api/v{version:apiVersion}/internal/reservas/calcular-precio")]
    public Task<IActionResult> CalculateInternalReservaPrecio(
        [FromBody] ReservationPriceRequest request,
        CancellationToken cancellationToken) => ProxyToReservasAsync(cancellationToken);

    [HttpPut("/api/v{version:apiVersion}/internal/reservas/{id:int}")]
    public Task<IActionResult> UpdateInternalReserva(
        int id,
        [FromBody] ReservaUpdateRequest request,
        CancellationToken cancellationToken) => ProxyToReservasAsync(cancellationToken);

    [HttpPatch("/api/v{version:apiVersion}/internal/reservas/{id:int}/confirmar")]
    public Task<IActionResult> ConfirmInternalReserva(int id, CancellationToken cancellationToken) => ProxyToReservasAsync(cancellationToken);

    [HttpPatch("/api/v{version:apiVersion}/internal/reservas/{id:int}/cancelar")]
    public Task<IActionResult> CancelInternalReserva(
        int id,
        [FromBody] CancelReservationRequest request,
        CancellationToken cancellationToken) => ProxyToReservasAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/clientes")]
    public Task<IActionResult> GetInternalClientes(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default) => ProxyToReservasAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/clientes/{id:int}")]
    public Task<IActionResult> GetInternalClienteById(int id, CancellationToken cancellationToken) => ProxyToReservasAsync(cancellationToken);

    [HttpPost("/api/v{version:apiVersion}/internal/clientes")]
    public Task<IActionResult> CreateInternalCliente(
        [FromBody] ClienteCreateRequest request,
        CancellationToken cancellationToken) => ProxyToReservasAsync(cancellationToken);

    [HttpPut("/api/v{version:apiVersion}/internal/clientes/{id:int}")]
    public Task<IActionResult> UpdateInternalCliente(
        int id,
        [FromBody] ClienteUpdateRequest request,
        CancellationToken cancellationToken) => ProxyToReservasAsync(cancellationToken);

    [HttpDelete("/api/v{version:apiVersion}/internal/clientes/{id:int}")]
    public Task<IActionResult> DeleteInternalCliente(int id, CancellationToken cancellationToken) => ProxyToReservasAsync(cancellationToken);

    private async Task<IActionResult> ProxyToReservasAsync(CancellationToken cancellationToken)
    {
        await _microserviceProxy.ProxyAsync("Reservas", HttpContext, cancellationToken: cancellationToken);
        return new EmptyResult();
    }
}

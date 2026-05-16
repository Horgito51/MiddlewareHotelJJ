using Asp.Versioning;
using HotelJJ.API.Infrastructure.Proxy;
using HotelJJ.API.Models.Requests.Facturacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelJJ.API.Controllers.V1.Gateway;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class FacturacionGatewayController : ControllerBase
{
    private readonly IMicroserviceProxy _microserviceProxy;

    public FacturacionGatewayController(IMicroserviceProxy microserviceProxy)
    {
        _microserviceProxy = microserviceProxy;
    }

    [HttpGet("/api/v{version:apiVersion}/internal/facturas")]
    public Task<IActionResult> GetFacturas(CancellationToken cancellationToken) => ProxyToFacturacionAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/facturas/{id:int}")]
    public Task<IActionResult> GetFacturaById(int id, CancellationToken cancellationToken) => ProxyToFacturacionAsync(cancellationToken);

    [HttpPost("/api/v{version:apiVersion}/internal/facturas/generar-reserva/{id:int}")]
    public Task<IActionResult> GenerarFacturaReserva(int id, CancellationToken cancellationToken) => ProxyToFacturacionAsync(cancellationToken);

    [HttpPost("/api/v{version:apiVersion}/internal/facturas/generar-final/{id:int}")]
    public Task<IActionResult> GenerarFacturaFinal(int id, CancellationToken cancellationToken) => ProxyToFacturacionAsync(cancellationToken);

    [HttpPatch("/api/v{version:apiVersion}/internal/facturas/{id:int}/anular")]
    public Task<IActionResult> AnularFactura(
        int id,
        [FromBody] AnularFacturaRequest request,
        CancellationToken cancellationToken) => ProxyToFacturacionAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/pagos")]
    public Task<IActionResult> GetPagos(CancellationToken cancellationToken) => ProxyToFacturacionAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/pagos/{id:int}")]
    public Task<IActionResult> GetPagoById(int id, CancellationToken cancellationToken) => ProxyToFacturacionAsync(cancellationToken);

    [HttpGet("/api/v{version:apiVersion}/internal/pagos/factura/{facturaId:int}")]
    public Task<IActionResult> GetPagosByFacturaId(int facturaId, CancellationToken cancellationToken) => ProxyToFacturacionAsync(cancellationToken);

    [HttpPatch("/api/v{version:apiVersion}/internal/pagos/{id:int}/estado")]
    public Task<IActionResult> ActualizarEstadoPago(
        int id,
        [FromBody] PagoEstadoRequest request,
        CancellationToken cancellationToken) => ProxyToFacturacionAsync(cancellationToken);

    [HttpPost("/api/v{version:apiVersion}/pagos/simular")]
    public Task<IActionResult> SimularPagoInterno(
        [FromBody] GatewayPagoSimularRequest request,
        CancellationToken cancellationToken) => ProxyToFacturacionAsync(cancellationToken);

    [HttpPost("/api/v{version:apiVersion}/public/pagos/simular")]
    [AllowAnonymous]
    public Task<IActionResult> SimularPagoPublico(
        [FromBody] PublicPagoSimularRequest request,
        CancellationToken cancellationToken) => ProxyToFacturacionAsync(cancellationToken);

    private async Task<IActionResult> ProxyToFacturacionAsync(CancellationToken cancellationToken)
    {
        await _microserviceProxy.ProxyAsync("Facturacion", HttpContext, cancellationToken: cancellationToken);
        return new EmptyResult();
    }
}

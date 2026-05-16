using Asp.Versioning;
using HotelJJ.API.Models.Requests.Facturacion;
using HotelJJ.Business.DTOs.Facturacion;
using HotelJJ.Business.Interfaces.Facturacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelJJ.API.Controllers.V1.Facturacion;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/internal")]
[Authorize]
public class FacturacionIntegrationController : ControllerBase
{
    private readonly IFacturacionOrchestrationService _facturacionOrchestrationService;

    public FacturacionIntegrationController(IFacturacionOrchestrationService facturacionOrchestrationService)
    {
        _facturacionOrchestrationService = facturacionOrchestrationService;
    }

    [HttpGet("facturas/{facturaGuid:guid}")]
    public async Task<ActionResult<FacturaDTO>> GetFactura(Guid facturaGuid, CancellationToken cancellationToken)
    {
        var result = await _facturacionOrchestrationService.GetFacturaAsync(
            facturaGuid,
            Request.Headers.Authorization.ToString(),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("facturas/generar-reserva/{reservaGuid:guid}")]
    public async Task<ActionResult<FacturaDTO>> GenerarFacturaReserva(
        Guid reservaGuid,
        CancellationToken cancellationToken)
    {
        var result = await _facturacionOrchestrationService.GenerarFacturaReservaAsync(
            reservaGuid,
            Request.Headers.Authorization.ToString(),
            cancellationToken);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPost("facturas/generar-final/{reservaGuid:guid}")]
    public async Task<ActionResult<FacturaDTO>> GenerarFacturaFinal(
        Guid reservaGuid,
        CancellationToken cancellationToken)
    {
        var result = await _facturacionOrchestrationService.GenerarFacturaFinalAsync(
            reservaGuid,
            Request.Headers.Authorization.ToString(),
            cancellationToken);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPost("pagos/simular")]
    public async Task<ActionResult<PagoSimuladoDTO>> SimularPago(
        [FromBody] PagoSimularRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _facturacionOrchestrationService.SimularPagoAsync(
            new PagoSimularDTO
            {
                ReservaGuid = request.ReservaGuid,
                Monto = request.Monto,
                TokenPago = request.TokenPago,
                Referencia = request.Referencia
            },
            Request.Headers.Authorization.ToString(),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("pagos")]
    public async Task<ActionResult<PagoDTO>> RegistrarPago(
        [FromBody] PagoCreateRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _facturacionOrchestrationService.RegistrarPagoAsync(
            new PagoCreateDTO
            {
                FacturaGuid = request.FacturaGuid,
                Monto = request.Monto,
                MetodoPago = request.MetodoPago,
                EsPagoElectronico = request.EsPagoElectronico,
                ProveedorPasarela = request.ProveedorPasarela,
                TransaccionExterna = request.TransaccionExterna,
                CodigoAutorizacion = request.CodigoAutorizacion,
                Referencia = request.Referencia,
                Moneda = request.Moneda,
                TipoCambio = request.TipoCambio
            },
            Request.Headers.Authorization.ToString(),
            cancellationToken);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet("facturas/{facturaGuid:guid}/saldo")]
    public async Task<ActionResult<FacturaSaldoDTO>> GetSaldo(
        Guid facturaGuid,
        CancellationToken cancellationToken)
    {
        var result = await _facturacionOrchestrationService.GetSaldoAsync(
            facturaGuid,
            Request.Headers.Authorization.ToString(),
            cancellationToken);

        return Ok(result);
    }
}

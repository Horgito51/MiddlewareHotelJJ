using HotelJJ.Business.DTOs.Facturacion;

namespace HotelJJ.Business.Interfaces.Facturacion;

public interface IFacturacionOrchestrationService
{
    Task<FacturaDTO> GetFacturaAsync(
        Guid facturaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<FacturaDTO> GenerarFacturaReservaAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<FacturaDTO> GenerarFacturaFinalAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<PagoSimuladoDTO> SimularPagoAsync(
        PagoSimularDTO request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<PagoDTO> RegistrarPagoAsync(
        PagoCreateDTO request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<FacturaSaldoDTO> GetSaldoAsync(
        Guid facturaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);
}

using System.Net;
using HotelJJ.Business.DTOs.Facturacion;
using HotelJJ.Business.Exceptions;
using HotelJJ.Business.Interfaces.Facturacion;
using HotelJJ.Business.Mappers.Facturacion;
using HotelJJ.Business.Validators.Facturacion;
using HotelJJ.DataAccess.Http.Common;
using HotelJJ.DataManagement.Common.Identifiers;
using HotelJJ.DataManagement.Facturacion.Interfaces;

namespace HotelJJ.Business.Services.Facturacion;

public class FacturacionOrchestrationService : IFacturacionOrchestrationService
{
    private readonly IFacturacionDataService _facturacionDataService;
    private readonly IIdentifierResolverService _identifierResolverService;

    public FacturacionOrchestrationService(
        IFacturacionDataService facturacionDataService,
        IIdentifierResolverService identifierResolverService)
    {
        _facturacionDataService = facturacionDataService;
        _identifierResolverService = identifierResolverService;
    }

    public Task<FacturaDTO> GetFacturaAsync(
        Guid facturaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        FacturacionValidator.ValidateGuid(facturaGuid, "facturaGuid");

        return ExecuteFacturacionOperationAsync(async () =>
        {
            var factura = await _facturacionDataService.GetByGuidAsync(
                facturaGuid,
                authorizationHeader,
                cancellationToken);

            return FacturacionBusinessMapper.ToDTO(factura);
        });
    }

    public Task<FacturaDTO> GenerarFacturaReservaAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        FacturacionValidator.ValidateGuid(reservaGuid, "reservaGuid");

        return ExecuteFacturacionOperationAsync(async () =>
        {
            var idReserva = await _identifierResolverService.ResolveReservaIdAsync(
                reservaGuid,
                authorizationHeader,
                cancellationToken);

            var factura = await _facturacionDataService.GenerarFacturaReservaAsync(
                idReserva,
                authorizationHeader,
                cancellationToken);

            return FacturacionBusinessMapper.ToDTO(factura);
        });
    }

    public Task<FacturaDTO> GenerarFacturaFinalAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        FacturacionValidator.ValidateGuid(reservaGuid, "reservaGuid");

        return ExecuteFacturacionOperationAsync(async () =>
        {
            var idReserva = await _identifierResolverService.ResolveReservaIdAsync(
                reservaGuid,
                authorizationHeader,
                cancellationToken);

            var factura = await _facturacionDataService.GenerarFacturaFinalAsync(
                idReserva,
                authorizationHeader,
                cancellationToken);

            return FacturacionBusinessMapper.ToDTO(factura);
        });
    }

    public Task<PagoSimuladoDTO> SimularPagoAsync(
        PagoSimularDTO request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        FacturacionValidator.ValidateSimulacion(request);

        return ExecuteFacturacionOperationAsync(async () =>
        {
            var idReserva = await _identifierResolverService.ResolveReservaIdAsync(
                request.ReservaGuid,
                authorizationHeader,
                cancellationToken);

            var pago = await _facturacionDataService.SimularPagoAsync(
                FacturacionBusinessMapper.ToDataRequest(request, idReserva),
                authorizationHeader,
                cancellationToken);

            return FacturacionBusinessMapper.ToDTO(pago);
        });
    }

    public Task<PagoDTO> RegistrarPagoAsync(
        PagoCreateDTO request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        FacturacionValidator.ValidatePago(request);

        return ExecuteFacturacionOperationAsync(async () =>
        {
            var factura = await _facturacionDataService.GetByGuidAsync(
                request.FacturaGuid,
                authorizationHeader,
                cancellationToken);

            var pago = await _facturacionDataService.RegistrarPagoAsync(
                FacturacionBusinessMapper.ToDataRequest(request, factura.IdFactura, factura.IdReserva),
                authorizationHeader,
                cancellationToken);

            return FacturacionBusinessMapper.ToDTO(pago);
        });
    }

    public Task<FacturaSaldoDTO> GetSaldoAsync(
        Guid facturaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        FacturacionValidator.ValidateGuid(facturaGuid, "facturaGuid");

        return ExecuteFacturacionOperationAsync(async () =>
        {
            var saldo = await _facturacionDataService.GetSaldoAsync(
                facturaGuid,
                authorizationHeader,
                cancellationToken);

            return FacturacionBusinessMapper.ToDTO(saldo);
        });
    }

    private static async Task<T> ExecuteFacturacionOperationAsync<T>(Func<Task<T>> operation)
    {
        try
        {
            return await operation();
        }
        catch (DownstreamApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new IntegrationNotFoundException("MID-FAC-404", "No se encontro el recurso solicitado en Facturacion.", ex);
        }
        catch (DownstreamApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized ||
                                                ex.StatusCode == HttpStatusCode.Forbidden)
        {
            throw new IntegrationUnauthorizedException("MID-FAC-401", "Token ausente o no autorizado para operar Facturacion.", ex);
        }
        catch (DownstreamApiException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
        {
            throw new IntegrationConflictException("MID-FAC-409", "Facturacion rechazo la operacion por conflicto de estado.", ex);
        }
        catch (DownstreamApiException ex) when (ex.StatusCode == HttpStatusCode.BadRequest ||
                                                ex.StatusCode == HttpStatusCode.UnprocessableEntity)
        {
            throw new IntegrationValidationException("MID-FAC-400", ex.Message);
        }
        catch (DownstreamApiException ex)
        {
            throw new IntegrationBusinessException("MID-FAC-502", ex.Message, ex);
        }
        catch (HttpRequestException ex)
        {
            throw new IntegrationBusinessException("MID-FAC-503", "No fue posible conectar con el microservicio Facturacion.", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new IntegrationBusinessException("MID-FAC-504", "La solicitud a Facturacion excedio el tiempo de espera.", ex);
        }
    }
}

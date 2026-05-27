using HotelJJ.Business.DTOs.Facturacion;
using HotelJJ.Business.DTOs.Flujos;
using HotelJJ.Business.DTOs.Hospedaje;
using HotelJJ.Business.Exceptions;
using HotelJJ.Business.Interfaces.Facturacion;
using HotelJJ.Business.Interfaces.Flujos;
using HotelJJ.Business.Interfaces.Hospedaje;
using HotelJJ.Business.Interfaces.Reservas;
using HotelJJ.Business.Validators.Facturacion;
using HotelJJ.Business.Validators.Hospedaje;
using HotelJJ.Business.Validators.Reservas;

namespace HotelJJ.Business.Services.Flujos;

public class IntegratedFlowOrchestrationService : IIntegratedFlowOrchestrationService
{
    private readonly IReservationOrchestrationService _reservationOrchestrationService;
    private readonly IFacturacionOrchestrationService _facturacionOrchestrationService;
    private readonly IHospedajeOrchestrationService _hospedajeOrchestrationService;

    public IntegratedFlowOrchestrationService(
        IReservationOrchestrationService reservationOrchestrationService,
        IFacturacionOrchestrationService facturacionOrchestrationService,
        IHospedajeOrchestrationService hospedajeOrchestrationService)
    {
        _reservationOrchestrationService = reservationOrchestrationService;
        _facturacionOrchestrationService = facturacionOrchestrationService;
        _hospedajeOrchestrationService = hospedajeOrchestrationService;
    }

    public async Task<IntegratedBookingResultDTO> CreateBookingAsync(
        IntegratedBookingCreateDTO request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new IntegrationValidationException("MID-FLU-001", "La solicitud de reserva integrada es requerida.");
        }

        ReservationValidator.ValidateCreate(request.Reserva);

        var reserva = await _reservationOrchestrationService.CreateAsync(request.Reserva, cancellationToken);
        FacturaDTO? factura = null;
        PagoDTO? pago = null;
        PagoSimuladoDTO? pagoSimulado = null;

        if (request.GenerarFacturaInicial || request.PagoInicial is not null)
        {
            factura = await _facturacionOrchestrationService.GenerarFacturaReservaAsync(
                reserva.ReservaGuid,
                authorizationHeader,
                cancellationToken);
        }

        if (request.PagoInicial is not null)
        {
            var paymentResult = await ExecutePaymentAsync(
                reserva.ReservaGuid,
                request.PagoInicial,
                factura,
                authorizationHeader,
                cancellationToken);

            factura = paymentResult.Factura ?? factura;
            pago = paymentResult.Pago;
            pagoSimulado = paymentResult.PagoSimulado;
        }

        return new IntegratedBookingResultDTO
        {
            Reserva = reserva,
            FacturaInicial = factura,
            Pago = pago,
            PagoSimulado = pagoSimulado
        };
    }

    public async Task<IntegratedBookingResultDTO> PayBookingAsync(
        Guid reservaGuid,
        IntegratedPaymentDTO request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        ReservationValidator.ValidateGet(reservaGuid);
        ValidatePayment(request);

        var reserva = await _reservationOrchestrationService.GetByGuidAsync(reservaGuid, cancellationToken);
        var paymentResult = await ExecutePaymentAsync(
            reservaGuid,
            request,
            existingFactura: null,
            authorizationHeader,
            cancellationToken);

        return new IntegratedBookingResultDTO
        {
            Reserva = reserva,
            FacturaInicial = paymentResult.Factura,
            Pago = paymentResult.Pago,
            PagoSimulado = paymentResult.PagoSimulado
        };
    }

    public async Task<IntegratedCheckInResultDTO> CheckInAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        ReservationValidator.ValidateGet(reservaGuid);

        var reserva = await _reservationOrchestrationService.GetByGuidAsync(reservaGuid, cancellationToken);
        var estadia = await _hospedajeOrchestrationService.CheckInAsync(
            reservaGuid,
            authorizationHeader,
            cancellationToken);

        return new IntegratedCheckInResultDTO
        {
            Reserva = reserva,
            Estadia = estadia
        };
    }

    public async Task<IntegratedCheckOutResultDTO> CheckOutAsync(
        Guid estadiaGuid,
        IntegratedCheckOutDTO request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        HospedajeValidator.ValidateGuid(estadiaGuid, "estadiaGuid");
        if (request is null)
        {
            throw new IntegrationValidationException("MID-FLU-002", "La solicitud de check-out integrado es requerida.");
        }

        if (request.GenerarFacturaFinal && (!request.ReservaGuid.HasValue || request.ReservaGuid.Value == Guid.Empty))
        {
            throw new IntegrationValidationException(
                "MID-FLU-003",
                "Debe enviar reservaGuid para generar la factura final desde el check-out integrado.");
        }

        await _hospedajeOrchestrationService.CheckOutAsync(
            estadiaGuid,
            new CheckOutHospedajeDTO
            {
                Observaciones = request.Observaciones,
                RequiereMantenimiento = request.RequiereMantenimiento
            },
            authorizationHeader,
            cancellationToken);

        var estadia = await _hospedajeOrchestrationService.GetByGuidAsync(
            estadiaGuid,
            authorizationHeader,
            cancellationToken);

        FacturaDTO? facturaFinal = null;
        if (request.GenerarFacturaFinal && request.ReservaGuid.HasValue)
        {
            facturaFinal = await _facturacionOrchestrationService.GenerarFacturaFinalAsync(
                request.ReservaGuid.Value,
                authorizationHeader,
                cancellationToken);
        }

        return new IntegratedCheckOutResultDTO
        {
            Estadia = estadia,
            FacturaFinal = facturaFinal
        };
    }

    private async Task<PaymentExecutionResult> ExecutePaymentAsync(
        Guid reservaGuid,
        IntegratedPaymentDTO request,
        FacturaDTO? existingFactura,
        string? authorizationHeader,
        CancellationToken cancellationToken)
    {
        ValidatePayment(request);

        if (request.SimularPago)
        {
            var facturaSimulada = existingFactura ?? await _facturacionOrchestrationService.GenerarFacturaReservaAsync(
                reservaGuid,
                authorizationHeader,
                cancellationToken);

            var pagoSimuladoRegistrado = await _facturacionOrchestrationService.RegistrarPagoAsync(
                new PagoCreateDTO
                {
                    FacturaGuid = facturaSimulada.FacturaGuid,
                    Monto = request.Monto,
                    MetodoPago = string.IsNullOrWhiteSpace(request.MetodoPago) ? "TARJETA" : request.MetodoPago,
                    EsPagoElectronico = true,
                    ProveedorPasarela = string.IsNullOrWhiteSpace(request.ProveedorPasarela) ? "SIMULADOR" : request.ProveedorPasarela,
                    TransaccionExterna = request.TransaccionExterna ?? BuildTransactionReference(reservaGuid, request),
                    CodigoAutorizacion = request.CodigoAutorizacion ?? BuildAuthorizationCode(reservaGuid, request),
                    Referencia = request.Referencia,
                    Moneda = request.Moneda,
                    TipoCambio = request.TipoCambio
                },
                authorizationHeader,
                cancellationToken);

            var pagoSimulado = new PagoSimuladoDTO
            {
                CodigoReserva = reservaGuid.ToString(),
                Monto = pagoSimuladoRegistrado.Monto,
                EstadoPago = pagoSimuladoRegistrado.EstadoPago,
                EstadoReserva = "PAG",
                TransaccionExterna = pagoSimuladoRegistrado.TransaccionExterna ?? string.Empty,
                CodigoAutorizacion = pagoSimuladoRegistrado.CodigoAutorizacion ?? string.Empty,
                Mensaje = "Pago aprobado y registrado contra la factura de la reserva.",
                FechaPagoUtc = pagoSimuladoRegistrado.FechaPagoUtc
            };

            return new PaymentExecutionResult(facturaSimulada, pagoSimuladoRegistrado, pagoSimulado);
        }

        var factura = existingFactura;
        if (request.FacturaGuid.HasValue && request.FacturaGuid.Value != Guid.Empty)
        {
            factura = await _facturacionOrchestrationService.GetFacturaAsync(
                request.FacturaGuid.Value,
                authorizationHeader,
                cancellationToken);
        }
        else if (factura is null)
        {
            factura = await _facturacionOrchestrationService.GenerarFacturaReservaAsync(
                reservaGuid,
                authorizationHeader,
                cancellationToken);
        }

        var pago = await _facturacionOrchestrationService.RegistrarPagoAsync(
            new PagoCreateDTO
            {
                FacturaGuid = factura.FacturaGuid,
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
            authorizationHeader,
            cancellationToken);

        return new PaymentExecutionResult(factura, pago, null);
    }

    private static void ValidatePayment(IntegratedPaymentDTO? request)
    {
        if (request is null)
        {
            throw new IntegrationValidationException("MID-FLU-004", "La solicitud de pago integrada es requerida.");
        }

        if (request.SimularPago)
        {
            FacturacionValidator.ValidateSimulacion(
                new PagoSimularDTO
                {
                    ReservaGuid = Guid.NewGuid(),
                    Monto = request.Monto,
                    TokenPago = request.TokenPago ?? string.Empty,
                    Referencia = request.Referencia
                });

            return;
        }

        FacturacionValidator.ValidatePago(
            new PagoCreateDTO
            {
                FacturaGuid = request.FacturaGuid.GetValueOrDefault(Guid.NewGuid()),
                Monto = request.Monto,
                MetodoPago = request.MetodoPago,
                EsPagoElectronico = request.EsPagoElectronico,
                ProveedorPasarela = request.ProveedorPasarela,
                TransaccionExterna = request.TransaccionExterna,
                CodigoAutorizacion = request.CodigoAutorizacion,
                Referencia = request.Referencia,
                Moneda = request.Moneda,
                TipoCambio = request.TipoCambio
        });
    }

    private static string BuildTransactionReference(Guid reservaGuid, IntegratedPaymentDTO request)
    {
        var seed = string.IsNullOrWhiteSpace(request.Referencia)
            ? request.TokenPago ?? reservaGuid.ToString("N")
            : request.Referencia;

        return $"SIM-{reservaGuid:N}-{Math.Abs(seed.GetHashCode()):X}";
    }

    private static string BuildAuthorizationCode(Guid reservaGuid, IntegratedPaymentDTO request)
    {
        var transactionReference = BuildTransactionReference(reservaGuid, request);
        return transactionReference.Length <= 24
            ? transactionReference
            : transactionReference[^24..];
    }

    private sealed record PaymentExecutionResult(
        FacturaDTO? Factura,
        PagoDTO? Pago,
        PagoSimuladoDTO? PagoSimulado);
}

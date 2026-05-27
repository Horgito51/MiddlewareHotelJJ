using Asp.Versioning;
using HotelJJ.API.Models.Requests.Flujos;
using HotelJJ.Business.DTOs.Flujos;
using HotelJJ.Business.DTOs.Facturacion;
using HotelJJ.Business.DTOs.Reservas;
using HotelJJ.Business.Interfaces.Flujos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelJJ.API.Controllers.V1.Flujos;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/internal/flujos")]
[Authorize]
public class IntegratedFlowController : ControllerBase
{
    private readonly IIntegratedFlowOrchestrationService _integratedFlowOrchestrationService;

    public IntegratedFlowController(IIntegratedFlowOrchestrationService integratedFlowOrchestrationService)
    {
        _integratedFlowOrchestrationService = integratedFlowOrchestrationService;
    }

    [HttpPost("booking/reservas")]
    public async Task<ActionResult<IntegratedBookingResultDTO>> CreateBooking(
        [FromBody] IntegratedBookingCreateRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _integratedFlowOrchestrationService.CreateBookingAsync(
            ToDTO(request),
            Request.Headers.Authorization.ToString(),
            cancellationToken);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPost("booking/reservas/{reservaGuid:guid}/pagar")]
    public async Task<ActionResult<IntegratedBookingResultDTO>> PayBooking(
        Guid reservaGuid,
        [FromBody] IntegratedPaymentRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _integratedFlowOrchestrationService.PayBookingAsync(
            reservaGuid,
            ToDTO(request),
            Request.Headers.Authorization.ToString(),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("operacion/check-in/{reservaGuid:guid}")]
    public async Task<ActionResult<IntegratedCheckInResultDTO>> CheckIn(
        Guid reservaGuid,
        CancellationToken cancellationToken)
    {
        var result = await _integratedFlowOrchestrationService.CheckInAsync(
            reservaGuid,
            Request.Headers.Authorization.ToString(),
            cancellationToken);

        return Ok(result);
    }

    [HttpPatch("operacion/check-out/{estadiaGuid:guid}")]
    public async Task<ActionResult<IntegratedCheckOutResultDTO>> CheckOut(
        Guid estadiaGuid,
        [FromBody] IntegratedCheckOutRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _integratedFlowOrchestrationService.CheckOutAsync(
            estadiaGuid,
            new IntegratedCheckOutDTO
            {
                Observaciones = request.Observaciones,
                RequiereMantenimiento = request.RequiereMantenimiento,
                GenerarFacturaFinal = request.GenerarFacturaFinal,
                ReservaGuid = request.ReservaGuid
            },
            Request.Headers.Authorization.ToString(),
            cancellationToken);

        return Ok(result);
    }

    private static IntegratedBookingCreateDTO ToDTO(IntegratedBookingCreateRequest request)
    {
        return new IntegratedBookingCreateDTO
        {
            GenerarFacturaInicial = request.GenerarFacturaInicial,
            PagoInicial = request.PagoInicial is null ? null : ToDTO(request.PagoInicial),
            Reserva = new ReservationCreateDTO
            {
                SucursalGuid = request.Reserva.SucursalGuid,
                FechaInicio = request.Reserva.FechaInicio,
                FechaFin = request.Reserva.FechaFin,
                DescuentoAplicado = 0m,
                Observaciones = request.Reserva.Observaciones,
                EsWalkin = request.Reserva.EsWalkin,
                OrigenCanalReserva = request.Reserva.OrigenCanalReserva,
                Cliente = request.Reserva.Cliente is null
                    ? null
                    : new ReservationClientDTO
                    {
                        TipoIdentificacion = request.Reserva.Cliente.TipoIdentificacion,
                        NumeroIdentificacion = request.Reserva.Cliente.NumeroIdentificacion,
                        Nombres = request.Reserva.Cliente.Nombres,
                        Apellidos = request.Reserva.Cliente.Apellidos,
                        Correo = request.Reserva.Cliente.Correo,
                        Telefono = request.Reserva.Cliente.Telefono,
                        Direccion = request.Reserva.Cliente.Direccion
                },
                Habitaciones = request.Reserva.Habitaciones.Select(h => new ReservationRoomCreateDTO
                {
                    TipoHabitacionGuid = h.TipoHabitacionGuid,
                    NumHabitaciones = h.NumHabitaciones,
                    NumAdultos = h.NumAdultos,
                    NumNinos = h.NumNinos,
                    DescuentoLinea = 0m
                }).ToList()
            }
        };
    }

    private static IntegratedPaymentDTO ToDTO(IntegratedPaymentRequest request)
    {
        return new IntegratedPaymentDTO
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
            TipoCambio = request.TipoCambio,
            SimularPago = request.SimularPago,
            TokenPago = request.TokenPago
        };
    }
}

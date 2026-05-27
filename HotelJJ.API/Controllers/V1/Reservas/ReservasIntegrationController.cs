using Asp.Versioning;
using HotelJJ.API.Models.Requests.Reservas;
using HotelJJ.API.Models.Responses.Reservas;
using HotelJJ.Business.DTOs.Reservas;
using HotelJJ.Business.Exceptions;
using HotelJJ.Business.Interfaces.Reservas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelJJ.API.Controllers.V1.Reservas;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
[Route("api/v{version:apiVersion}/public/reservas")]
public class ReservasIntegrationController : ControllerBase
{
    private readonly IReservationOrchestrationService _reservationOrchestrationService;

    public ReservasIntegrationController(IReservationOrchestrationService reservationOrchestrationService)
    {
        _reservationOrchestrationService = reservationOrchestrationService;
    }

    [HttpPost]
    [HttpPost("/api/v{version:apiVersion}/accommodations/reservas")]
    [AllowAnonymous]
    public async Task<ActionResult<ReservationResponse>> Create(
        [FromBody] CreateReservationRequest request,
        CancellationToken cancellationToken)
    {
        if (request.SucursalGuid == Guid.Empty)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new { Message = "SucursalGuid es obligatorio." });
        } 
        var result = await _reservationOrchestrationService.CreateAsync(ToDTO(request), cancellationToken);
        return StatusCode(StatusCodes.Status201Created, ToResponse(result));
    }

    [HttpGet("{reservaGuid}")]
    [HttpGet("/api/v{version:apiVersion}/accommodations/reservas/{reservaGuid}")]
    [ProducesResponseType(typeof(ReservationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservationResponse>> GetByGuid(
        string reservaGuid,
        CancellationToken cancellationToken)
    {
        var parsedReservaGuid = ParseRequiredGuid(reservaGuid, "reservaGuid");

        var result = await _reservationOrchestrationService.GetByGuidAuthorizedAsync(
            parsedReservaGuid,
            Request.Headers.Authorization.ToString(),
            cancellationToken);

        return Ok(ToResponse(result));
    }

    [HttpPost("calcular-precio")]
    [AllowAnonymous]
    public async Task<ActionResult<ReservationPriceDTO>> CalculatePrice(
        [FromBody] ReservationPriceRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _reservationOrchestrationService.CalculatePriceAsync(
            new ReservationPriceRequestDTO
            {
                HabitacionGuid = request.HabitacionGuid,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                Canal = request.Canal
            },
            cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{reservaGuid:guid}/cancelar")]
    [AllowAnonymous]
    public async Task<IActionResult> Cancel(
        Guid reservaGuid,
        [FromBody] PublicCancelarReservaRequest request,
        CancellationToken cancellationToken)
    {
        await _reservationOrchestrationService.CancelAsync(
            reservaGuid,
            new CancelReservationDTO { Motivo = request.Motivo },
            cancellationToken);

        return NoContent();
    }

    private static ReservationCreateDTO ToDTO(CreateReservationRequest request)
    {
        return new ReservationCreateDTO
        {
            SucursalGuid = request.SucursalGuid,
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin,
            DescuentoAplicado = 0m,
            Observaciones = request.Observaciones,
            EsWalkin = request.EsWalkin,
            OrigenCanalReserva = request.OrigenCanalReserva,
            Cliente = request.Cliente is null
                ? null
                : new ReservationClientDTO
                {
                    TipoIdentificacion = request.Cliente.TipoIdentificacion,
                    NumeroIdentificacion = request.Cliente.NumeroIdentificacion,
                    Nombres = request.Cliente.Nombres,
                    Apellidos = request.Cliente.Apellidos,
                    Correo = request.Cliente.Correo,
                    Telefono = request.Cliente.Telefono,
                    Direccion = request.Cliente.Direccion
                },
            Habitaciones = request.Habitaciones.Select(h => new ReservationRoomCreateDTO
            {
                TipoHabitacionGuid = h.TipoHabitacionGuid,
                NumHabitaciones = h.NumHabitaciones,
                NumAdultos = h.NumAdultos,
                NumNinos = h.NumNinos,
                DescuentoLinea = 0m
            }).ToList()
        };
    }

    private static ReservationResponse ToResponse(ReservationDTO dto)
    {
        return new ReservationResponse
        {
            ReservaGuid = dto.ReservaGuid,
            CodigoReserva = dto.CodigoReserva,
            ClienteGuid = dto.ClienteGuid,
            SucursalGuid = dto.SucursalGuid,
            FechaReservaUtc = dto.FechaReservaUtc,
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            SubtotalReserva = dto.SubtotalReserva,
            ValorIva = dto.ValorIva,
            TotalReserva = dto.TotalReserva,
            SaldoPendiente = dto.SaldoPendiente,
            OrigenCanalReserva = dto.OrigenCanalReserva,
            EstadoReserva = dto.EstadoReserva,
            FechaConfirmacionUtc = dto.FechaConfirmacionUtc,
            FechaCancelacionUtc = dto.FechaCancelacionUtc,
            MotivoCancelacion = dto.MotivoCancelacion,
            Observaciones = dto.Observaciones,
            Habitaciones = dto.Habitaciones.Select(ToResponse).ToList()
        };
    }

    private static ReservationRoomResponse ToResponse(ReservationRoomDTO dto)
    {
        return new ReservationRoomResponse
        {
            ReservaHabitacionGuid = dto.ReservaHabitacionGuid,
            HabitacionGuid = dto.HabitacionGuid,
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            NumAdultos = dto.NumAdultos,
            NumNinos = dto.NumNinos,
            PrecioNocheAplicado = dto.PrecioNocheAplicado,
            SubtotalLinea = dto.SubtotalLinea,
            ValorIvaLinea = dto.ValorIvaLinea,
            TotalLinea = dto.TotalLinea,
            EstadoDetalle = dto.EstadoDetalle
        };
    }

    private static Guid ParseRequiredGuid(string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value) || !Guid.TryParse(value, out var guid) || guid == Guid.Empty)
            throw new IntegrationValidationException("MID-RES-400", $"{fieldName} es obligatorio y debe tener formato UUID valido.");

        return guid;
    }
}

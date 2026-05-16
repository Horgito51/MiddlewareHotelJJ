using Asp.Versioning;
using HotelJJ.API.Models.Requests.Reservas;
using HotelJJ.Business.DTOs.Reservas;
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
    public async Task<ActionResult<ReservationDTO>> Create(
        [FromBody] CreateReservationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _reservationOrchestrationService.CreateAsync(ToDTO(request), cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet("{reservaGuid:guid}")]
    [HttpGet("/api/v{version:apiVersion}/accommodations/reservas/{reservaGuid:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ReservationDTO>> GetByGuid(
        Guid reservaGuid,
        CancellationToken cancellationToken)
    {
        var result = await _reservationOrchestrationService.GetByGuidAsync(reservaGuid, cancellationToken);
        return Ok(result);
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
            ClienteGuid = request.ClienteGuid,
            SucursalGuid = request.SucursalGuid,
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin,
            DescuentoAplicado = request.DescuentoAplicado,
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
                HabitacionGuid = h.HabitacionGuid,
                TipoHabitacionGuid = h.TipoHabitacionGuid,
                NumHabitaciones = h.NumHabitaciones,
                FechaInicio = h.FechaInicio,
                FechaFin = h.FechaFin,
                NumAdultos = h.NumAdultos,
                NumNinos = h.NumNinos,
                DescuentoLinea = h.DescuentoLinea
            }).ToList()
        };
    }
}

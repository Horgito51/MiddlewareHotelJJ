using Asp.Versioning;
using HotelJJ.API.Models.Requests.Alojamiento;
using HotelJJ.Business.DTOs.Alojamiento;
using HotelJJ.Business.Interfaces.Alojamiento;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelJJ.API.Controllers.V1.Alojamiento;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
[Route("api/v{version:apiVersion}/accommodations")]
public class AlojamientoIntegrationController : ControllerBase
{
    private readonly IAlojamientoOrchestrationService _alojamientoOrchestrationService;

    public AlojamientoIntegrationController(IAlojamientoOrchestrationService alojamientoOrchestrationService)
    {
        _alojamientoOrchestrationService = alojamientoOrchestrationService;
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<ActionResult<PagedAlojamientoDTO<AlojamientoSearchItemDTO>>> Search(
        [FromQuery] AlojamientoSearchRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _alojamientoOrchestrationService.SearchAsync(new AlojamientoSearchDTO
        {
            Destino = request.Destino,
            FechaEntrada = request.FechaEntrada,
            FechaSalida = request.FechaSalida,
            NumAdultos = request.NumAdultos,
            NumNinos = request.NumNinos,
            NumHabitaciones = request.NumHabitaciones,
            TipoAlojamiento = request.TipoAlojamiento,
            PrecioMin = request.PrecioMin,
            PrecioMax = request.PrecioMax,
            CategoriaViaje = request.CategoriaViaje,
            OrdenarPor = request.OrdenarPor,
            Pagina = request.Pagina,
            Limite = request.Limite
        }, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{sucursalGuid:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<AlojamientoDetailDTO>> GetDetail(
        Guid sucursalGuid,
        [FromQuery] DateTime? fechaEntrada,
        [FromQuery] DateTime? fechaSalida,
        CancellationToken cancellationToken)
    {
        var result = await _alojamientoOrchestrationService.GetDetailAsync(
            sucursalGuid,
            new AlojamientoDetailQueryDTO { FechaEntrada = fechaEntrada, FechaSalida = fechaSalida },
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{sucursalGuid:guid}/reviews")]
    [AllowAnonymous]
    public async Task<ActionResult<PagedAlojamientoDTO<AlojamientoReviewDTO>>> GetReviews(
        Guid sucursalGuid,
        [FromQuery] int pagina = 1,
        [FromQuery] int limite = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _alojamientoOrchestrationService.GetReviewsAsync(
            sucursalGuid,
            new AlojamientoReviewsQueryDTO { Pagina = pagina, Limite = limite },
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("sucursales/{sucursalGuid:guid}/habitaciones")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<AlojamientoHabitacionDTO>>> GetHabitaciones(
        Guid sucursalGuid,
        [FromQuery] Guid? tipoHabitacionGuid,
        [FromQuery] DateTime? fechaInicio,
        [FromQuery] DateTime? fechaFin,
        CancellationToken cancellationToken)
    {
        var result = await _alojamientoOrchestrationService.GetHabitacionesAsync(
            sucursalGuid,
            new AlojamientoHabitacionesQueryDTO
            {
                TipoHabitacionGuid = tipoHabitacionGuid,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin
            },
            cancellationToken);

        return Ok(result);
    }
}

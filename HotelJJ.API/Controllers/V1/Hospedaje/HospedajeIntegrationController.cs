using Asp.Versioning;
using HotelJJ.API.Models.Requests.Hospedaje;
using HotelJJ.Business.DTOs.Hospedaje;
using HotelJJ.Business.Interfaces.Hospedaje;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelJJ.API.Controllers.V1.Hospedaje;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/internal/hospedaje")]
[Authorize]
public class HospedajeIntegrationController : ControllerBase
{
    private readonly IHospedajeOrchestrationService _hospedajeOrchestrationService;

    public HospedajeIntegrationController(IHospedajeOrchestrationService hospedajeOrchestrationService)
    {
        _hospedajeOrchestrationService = hospedajeOrchestrationService;
    }

    [HttpPost("check-in/{reservaGuid:guid}")]
    public async Task<ActionResult<EstadiaDTO>> CheckIn(Guid reservaGuid, CancellationToken cancellationToken)
    {
        var result = await _hospedajeOrchestrationService.CheckInAsync(
            reservaGuid,
            Request.Headers.Authorization.ToString(),
            cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{estadiaGuid:guid}/check-out")]
    public async Task<IActionResult> CheckOut(
        Guid estadiaGuid,
        [FromBody] CheckOutHospedajeRequest request,
        CancellationToken cancellationToken)
    {
        await _hospedajeOrchestrationService.CheckOutAsync(
            estadiaGuid,
            new CheckOutHospedajeDTO
            {
                Observaciones = request.Observaciones,
                RequiereMantenimiento = request.RequiereMantenimiento
            },
            Request.Headers.Authorization.ToString(),
            cancellationToken);

        return NoContent();
    }

    [HttpGet("estadias/{estadiaGuid:guid}")]
    public async Task<ActionResult<EstadiaDTO>> GetByGuid(Guid estadiaGuid, CancellationToken cancellationToken)
    {
        var result = await _hospedajeOrchestrationService.GetByGuidAsync(
            estadiaGuid,
            Request.Headers.Authorization.ToString(),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("{estadiaGuid:guid}/cargos")]
    public async Task<ActionResult<CargoEstadiaDTO>> AddCargo(
        Guid estadiaGuid,
        [FromBody] CargoHospedajeCreateRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _hospedajeOrchestrationService.AddCargoAsync(
            estadiaGuid,
            new CargoHospedajeCreateDTO
            {
                IdCatalogo = request.IdCatalogo,
                DescripcionCargo = request.DescripcionCargo,
                Cantidad = request.Cantidad,
                PrecioUnitario = request.PrecioUnitario,
                ValorIva = request.ValorIva
            },
            Request.Headers.Authorization.ToString(),
            cancellationToken);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPatch("cargos/{cargoGuid:guid}/anular")]
    public async Task<IActionResult> AnularCargo(Guid cargoGuid, CancellationToken cancellationToken)
    {
        await _hospedajeOrchestrationService.AnularCargoAsync(
            cargoGuid,
            Request.Headers.Authorization.ToString(),
            cancellationToken);

        return NoContent();
    }
}

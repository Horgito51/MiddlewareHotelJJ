using Asp.Versioning;
using HotelJJ.API.Models.Requests.Alojamiento;
using HotelJJ.Business.DTOs.Alojamiento;
using HotelJJ.Business.Exceptions;
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
        ValidateQueryParameters(SearchQueryParameters);

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

    [HttpGet("{sucursalGuid}")]
    [AllowAnonymous]
    public async Task<ActionResult<AlojamientoDetailDTO>> GetDetail(
        string sucursalGuid,
        [FromQuery] DateTime? fechaInicio,
        [FromQuery] DateTime? fechaFin,
        CancellationToken cancellationToken)
    {
        ValidateQueryParameters(DetailQueryParameters);
        var parsedSucursalGuid = ParseGuid(sucursalGuid, "sucursalGuid");

        var result = await _alojamientoOrchestrationService.GetDetailAsync(
            parsedSucursalGuid,
            new AlojamientoDetailQueryDTO { FechaEntrada = fechaInicio, FechaSalida = fechaFin },
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{sucursalGuid}/reviews")]
    [AllowAnonymous]
    public async Task<ActionResult<PagedAlojamientoDTO<AlojamientoReviewDTO>>> GetReviews(
        string sucursalGuid,
        [FromQuery] int pagina = 1,
        [FromQuery] int limite = 10,
        CancellationToken cancellationToken = default)
    {
        ValidateQueryParameters(ReviewsQueryParameters);
        var parsedSucursalGuid = ParseGuid(sucursalGuid, "sucursalGuid");

        var result = await _alojamientoOrchestrationService.GetReviewsAsync(
            parsedSucursalGuid,
            new AlojamientoReviewsQueryDTO { Pagina = pagina, Limite = limite },
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("/api/v{version:apiVersion}/alojamiento/sucursales/{sucursalGuid}/valoraciones")]
    [AllowAnonymous]
    public async Task<ActionResult<PagedAlojamientoDTO<AlojamientoReviewDTO>>> GetValoracionesBySucursal(
        string sucursalGuid,
        [FromQuery] int pagina = 1,
        [FromQuery] int limite = 10,
        CancellationToken cancellationToken = default)
    {
        ValidateQueryParameters(ReviewsQueryParameters);
        var parsedSucursalGuid = ParseGuid(sucursalGuid, "sucursalGuid");

        var result = await _alojamientoOrchestrationService.GetReviewsAsync(
            parsedSucursalGuid,
            new AlojamientoReviewsQueryDTO { Pagina = pagina, Limite = limite },
            cancellationToken);

        return Ok(result);
    }

    private static readonly HashSet<string> SearchQueryParameters = new(StringComparer.OrdinalIgnoreCase)
    {
        "Destino", "fechaInicio", "fechaFin", "NumAdultos", "NumNinos", "NumHabitaciones",
        "TipoAlojamiento", "PrecioMin", "PrecioMax", "CategoriaViaje", "OrdenarPor", "Pagina",
        "Limite", "api-version"
    };

    private static readonly HashSet<string> DetailQueryParameters = new(StringComparer.OrdinalIgnoreCase)
    {
        "fechaInicio", "fechaFin"
    };

    private static readonly HashSet<string> ReviewsQueryParameters = new(StringComparer.OrdinalIgnoreCase)
    {
        "pagina", "limite", "api-version"
    };

    private void ValidateQueryParameters(IReadOnlySet<string> allowedParameters)
    {
        foreach (var key in Request.Query.Keys)
        {
            if (!allowedParameters.Contains(key))
            {
                throw new IntegrationValidationException(
                    "MID-ALOJ-QUERY-001",
                    $"El parametro '{key}' no esta soportado por este endpoint.");
            }
        }
    }

    private static Guid ParseGuid(string value, string parameterName)
    {
        if (!Guid.TryParse(value, out var guid) || guid == Guid.Empty)
        {
            throw new IntegrationValidationException(
                "MID-ALOJ-GUID-001",
                $"{parameterName} debe ser un UUID valido.");
        }

        return guid;
    }
}

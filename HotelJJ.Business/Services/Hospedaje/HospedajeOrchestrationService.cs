using System.Net;
using HotelJJ.Business.DTOs.Hospedaje;
using HotelJJ.Business.Exceptions;
using HotelJJ.Business.Interfaces.Hospedaje;
using HotelJJ.Business.Mappers.Hospedaje;
using HotelJJ.Business.Validators.Hospedaje;
using HotelJJ.DataAccess.Http.Common;
using HotelJJ.DataManagement.Common.Identifiers;
using HotelJJ.DataManagement.Hospedaje.Interfaces;

namespace HotelJJ.Business.Services.Hospedaje;

public class HospedajeOrchestrationService : IHospedajeOrchestrationService
{
    private readonly IHospedajeDataService _hospedajeDataService;
    private readonly IIdentifierResolverService _identifierResolverService;

    public HospedajeOrchestrationService(
        IHospedajeDataService hospedajeDataService,
        IIdentifierResolverService identifierResolverService)
    {
        _hospedajeDataService = hospedajeDataService;
        _identifierResolverService = identifierResolverService;
    }

    public Task<EstadiaDTO> CheckInAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        HospedajeValidator.ValidateGuid(reservaGuid, "reservaGuid");

        return ExecuteHospedajeOperationAsync(async () =>
        {
            var idReservaHabitacion = await _identifierResolverService.ResolveReservaHabitacionIdAsync(
                reservaGuid,
                authorizationHeader,
                cancellationToken);

            var estadia = await _hospedajeDataService.CheckInAsync(
                idReservaHabitacion,
                authorizationHeader,
                cancellationToken);

            return HospedajeBusinessMapper.ToDTO(estadia);
        });
    }

    public Task<EstadiaDTO> GetByGuidAsync(
        Guid estadiaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        HospedajeValidator.ValidateGuid(estadiaGuid, "estadiaGuid");

        return ExecuteHospedajeOperationAsync(async () =>
        {
            var estadia = await _hospedajeDataService.GetByGuidAsync(
                estadiaGuid,
                authorizationHeader,
                cancellationToken);

            return HospedajeBusinessMapper.ToDTO(estadia);
        });
    }

    public Task CheckOutAsync(
        Guid estadiaGuid,
        CheckOutHospedajeDTO request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        HospedajeValidator.ValidateGuid(estadiaGuid, "estadiaGuid");

        return ExecuteHospedajeOperationAsync(async () =>
        {
            var idEstadia = await _identifierResolverService.ResolveEstadiaIdAsync(
                estadiaGuid,
                authorizationHeader,
                cancellationToken);

            await _hospedajeDataService.CheckOutAsync(
                idEstadia,
                HospedajeBusinessMapper.ToDataRequest(request),
                authorizationHeader,
                cancellationToken);

            return true;
        });
    }

    public Task<CargoEstadiaDTO> AddCargoAsync(
        Guid estadiaGuid,
        CargoHospedajeCreateDTO request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        HospedajeValidator.ValidateGuid(estadiaGuid, "estadiaGuid");
        HospedajeValidator.ValidateCargo(request);

        return ExecuteHospedajeOperationAsync(async () =>
        {
            var idEstadia = await _identifierResolverService.ResolveEstadiaIdAsync(
                estadiaGuid,
                authorizationHeader,
                cancellationToken);

            var cargo = await _hospedajeDataService.AddCargoAsync(
                idEstadia,
                HospedajeBusinessMapper.ToDataRequest(request),
                authorizationHeader,
                cancellationToken);

            return HospedajeBusinessMapper.ToDTO(cargo);
        });
    }

    public Task AnularCargoAsync(
        Guid cargoGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        HospedajeValidator.ValidateGuid(cargoGuid, "cargoGuid");

        return ExecuteHospedajeOperationAsync(async () =>
        {
            var idCargo = await _identifierResolverService.ResolveCargoIdAsync(
                cargoGuid,
                authorizationHeader,
                cancellationToken);

            await _hospedajeDataService.AnularCargoAsync(
                idCargo,
                authorizationHeader,
                cancellationToken);

            return true;
        });
    }

    private static async Task<T> ExecuteHospedajeOperationAsync<T>(Func<Task<T>> operation)
    {
        try
        {
            return await operation();
        }
        catch (DownstreamApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new IntegrationNotFoundException("MID-HOS-404", "No se encontro el recurso solicitado en Hospedaje.", ex);
        }
        catch (DownstreamApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized ||
                                                ex.StatusCode == HttpStatusCode.Forbidden)
        {
            throw new IntegrationUnauthorizedException("MID-HOS-401", "Token ausente o no autorizado para operar Hospedaje.", ex);
        }
        catch (DownstreamApiException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
        {
            throw new IntegrationConflictException("MID-HOS-409", "Hospedaje rechazo la operacion por conflicto de estado.", ex);
        }
        catch (DownstreamApiException ex) when (ex.StatusCode == HttpStatusCode.BadRequest ||
                                                ex.StatusCode == HttpStatusCode.UnprocessableEntity)
        {
            throw new IntegrationValidationException("MID-HOS-400", ex.Message);
        }
        catch (DownstreamApiException ex)
        {
            throw new IntegrationBusinessException("MID-HOS-502", ex.Message, ex);
        }
        catch (HttpRequestException ex)
        {
            throw new IntegrationBusinessException("MID-HOS-503", "No fue posible conectar con el microservicio Hospedaje.", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new IntegrationBusinessException("MID-HOS-504", "La solicitud a Hospedaje excedio el tiempo de espera.", ex);
        }
    }
}

using System.Net;
using HotelJJ.Business.DTOs.Alojamiento;
using HotelJJ.Business.Exceptions;
using HotelJJ.Business.Interfaces.Alojamiento;
using HotelJJ.Business.Mappers.Alojamiento;
using HotelJJ.Business.Validators.Alojamiento;
using HotelJJ.DataAccess.Http.Common;
using HotelJJ.DataManagement.Alojamiento.Interfaces;

namespace HotelJJ.Business.Services.Alojamiento;

public class AlojamientoOrchestrationService : IAlojamientoOrchestrationService
{
    private readonly IAlojamientoDataService _alojamientoDataService;

    public AlojamientoOrchestrationService(IAlojamientoDataService alojamientoDataService)
    {
        _alojamientoDataService = alojamientoDataService;
    }

    public Task<PagedAlojamientoDTO<AlojamientoSearchItemDTO>> SearchAsync(
        AlojamientoSearchDTO request,
        CancellationToken cancellationToken = default)
    {
        AlojamientoQueryValidator.ValidateSearch(request);

        return ExecuteAlojamientoOperationAsync(async () =>
        {
            var data = await _alojamientoDataService.SearchAsync(
                AlojamientoBusinessMapper.ToDataRequest(request),
                cancellationToken);

            return AlojamientoBusinessMapper.ToPagedDTO(data, AlojamientoBusinessMapper.ToDTO);
        });
    }

    public Task<AlojamientoDetailDTO> GetDetailAsync(
        Guid sucursalGuid,
        AlojamientoDetailQueryDTO request,
        CancellationToken cancellationToken = default)
    {
        AlojamientoQueryValidator.ValidateDetail(sucursalGuid, request);

        return ExecuteAlojamientoOperationAsync(async () =>
        {
            var data = await _alojamientoDataService.GetDetailAsync(
                sucursalGuid,
                AlojamientoBusinessMapper.ToDataRequest(request),
                cancellationToken);

            return AlojamientoBusinessMapper.ToDTO(data);
        });
    }

    public Task<PagedAlojamientoDTO<AlojamientoReviewDTO>> GetReviewsAsync(
        Guid sucursalGuid,
        AlojamientoReviewsQueryDTO request,
        CancellationToken cancellationToken = default)
    {
        AlojamientoQueryValidator.ValidateReviews(sucursalGuid, request);

        return ExecuteAlojamientoOperationAsync(async () =>
        {
            var data = await _alojamientoDataService.GetReviewsAsync(
                sucursalGuid,
                AlojamientoBusinessMapper.ToDataRequest(request),
                cancellationToken);

            return AlojamientoBusinessMapper.ToPagedDTO(data, AlojamientoBusinessMapper.ToDTO);
        });
    }

    public Task<IReadOnlyList<AlojamientoHabitacionDTO>> GetHabitacionesAsync(
        Guid sucursalGuid,
        AlojamientoHabitacionesQueryDTO request,
        CancellationToken cancellationToken = default)
    {
        AlojamientoQueryValidator.ValidateHabitaciones(sucursalGuid, request);

        return ExecuteAlojamientoOperationAsync(async () =>
        {
            var data = await _alojamientoDataService.GetHabitacionesAsync(
                sucursalGuid,
                AlojamientoBusinessMapper.ToDataRequest(request),
                cancellationToken);

            return (IReadOnlyList<AlojamientoHabitacionDTO>)data.Select(AlojamientoBusinessMapper.ToDTO).ToList();
        });
    }

    private static async Task<T> ExecuteAlojamientoOperationAsync<T>(Func<Task<T>> operation)
    {
        try
        {
            return await operation();
        }
        catch (DownstreamApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new IntegrationNotFoundException("MID-ALOJ-404", "No se encontro el recurso solicitado en Alojamiento.", ex);
        }
        catch (DownstreamApiException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            throw new IntegrationValidationException("MID-ALOJ-400", ex.Message);
        }
        catch (DownstreamApiException ex)
        {
            throw new IntegrationBusinessException("MID-ALOJ-502", ex.Message, ex);
        }
        catch (HttpRequestException ex)
        {
            throw new IntegrationBusinessException("MID-ALOJ-503", "No fue posible conectar con el microservicio Alojamiento.", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new IntegrationBusinessException("MID-ALOJ-504", "La solicitud al microservicio Alojamiento excedio el tiempo de espera.", ex);
        }
    }
}

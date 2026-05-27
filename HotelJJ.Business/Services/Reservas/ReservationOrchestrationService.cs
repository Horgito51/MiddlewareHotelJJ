using System.Net;
using HotelJJ.Business.DTOs.Reservas;
using HotelJJ.Business.Exceptions;
using HotelJJ.Business.Interfaces.Reservas;
using HotelJJ.Business.Mappers.Reservas;
using HotelJJ.Business.Validators.Reservas;
using HotelJJ.DataAccess.Http.Common;
using HotelJJ.DataManagement.Reservas.Interfaces;

namespace HotelJJ.Business.Services.Reservas;

public class ReservationOrchestrationService : IReservationOrchestrationService
{
    private readonly IReservasDataService _reservasDataService;

    public ReservationOrchestrationService(
        IReservasDataService reservasDataService)
    {
        _reservasDataService = reservasDataService;
    }

    public Task<ReservationDTO> CreateAsync(
        ReservationCreateDTO request,
        CancellationToken cancellationToken = default)
    {
        ReservationValidator.ValidateCreate(request);

        return ExecuteReservasOperationAsync(async () =>
        {
            var data = await _reservasDataService.CreateAsync(
                ReservationBusinessMapper.ToDataRequest(request),
                cancellationToken);

            return ReservationBusinessMapper.ToDTO(data);
        });
    }

    public Task<ReservationDTO> GetByGuidAsync(
        Guid reservaGuid,
        CancellationToken cancellationToken = default)
    {
        ReservationValidator.ValidateGet(reservaGuid);

        return ExecuteReservasOperationAsync(async () =>
        {
            var data = await _reservasDataService.GetByGuidAsync(reservaGuid, cancellationToken);
            return ReservationBusinessMapper.ToDTO(data);
        });
    }

    public Task<ReservationDTO> GetByGuidAuthorizedAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        ReservationValidator.ValidateGet(reservaGuid);

        return ExecuteReservasOperationAsync(async () =>
        {
            var data = await _reservasDataService.GetByGuidAuthorizedAsync(
                reservaGuid,
                authorizationHeader,
                cancellationToken);

            return ReservationBusinessMapper.ToDTO(data);
        });
    }

    public Task<ReservationPriceDTO> CalculatePriceAsync(
        ReservationPriceRequestDTO request,
        CancellationToken cancellationToken = default)
    {
        ReservationValidator.ValidatePrice(request);

        return ExecuteReservasOperationAsync(async () =>
        {
            var data = await _reservasDataService.CalcularPrecioAsync(
                ReservationBusinessMapper.ToDataRequest(request),
                cancellationToken);

            return ReservationBusinessMapper.ToDTO(data);
        });
    }

    public Task CancelAsync(
        Guid reservaGuid,
        CancelReservationDTO request,
        CancellationToken cancellationToken = default)
    {
        ReservationValidator.ValidateCancel(reservaGuid);

        return ExecuteReservasOperationAsync(async () =>
        {
            await _reservasDataService.CancelarAsync(
                reservaGuid,
                ReservationBusinessMapper.ToDataRequest(request),
                cancellationToken);

            return true;
        });
    }

    private static async Task<T> ExecuteReservasOperationAsync<T>(Func<Task<T>> operation)
    {
        try
        {
            return await operation();
        }
        catch (DownstreamApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new IntegrationNotFoundException("MID-RES-404", ex.Message, ex);
        }
        catch (DownstreamApiException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
        {
            throw new IntegrationConflictException("MID-RES-409", ex.Message, ex);
        }
        catch (DownstreamApiException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
        {
            throw new IntegrationForbiddenException("MID-RES-403", ex.Message, ex);
        }
        catch (DownstreamApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new IntegrationUnauthorizedException("MID-RES-401", ex.Message, ex);
        }
        catch (DownstreamApiException ex) when (ex.StatusCode == HttpStatusCode.BadRequest ||
                                                ex.StatusCode == HttpStatusCode.UnprocessableEntity)
        {
            throw new IntegrationValidationException("MID-RES-400", ex.Message, ex);
        }
        catch (DownstreamApiException ex)
        {
            throw new IntegrationBusinessException("MID-RES-502", ex.Message, ex);
        }
        catch (HttpRequestException ex)
        {
            throw new IntegrationBusinessException("MID-RES-503", "No fue posible conectar con los microservicios de la reserva.", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new IntegrationBusinessException("MID-RES-504", "La solicitud de reserva excedio el tiempo de espera.", ex);
        }
    }
}

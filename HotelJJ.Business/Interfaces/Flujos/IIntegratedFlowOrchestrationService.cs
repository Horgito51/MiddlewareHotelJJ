using HotelJJ.Business.DTOs.Flujos;

namespace HotelJJ.Business.Interfaces.Flujos;

public interface IIntegratedFlowOrchestrationService
{
    Task<IntegratedBookingResultDTO> CreateBookingAsync(
        IntegratedBookingCreateDTO request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<IntegratedBookingResultDTO> PayBookingAsync(
        Guid reservaGuid,
        IntegratedPaymentDTO request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<IntegratedCheckInResultDTO> CheckInAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<IntegratedCheckOutResultDTO> CheckOutAsync(
        Guid estadiaGuid,
        IntegratedCheckOutDTO request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);
}

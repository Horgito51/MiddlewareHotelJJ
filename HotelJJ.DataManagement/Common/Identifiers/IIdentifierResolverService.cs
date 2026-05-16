namespace HotelJJ.DataManagement.Common.Identifiers;

public interface IIdentifierResolverService
{
    Task<int> ResolveReservaIdAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<int> ResolveReservaHabitacionIdAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<int> ResolveEstadiaIdAsync(
        Guid estadiaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<int> ResolveCargoIdAsync(
        Guid cargoGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<int> ResolveFacturaIdAsync(
        Guid facturaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);
}

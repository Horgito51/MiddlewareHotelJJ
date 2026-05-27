using HotelJJ.Business.DTOs.Alojamiento;
using HotelJJ.Business.Exceptions;

namespace HotelJJ.Business.Validators.Alojamiento;

public static class AlojamientoQueryValidator
{
    public static void ValidateSearch(AlojamientoSearchDTO request)
    {
        if (request is null)
        {
            throw new IntegrationValidationException("MID-ALOJ-001", "La solicitud de busqueda no puede ser nula.");
        }

        ValidateDatePair(request.FechaEntrada, request.FechaSalida, "MID-ALOJ-002", "MID-ALOJ-003");
        ValidateNonNegative(request.NumAdultos, "MID-ALOJ-004", "El numero de adultos no puede ser negativo.");
        ValidateNonNegative(request.NumNinos, "MID-ALOJ-005", "El numero de ninos no puede ser negativo.");
        ValidateNonNegative(request.NumHabitaciones, "MID-ALOJ-006", "El numero de habitaciones no puede ser negativo.");

        if (request.PrecioMin.HasValue && request.PrecioMin.Value < 0)
        {
            throw new IntegrationValidationException("MID-ALOJ-007", "El precio minimo no puede ser negativo.");
        }

        if (request.PrecioMax.HasValue && request.PrecioMax.Value < 0)
        {
            throw new IntegrationValidationException("MID-ALOJ-008", "El precio maximo no puede ser negativo.");
        }

        if (request.PrecioMin.HasValue && request.PrecioMax.HasValue && request.PrecioMax.Value < request.PrecioMin.Value)
        {
            throw new IntegrationValidationException("MID-ALOJ-009", "El precio maximo debe ser mayor o igual al precio minimo.");
        }

        request.Pagina = Math.Max(1, request.Pagina);
        request.Limite = Math.Clamp(request.Limite, 1, 50);
    }

    public static void ValidateDetail(Guid sucursalGuid, AlojamientoDetailQueryDTO request)
    {
        ValidateSucursalGuid(sucursalGuid);
        ValidateDatePair(request.FechaEntrada, request.FechaSalida, "MID-ALOJ-011", "MID-ALOJ-012");
    }

    public static void ValidateReviews(Guid sucursalGuid, AlojamientoReviewsQueryDTO request)
    {
        ValidateSucursalGuid(sucursalGuid);
        request.Pagina = Math.Max(1, request.Pagina);
        request.Limite = Math.Clamp(request.Limite, 1, 50);
    }

    public static void ValidateHabitaciones(Guid sucursalGuid, AlojamientoHabitacionesQueryDTO request)
    {
        ValidateSucursalGuid(sucursalGuid);

        if (request.TipoHabitacionGuid == Guid.Empty)
        {
            throw new IntegrationValidationException("MID-ALOJ-015", "tipoHabitacionGuid debe ser un UUID valido cuando se envia.");
        }

        ValidateDatePair(request.FechaInicio, request.FechaFin, "MID-ALOJ-013", "MID-ALOJ-014");
    }

    private static void ValidateSucursalGuid(Guid sucursalGuid)
    {
        if (sucursalGuid == Guid.Empty)
        {
            throw new IntegrationValidationException("MID-ALOJ-010", "El identificador de sucursal es requerido.");
        }
    }

    private static void ValidateDatePair(DateTime? start, DateTime? end, string incompleteCode, string rangeCode)
    {
        if (start.HasValue != end.HasValue)
        {
            throw new IntegrationValidationException(incompleteCode, "fechaInicio y fechaFin deben enviarse juntas.");
        }

        if (start.HasValue && end.HasValue && end.Value <= start.Value)
        {
            throw new IntegrationValidationException(rangeCode, "fechaFin debe ser posterior a fechaInicio.");
        }
    }

    private static void ValidateNonNegative(int? value, string code, string message)
    {
        if (value.HasValue && value.Value < 0)
        {
            throw new IntegrationValidationException(code, message);
        }
    }
}

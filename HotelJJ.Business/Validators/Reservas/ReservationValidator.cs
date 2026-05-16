using HotelJJ.Business.DTOs.Reservas;
using HotelJJ.Business.Exceptions;

namespace HotelJJ.Business.Validators.Reservas;

public static class ReservationValidator
{
    public static void ValidateCreate(ReservationCreateDTO request)
    {
        if (request.SucursalGuid == Guid.Empty)
        {
            throw new IntegrationValidationException("MID-RES-VAL-001", "sucursalGuid es obligatorio.");
        }

        ValidateDateRange(request.FechaInicio, request.FechaFin);

        if (request.ClienteGuid == Guid.Empty && request.Cliente is null)
        {
            throw new IntegrationValidationException("MID-RES-VAL-002", "Debe enviar clienteGuid o los datos del cliente.");
        }

        if (request.Cliente is not null)
        {
            ValidateClient(request.Cliente);
        }

        if (request.Habitaciones.Count == 0)
        {
            throw new IntegrationValidationException("MID-RES-VAL-003", "Debe enviar al menos una habitacion.");
        }

        foreach (var habitacion in request.Habitaciones)
        {
            ValidateRoom(habitacion, request.FechaInicio, request.FechaFin);
        }
    }

    public static void ValidateGet(Guid reservaGuid)
    {
        if (reservaGuid == Guid.Empty)
        {
            throw new IntegrationValidationException("MID-RES-VAL-010", "reservaGuid es obligatorio.");
        }
    }

    public static void ValidateCancel(Guid reservaGuid)
    {
        if (reservaGuid == Guid.Empty)
        {
            throw new IntegrationValidationException("MID-RES-VAL-011", "reservaGuid es obligatorio.");
        }
    }

    public static void ValidatePrice(ReservationPriceRequestDTO request)
    {
        if (request.HabitacionGuid == Guid.Empty)
        {
            throw new IntegrationValidationException("MID-RES-VAL-020", "habitacionGuid es obligatorio.");
        }

        ValidateDateRange(request.FechaInicio, request.FechaFin);
    }

    private static void ValidateClient(ReservationClientDTO cliente)
    {
        if (string.IsNullOrWhiteSpace(cliente.TipoIdentificacion) ||
            string.IsNullOrWhiteSpace(cliente.NumeroIdentificacion) ||
            string.IsNullOrWhiteSpace(cliente.Nombres) ||
            string.IsNullOrWhiteSpace(cliente.Correo) ||
            string.IsNullOrWhiteSpace(cliente.Telefono))
        {
            throw new IntegrationValidationException(
                "MID-RES-VAL-004",
                "tipoIdentificacion, numeroIdentificacion, nombres, correo y telefono son obligatorios.");
        }
    }

    private static void ValidateRoom(
        ReservationRoomCreateDTO habitacion,
        DateTime reservaInicio,
        DateTime reservaFin)
    {
        if (habitacion.HabitacionGuid == Guid.Empty && habitacion.TipoHabitacionGuid == Guid.Empty)
        {
            throw new IntegrationValidationException(
                "MID-RES-VAL-005",
                "Cada habitacion debe tener habitacionGuid o tipoHabitacionGuid.");
        }

        if (habitacion.NumHabitaciones <= 0)
        {
            throw new IntegrationValidationException("MID-RES-VAL-006", "numHabitaciones debe ser mayor a cero.");
        }

        if (habitacion.NumAdultos <= 0)
        {
            throw new IntegrationValidationException("MID-RES-VAL-007", "numAdultos debe ser mayor a cero.");
        }

        if (habitacion.NumNinos < 0)
        {
            throw new IntegrationValidationException("MID-RES-VAL-008", "numNinos no puede ser negativo.");
        }

        ValidateDateRange(
            habitacion.FechaInicio ?? reservaInicio,
            habitacion.FechaFin ?? reservaFin);
    }

    private static void ValidateDateRange(DateTime fechaInicio, DateTime fechaFin)
    {
        if (fechaInicio == default || fechaFin == default)
        {
            throw new IntegrationValidationException("MID-RES-VAL-009", "fechaInicio y fechaFin son obligatorias.");
        }

        if (fechaFin <= fechaInicio)
        {
            throw new IntegrationValidationException("MID-RES-VAL-012", "fechaFin debe ser posterior a fechaInicio.");
        }
    }
}

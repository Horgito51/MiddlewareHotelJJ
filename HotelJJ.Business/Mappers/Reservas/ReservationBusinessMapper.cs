using HotelJJ.Business.DTOs.Reservas;
using HotelJJ.DataManagement.Reservas.Models;

namespace HotelJJ.Business.Mappers.Reservas;

public static class ReservationBusinessMapper
{
    public static ReservaCreateDataRequest ToDataRequest(ReservationCreateDTO request)
    {
        return new ReservaCreateDataRequest
        {
            SucursalGuid = request.SucursalGuid,
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin,
            DescuentoAplicado = request.DescuentoAplicado,
            Observaciones = request.Observaciones,
            EsWalkin = request.EsWalkin,
            OrigenCanalReserva = string.IsNullOrWhiteSpace(request.OrigenCanalReserva)
                ? "MARKETPLACE"
                : request.OrigenCanalReserva,
            Cliente = request.Cliente is null ? null : ToDataRequest(request.Cliente),
            Habitaciones = request.Habitaciones.Select(ToDataRequest).ToList()
        };
    }

    public static ReservaPrecioDataRequest ToDataRequest(ReservationPriceRequestDTO request)
    {
        return new ReservaPrecioDataRequest
        {
            HabitacionGuid = request.HabitacionGuid,
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin,
            Canal = request.Canal
        };
    }

    public static CancelarReservaDataRequest ToDataRequest(CancelReservationDTO request)
    {
        return new CancelarReservaDataRequest
        {
            Motivo = request.Motivo
        };
    }

    public static ReservationDTO ToDTO(ReservaDataModel data)
    {
        return new ReservationDTO
        {
            ReservaGuid = data.ReservaGuid,
            CodigoReserva = data.CodigoReserva,
            ClienteGuid = data.ClienteGuid,
            SucursalGuid = data.SucursalGuid,
            FechaReservaUtc = data.FechaReservaUtc,
            FechaInicio = data.FechaInicio,
            FechaFin = data.FechaFin,
            SubtotalReserva = data.SubtotalReserva,
            ValorIva = data.ValorIva,
            TotalReserva = data.TotalReserva,
            DescuentoAplicado = data.DescuentoAplicado,
            SaldoPendiente = data.SaldoPendiente,
            OrigenCanalReserva = data.OrigenCanalReserva,
            EstadoReserva = data.EstadoReserva,
            FechaConfirmacionUtc = data.FechaConfirmacionUtc,
            FechaCancelacionUtc = data.FechaCancelacionUtc,
            MotivoCancelacion = data.MotivoCancelacion,
            Observaciones = data.Observaciones,
            EsWalkin = data.EsWalkin,
            Habitaciones = data.Habitaciones.Select(ToDTO).ToList()
        };
    }

    public static ReservationPriceDTO ToDTO(ReservaPrecioDataModel data)
    {
        return new ReservationPriceDTO
        {
            HabitacionGuid = data.HabitacionGuid,
            PrecioNocheAplicado = data.PrecioNocheAplicado,
            SubtotalLinea = data.SubtotalLinea,
            ValorIvaLinea = data.ValorIvaLinea,
            TotalLinea = data.TotalLinea,
            OrigenPrecio = data.OrigenPrecio
        };
    }

    private static ReservaClienteDataRequest ToDataRequest(ReservationClientDTO request)
    {
        return new ReservaClienteDataRequest
        {
            TipoIdentificacion = request.TipoIdentificacion,
            NumeroIdentificacion = request.NumeroIdentificacion,
            Nombres = request.Nombres,
            Apellidos = request.Apellidos,
            Correo = request.Correo,
            Telefono = request.Telefono,
            Direccion = request.Direccion
        };
    }

    private static ReservaHabitacionDataRequest ToDataRequest(ReservationRoomCreateDTO request)
    {
        return new ReservaHabitacionDataRequest
        {
            TipoHabitacionGuid = request.TipoHabitacionGuid,
            NumHabitaciones = request.NumHabitaciones,
            NumAdultos = request.NumAdultos,
            NumNinos = request.NumNinos,
            DescuentoLinea = request.DescuentoLinea
        };
    }

    private static ReservationRoomDTO ToDTO(ReservaHabitacionDataModel data)
    {
        return new ReservationRoomDTO
        {
            ReservaHabitacionGuid = data.ReservaHabitacionGuid,
            HabitacionGuid = data.HabitacionGuid,
            FechaInicio = data.FechaInicio,
            FechaFin = data.FechaFin,
            NumAdultos = data.NumAdultos,
            NumNinos = data.NumNinos,
            PrecioNocheAplicado = data.PrecioNocheAplicado,
            SubtotalLinea = data.SubtotalLinea,
            ValorIvaLinea = data.ValorIvaLinea,
            DescuentoLinea = data.DescuentoLinea,
            TotalLinea = data.TotalLinea,
            EstadoDetalle = data.EstadoDetalle
        };
    }
}

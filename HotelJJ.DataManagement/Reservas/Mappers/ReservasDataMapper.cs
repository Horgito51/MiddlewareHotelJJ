using HotelJJ.DataAccess.Http.Models.Reservas.Requests;
using HotelJJ.DataAccess.Http.Models.Reservas.Responses;
using HotelJJ.DataManagement.Reservas.Models;

namespace HotelJJ.DataManagement.Reservas.Mappers;

public static class ReservasDataMapper
{
    public static CreateReservaRequestModel ToHttpRequest(ReservaCreateDataRequest request)
    {
        return new CreateReservaRequestModel
        {
            ClienteGuid = request.ClienteGuid,
            SucursalGuid = request.SucursalGuid,
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin,
            DescuentoAplicado = request.DescuentoAplicado,
            Observaciones = request.Observaciones,
            EsWalkin = request.EsWalkin,
            OrigenCanalReserva = request.OrigenCanalReserva,
            Cliente = request.Cliente is null ? null : ToHttpRequest(request.Cliente),
            Habitaciones = request.Habitaciones.Select(ToHttpRequest).ToList()
        };
    }

    public static ReservaPrecioRequestModel ToHttpRequest(ReservaPrecioDataRequest request)
    {
        return new ReservaPrecioRequestModel
        {
            HabitacionGuid = request.HabitacionGuid,
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin,
            Canal = request.Canal
        };
    }

    public static CancelarReservaRequestModel ToHttpRequest(CancelarReservaDataRequest request)
    {
        return new CancelarReservaRequestModel
        {
            Motivo = request.Motivo
        };
    }

    public static ReservaDataModel ToDataModel(ReservaResponseModel response)
    {
        return new ReservaDataModel
        {
            ReservaGuid = response.ReservaGuid,
            CodigoReserva = response.CodigoReserva,
            ClienteGuid = response.ClienteGuid,
            SucursalGuid = response.SucursalGuid,
            FechaReservaUtc = response.FechaReservaUtc,
            FechaInicio = response.FechaInicio,
            FechaFin = response.FechaFin,
            SubtotalReserva = response.SubtotalReserva,
            ValorIva = response.ValorIva,
            TotalReserva = response.TotalReserva,
            DescuentoAplicado = response.DescuentoAplicado,
            SaldoPendiente = response.SaldoPendiente,
            OrigenCanalReserva = response.OrigenCanalReserva,
            EstadoReserva = response.EstadoReserva,
            FechaConfirmacionUtc = response.FechaConfirmacionUtc,
            FechaCancelacionUtc = response.FechaCancelacionUtc,
            MotivoCancelacion = response.MotivoCancelacion,
            Observaciones = response.Observaciones,
            EsWalkin = response.EsWalkin,
            Habitaciones = response.Habitaciones.Select(ToDataModel).ToList()
        };
    }

    public static ReservaPrecioDataModel ToDataModel(ReservaPrecioResponseModel response)
    {
        return new ReservaPrecioDataModel
        {
            HabitacionGuid = response.HabitacionGuid,
            PrecioNocheAplicado = response.PrecioNocheAplicado,
            SubtotalLinea = response.SubtotalLinea,
            ValorIvaLinea = response.ValorIvaLinea,
            TotalLinea = response.TotalLinea,
            OrigenPrecio = response.OrigenPrecio
        };
    }

    private static ReservaClienteRequestModel ToHttpRequest(ReservaClienteDataRequest request)
    {
        return new ReservaClienteRequestModel
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

    private static ReservaHabitacionRequestModel ToHttpRequest(ReservaHabitacionDataRequest request)
    {
        return new ReservaHabitacionRequestModel
        {
            HabitacionGuid = request.HabitacionGuid,
            TipoHabitacionGuid = request.TipoHabitacionGuid,
            NumHabitaciones = request.NumHabitaciones,
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin,
            NumAdultos = request.NumAdultos,
            NumNinos = request.NumNinos,
            DescuentoLinea = request.DescuentoLinea
        };
    }

    private static ReservaHabitacionDataModel ToDataModel(ReservaHabitacionResponseModel response)
    {
        return new ReservaHabitacionDataModel
        {
            ReservaHabitacionGuid = response.ReservaHabitacionGuid,
            HabitacionGuid = response.HabitacionGuid,
            FechaInicio = response.FechaInicio,
            FechaFin = response.FechaFin,
            NumAdultos = response.NumAdultos,
            NumNinos = response.NumNinos,
            PrecioNocheAplicado = response.PrecioNocheAplicado,
            SubtotalLinea = response.SubtotalLinea,
            ValorIvaLinea = response.ValorIvaLinea,
            DescuentoLinea = response.DescuentoLinea,
            TotalLinea = response.TotalLinea,
            EstadoDetalle = response.EstadoDetalle
        };
    }
}

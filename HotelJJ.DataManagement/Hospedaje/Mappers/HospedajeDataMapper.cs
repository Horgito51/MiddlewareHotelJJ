using HotelJJ.DataAccess.Http.Models.Hospedaje.Requests;
using HotelJJ.DataAccess.Http.Models.Hospedaje.Responses;
using HotelJJ.DataAccess.Http.Models.Reservas.Responses;
using HotelJJ.DataManagement.Hospedaje.Models;

namespace HotelJJ.DataManagement.Hospedaje.Mappers;

public static class HospedajeDataMapper
{
    public static CheckOutHospedajeRequestModel ToHttpRequest(CheckOutHospedajeDataRequest request)
    {
        return new CheckOutHospedajeRequestModel
        {
            Observaciones = request.Observaciones,
            RequiereMantenimiento = request.RequiereMantenimiento
        };
    }

    public static CargoHospedajeRequestModel ToHttpRequest(CargoHospedajeDataRequest request)
    {
        return new CargoHospedajeRequestModel
        {
            IdCatalogo = request.IdCatalogo,
            DescripcionCargo = request.DescripcionCargo,
            Cantidad = request.Cantidad,
            PrecioUnitario = request.PrecioUnitario,
            ValorIva = request.ValorIva
        };
    }

    public static EstadiaDataModel ToDataModel(EstadiaHospedajeResponseModel response)
    {
        return new EstadiaDataModel
        {
            IdEstadia = response.IdEstadia,
            EstadiaGuid = response.EstadiaGuid,
            IdReservaHabitacion = response.IdReservaHabitacion,
            IdCliente = response.IdCliente,
            IdHabitacion = response.IdHabitacion,
            CheckinUtc = response.CheckinUtc,
            CheckoutUtc = response.CheckoutUtc,
            EstadoEstadia = response.EstadoEstadia,
            ObservacionesCheckin = response.ObservacionesCheckin,
            ObservacionesCheckout = response.ObservacionesCheckout,
            RequiereMantenimiento = response.RequiereMantenimiento,
            FechaRegistroUtc = response.FechaRegistroUtc,
            Cargos = response.Cargos?.Select(ToDataModel).ToList() ?? new List<CargoEstadiaDataModel>()
        };
    }

    public static CargoEstadiaDataModel ToDataModel(CargoHospedajeResponseModel response)
    {
        return new CargoEstadiaDataModel
        {
            IdCargoEstadia = response.IdCargoEstadia,
            CargoGuid = response.CargoGuid,
            IdEstadia = response.IdEstadia,
            IdCatalogo = response.IdCatalogo,
            DescripcionCargo = response.DescripcionCargo,
            Cantidad = response.Cantidad,
            PrecioUnitario = response.PrecioUnitario,
            Subtotal = response.Subtotal,
            ValorIva = response.ValorIva,
            TotalCargo = response.TotalCargo,
            FechaConsumoUtc = response.FechaConsumoUtc,
            EstadoCargo = response.EstadoCargo,
            FechaRegistroUtc = response.FechaRegistroUtc
        };
    }

    public static ReservaHospedajeDataModel ToDataModel(InternalReservaResponseModel response)
    {
        return new ReservaHospedajeDataModel
        {
            IdReserva = response.IdReserva,
            ReservaGuid = response.GuidReserva,
            EstadoReserva = response.EstadoReserva,
            Habitaciones = response.Habitaciones.Select(ToDataModel).ToList()
        };
    }

    private static ReservaHabitacionHospedajeDataModel ToDataModel(InternalReservaHabitacionResponseModel response)
    {
        return new ReservaHabitacionHospedajeDataModel
        {
            IdReservaHabitacion = response.IdReservaHabitacion,
            ReservaHabitacionGuid = response.ReservaHabitacionGuid,
            EstadoDetalle = response.EstadoDetalle
        };
    }
}

using HotelJJ.Business.DTOs.Hospedaje;
using HotelJJ.DataManagement.Hospedaje.Models;

namespace HotelJJ.Business.Mappers.Hospedaje;

public static class HospedajeBusinessMapper
{
    public static EstadiaDTO ToDTO(EstadiaDataModel data)
    {
        return new EstadiaDTO
        {
            EstadiaGuid = data.EstadiaGuid,
            CheckinUtc = data.CheckinUtc,
            CheckoutUtc = data.CheckoutUtc,
            EstadoEstadia = data.EstadoEstadia,
            ObservacionesCheckin = data.ObservacionesCheckin,
            ObservacionesCheckout = data.ObservacionesCheckout,
            RequiereMantenimiento = data.RequiereMantenimiento,
            FechaRegistroUtc = data.FechaRegistroUtc,
            Cargos = data.Cargos.Select(ToDTO).ToList()
        };
    }

    public static CargoEstadiaDTO ToDTO(CargoEstadiaDataModel data)
    {
        return new CargoEstadiaDTO
        {
            CargoGuid = data.CargoGuid,
            DescripcionCargo = data.DescripcionCargo,
            Cantidad = data.Cantidad,
            PrecioUnitario = data.PrecioUnitario,
            Subtotal = data.Subtotal,
            ValorIva = data.ValorIva,
            TotalCargo = data.TotalCargo,
            FechaConsumoUtc = data.FechaConsumoUtc,
            EstadoCargo = data.EstadoCargo,
            FechaRegistroUtc = data.FechaRegistroUtc
        };
    }

    public static CheckOutHospedajeDataRequest ToDataRequest(CheckOutHospedajeDTO dto)
    {
        return new CheckOutHospedajeDataRequest
        {
            Observaciones = dto.Observaciones,
            RequiereMantenimiento = dto.RequiereMantenimiento
        };
    }

    public static CargoHospedajeDataRequest ToDataRequest(CargoHospedajeCreateDTO dto)
    {
        return new CargoHospedajeDataRequest
        {
            IdCatalogo = dto.IdCatalogo,
            DescripcionCargo = dto.DescripcionCargo,
            Cantidad = dto.Cantidad,
            PrecioUnitario = dto.PrecioUnitario,
            ValorIva = dto.ValorIva
        };
    }
}

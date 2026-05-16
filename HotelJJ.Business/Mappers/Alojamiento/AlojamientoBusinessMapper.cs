using HotelJJ.Business.DTOs.Alojamiento;
using HotelJJ.DataManagement.Alojamiento.Models;

namespace HotelJJ.Business.Mappers.Alojamiento;

public static class AlojamientoBusinessMapper
{
    public static AlojamientoSearchDataRequest ToDataRequest(AlojamientoSearchDTO request)
    {
        return new AlojamientoSearchDataRequest
        {
            Destino = request.Destino?.Trim(),
            FechaEntrada = request.FechaEntrada,
            FechaSalida = request.FechaSalida,
            NumAdultos = request.NumAdultos,
            NumNinos = request.NumNinos,
            NumHabitaciones = request.NumHabitaciones,
            TipoAlojamiento = request.TipoAlojamiento?.Trim(),
            PrecioMin = request.PrecioMin,
            PrecioMax = request.PrecioMax,
            CategoriaViaje = request.CategoriaViaje?.Trim(),
            OrdenarPor = request.OrdenarPor?.Trim(),
            Pagina = request.Pagina,
            Limite = request.Limite
        };
    }

    public static AlojamientoDetailDataRequest ToDataRequest(AlojamientoDetailQueryDTO request)
    {
        return new AlojamientoDetailDataRequest
        {
            FechaEntrada = request.FechaEntrada,
            FechaSalida = request.FechaSalida
        };
    }

    public static AlojamientoReviewsDataRequest ToDataRequest(AlojamientoReviewsQueryDTO request)
    {
        return new AlojamientoReviewsDataRequest
        {
            Pagina = request.Pagina,
            Limite = request.Limite
        };
    }

    public static AlojamientoHabitacionesDataRequest ToDataRequest(AlojamientoHabitacionesQueryDTO request)
    {
        return new AlojamientoHabitacionesDataRequest
        {
            TipoHabitacionGuid = request.TipoHabitacionGuid,
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin
        };
    }

    public static PagedAlojamientoDTO<TDto> ToPagedDTO<TData, TDto>(
        PagedAlojamientoDataModel<TData> data,
        Func<TData, TDto> map)
    {
        return new PagedAlojamientoDTO<TDto>
        {
            Items = data.Items.Select(map).ToList(),
            Pagina = data.Pagina,
            Limite = data.Limite,
            TotalResultados = data.TotalResultados,
            TotalPaginas = data.TotalPaginas,
            TieneSiguiente = data.TieneSiguiente,
            TieneAnterior = data.TieneAnterior
        };
    }

    public static AlojamientoSearchItemDTO ToDTO(AlojamientoSearchItemDataModel data)
    {
        return new AlojamientoSearchItemDTO
        {
            SucursalGuid = data.SucursalGuid,
            Nombre = data.Nombre,
            Ciudad = data.Ciudad,
            Provincia = data.Provincia,
            Pais = data.Pais,
            Direccion = data.Direccion,
            Descripcion = data.Descripcion,
            Categoria = data.Categoria,
            Estrellas = data.Estrellas,
            TipoAlojamiento = data.TipoAlojamiento,
            PrecioDesde = data.PrecioDesde,
            Moneda = data.Moneda,
            ImagenPrincipalUrl = data.ImagenPrincipalUrl,
            PromedioValoracion = data.PromedioValoracion,
            TotalValoraciones = data.TotalValoraciones,
            HabitacionesDisponibles = data.HabitacionesDisponibles,
            ServiciosDestacados = data.ServiciosDestacados,
            HoraCheckIn = data.HoraCheckIn,
            HoraCheckOut = data.HoraCheckOut,
            AceptaNinos = data.AceptaNinos,
            PermiteMascotas = data.PermiteMascotas
        };
    }

    public static AlojamientoDetailDTO ToDTO(AlojamientoDetailDataModel data)
    {
        var item = ToDTO((AlojamientoSearchItemDataModel)data);

        return new AlojamientoDetailDTO
        {
            SucursalGuid = item.SucursalGuid,
            Nombre = item.Nombre,
            Ciudad = item.Ciudad,
            Provincia = item.Provincia,
            Pais = item.Pais,
            Direccion = item.Direccion,
            Descripcion = item.Descripcion,
            Categoria = item.Categoria,
            Estrellas = item.Estrellas,
            TipoAlojamiento = item.TipoAlojamiento,
            PrecioDesde = item.PrecioDesde,
            Moneda = item.Moneda,
            ImagenPrincipalUrl = item.ImagenPrincipalUrl,
            PromedioValoracion = item.PromedioValoracion,
            TotalValoraciones = item.TotalValoraciones,
            HabitacionesDisponibles = item.HabitacionesDisponibles,
            ServiciosDestacados = item.ServiciosDestacados,
            HoraCheckIn = item.HoraCheckIn,
            HoraCheckOut = item.HoraCheckOut,
            AceptaNinos = item.AceptaNinos,
            PermiteMascotas = item.PermiteMascotas,
            DescripcionCompleta = data.DescripcionCompleta,
            TiposHabitacion = data.TiposHabitacion.Select(ToDTO).ToList(),
            TarifasActivas = data.TarifasActivas.Select(ToDTO).ToList(),
            Amenities = data.Amenities,
            Imagenes = data.Imagenes,
            Politicas = ToDTO(data.Politicas),
            Disponibilidad = data.Disponibilidad is null ? null : ToDTO(data.Disponibilidad)
        };
    }

    public static AlojamientoReviewDTO ToDTO(AlojamientoReviewDataModel data)
    {
        return new AlojamientoReviewDTO
        {
            ValoracionGuid = data.ValoracionGuid,
            Puntuacion = data.Puntuacion,
            ComentarioPositivo = data.ComentarioPositivo,
            ComentarioNegativo = data.ComentarioNegativo,
            TipoViaje = data.TipoViaje,
            Fecha = data.Fecha,
            NombreVisibleCliente = data.NombreVisibleCliente,
            RespuestaPropiedad = data.RespuestaPropiedad
        };
    }

    public static AlojamientoHabitacionDTO ToDTO(AlojamientoHabitacionDataModel data)
    {
        return new AlojamientoHabitacionDTO
        {
            HabitacionGuid = data.HabitacionGuid,
            TipoHabitacionGuid = data.TipoHabitacionGuid,
            TipoNombre = data.TipoNombre,
            NumeroHabitacion = data.NumeroHabitacion,
            Piso = data.Piso,
            CapacidadAdultos = data.CapacidadAdultos,
            CapacidadNinos = data.CapacidadNinos,
            PrecioBase = data.PrecioBase,
            Moneda = data.Moneda,
            EstadoHabitacion = data.EstadoHabitacion,
            DisponibleEnRango = data.DisponibleEnRango
        };
    }

    private static AlojamientoRoomTypeDTO ToDTO(AlojamientoRoomTypeDataModel data)
    {
        return new AlojamientoRoomTypeDTO
        {
            TipoHabitacionGuid = data.TipoHabitacionGuid,
            Nombre = data.Nombre,
            TipoCama = data.TipoCama,
            CapacidadAdultos = data.CapacidadAdultos,
            CapacidadNinos = data.CapacidadNinos,
            AreaM2 = data.AreaM2,
            PrecioBase = data.PrecioBase,
            Imagenes = data.Imagenes,
            DisponiblesEnRango = data.DisponiblesEnRango
        };
    }

    private static AlojamientoTariffDTO ToDTO(AlojamientoTariffDataModel data)
    {
        return new AlojamientoTariffDTO
        {
            TarifaGuid = data.TarifaGuid,
            Nombre = data.Nombre,
            PrecioPorNoche = data.PrecioPorNoche,
            Moneda = data.Moneda,
            FechaInicio = data.FechaInicio,
            FechaFin = data.FechaFin,
            MinNoches = data.MinNoches,
            TipoHabitacionGuid = data.TipoHabitacionGuid
        };
    }

    private static AlojamientoPolicyDTO ToDTO(AlojamientoPolicyDataModel data)
    {
        return new AlojamientoPolicyDTO
        {
            HoraCheckIn = data.HoraCheckIn,
            HoraCheckOut = data.HoraCheckOut,
            AceptaNinos = data.AceptaNinos,
            PermiteMascotas = data.PermiteMascotas,
            Politicas = data.Politicas
        };
    }

    private static AlojamientoAvailabilityDTO ToDTO(AlojamientoAvailabilityDataModel data)
    {
        return new AlojamientoAvailabilityDTO
        {
            FechaEntrada = data.FechaEntrada,
            FechaSalida = data.FechaSalida,
            PorTipoHabitacion = data.PorTipoHabitacion.Select(ToDTO).ToList()
        };
    }

    private static AlojamientoAvailabilityByRoomTypeDTO ToDTO(AlojamientoAvailabilityByRoomTypeDataModel data)
    {
        return new AlojamientoAvailabilityByRoomTypeDTO
        {
            TipoHabitacionGuid = data.TipoHabitacionGuid,
            Nombre = data.Nombre,
            Disponibles = data.Disponibles
        };
    }
}

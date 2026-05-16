using HotelJJ.DataAccess.Http.Models.Alojamiento.Requests;
using HotelJJ.DataAccess.Http.Models.Alojamiento.Responses;
using HotelJJ.DataManagement.Alojamiento.Models;

namespace HotelJJ.DataManagement.Alojamiento.Mappers;

public static class AlojamientoDataMapper
{
    public static SearchAlojamientoRequestModel ToHttpRequest(AlojamientoSearchDataRequest request)
    {
        return new SearchAlojamientoRequestModel
        {
            Destino = request.Destino,
            FechaEntrada = request.FechaEntrada,
            FechaSalida = request.FechaSalida,
            NumAdultos = request.NumAdultos,
            NumNinos = request.NumNinos,
            NumHabitaciones = request.NumHabitaciones,
            TipoAlojamiento = request.TipoAlojamiento,
            PrecioMin = request.PrecioMin,
            PrecioMax = request.PrecioMax,
            CategoriaViaje = request.CategoriaViaje,
            OrdenarPor = request.OrdenarPor,
            Pagina = request.Pagina,
            Limite = request.Limite
        };
    }

    public static AlojamientoDetailRequestModel ToHttpRequest(AlojamientoDetailDataRequest request)
    {
        return new AlojamientoDetailRequestModel
        {
            FechaEntrada = request.FechaEntrada,
            FechaSalida = request.FechaSalida
        };
    }

    public static AlojamientoReviewsRequestModel ToHttpRequest(AlojamientoReviewsDataRequest request)
    {
        return new AlojamientoReviewsRequestModel
        {
            Pagina = request.Pagina,
            Limite = request.Limite
        };
    }

    public static AlojamientoHabitacionesRequestModel ToHttpRequest(AlojamientoHabitacionesDataRequest request)
    {
        return new AlojamientoHabitacionesRequestModel
        {
            TipoHabitacionGuid = request.TipoHabitacionGuid,
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin
        };
    }

    public static PagedAlojamientoDataModel<TData> ToPagedDataModel<THttp, TData>(
        PagedAlojamientoResponseModel<THttp> response,
        Func<THttp, TData> map)
    {
        return new PagedAlojamientoDataModel<TData>
        {
            Items = response.Items.Select(map).ToList(),
            Pagina = response.Pagina,
            Limite = response.Limite,
            TotalResultados = response.TotalResultados,
            TotalPaginas = response.TotalPaginas,
            TieneSiguiente = response.TieneSiguiente,
            TieneAnterior = response.TieneAnterior
        };
    }

    public static AlojamientoSearchItemDataModel ToDataModel(AlojamientoSearchItemResponseModel response)
    {
        return new AlojamientoSearchItemDataModel
        {
            SucursalGuid = response.SucursalGuid,
            Nombre = response.Nombre,
            Ciudad = response.Ciudad,
            Provincia = response.Provincia,
            Pais = response.Pais,
            Direccion = response.Direccion,
            Descripcion = response.Descripcion,
            Categoria = response.Categoria,
            Estrellas = response.Estrellas,
            TipoAlojamiento = response.TipoAlojamiento,
            PrecioDesde = response.PrecioDesde,
            Moneda = response.Moneda,
            ImagenPrincipalUrl = response.ImagenPrincipalUrl,
            PromedioValoracion = response.PromedioValoracion,
            TotalValoraciones = response.TotalValoraciones,
            HabitacionesDisponibles = response.HabitacionesDisponibles,
            ServiciosDestacados = response.ServiciosDestacados,
            HoraCheckIn = response.HoraCheckIn,
            HoraCheckOut = response.HoraCheckOut,
            AceptaNinos = response.AceptaNinos,
            PermiteMascotas = response.PermiteMascotas
        };
    }

    public static AlojamientoDetailDataModel ToDataModel(AlojamientoDetailResponseModel response)
    {
        var item = ToDataModel((AlojamientoSearchItemResponseModel)response);

        return new AlojamientoDetailDataModel
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
            DescripcionCompleta = response.DescripcionCompleta,
            TiposHabitacion = response.TiposHabitacion.Select(ToDataModel).ToList(),
            TarifasActivas = response.TarifasActivas.Select(ToDataModel).ToList(),
            Amenities = response.Amenities,
            Imagenes = response.Imagenes,
            Politicas = ToDataModel(response.Politicas),
            Disponibilidad = response.Disponibilidad is null ? null : ToDataModel(response.Disponibilidad)
        };
    }

    public static AlojamientoReviewDataModel ToDataModel(AlojamientoReviewResponseModel response)
    {
        return new AlojamientoReviewDataModel
        {
            ValoracionGuid = response.ValoracionGuid,
            Puntuacion = response.Puntuacion,
            ComentarioPositivo = response.ComentarioPositivo,
            ComentarioNegativo = response.ComentarioNegativo,
            TipoViaje = response.TipoViaje,
            Fecha = response.Fecha,
            NombreVisibleCliente = response.NombreVisibleCliente,
            RespuestaPropiedad = response.RespuestaPropiedad
        };
    }

    public static AlojamientoHabitacionDataModel ToDataModel(AlojamientoHabitacionResponseModel response)
    {
        return new AlojamientoHabitacionDataModel
        {
            HabitacionGuid = response.HabitacionGuid,
            TipoHabitacionGuid = response.TipoHabitacionGuid,
            TipoNombre = response.TipoNombre,
            NumeroHabitacion = response.NumeroHabitacion,
            Piso = response.Piso,
            CapacidadAdultos = response.CapacidadAdultos,
            CapacidadNinos = response.CapacidadNinos,
            PrecioBase = response.PrecioBase,
            Moneda = response.Moneda,
            EstadoHabitacion = response.EstadoHabitacion,
            DisponibleEnRango = response.DisponibleEnRango
        };
    }

    private static AlojamientoRoomTypeDataModel ToDataModel(AlojamientoRoomTypeResponseModel response)
    {
        return new AlojamientoRoomTypeDataModel
        {
            TipoHabitacionGuid = response.TipoHabitacionGuid,
            Nombre = response.Nombre,
            TipoCama = response.TipoCama,
            CapacidadAdultos = response.CapacidadAdultos,
            CapacidadNinos = response.CapacidadNinos,
            AreaM2 = response.AreaM2,
            PrecioBase = response.PrecioBase,
            Imagenes = response.Imagenes,
            DisponiblesEnRango = response.DisponiblesEnRango
        };
    }

    private static AlojamientoTariffDataModel ToDataModel(AlojamientoTariffResponseModel response)
    {
        return new AlojamientoTariffDataModel
        {
            TarifaGuid = response.TarifaGuid,
            Nombre = response.Nombre,
            PrecioPorNoche = response.PrecioPorNoche,
            Moneda = response.Moneda,
            FechaInicio = response.FechaInicio,
            FechaFin = response.FechaFin,
            MinNoches = response.MinNoches,
            TipoHabitacionGuid = response.TipoHabitacionGuid
        };
    }

    private static AlojamientoPolicyDataModel ToDataModel(AlojamientoPolicyResponseModel response)
    {
        return new AlojamientoPolicyDataModel
        {
            HoraCheckIn = response.HoraCheckIn,
            HoraCheckOut = response.HoraCheckOut,
            AceptaNinos = response.AceptaNinos,
            PermiteMascotas = response.PermiteMascotas,
            Politicas = response.Politicas
        };
    }

    private static AlojamientoAvailabilityDataModel ToDataModel(AlojamientoAvailabilityResponseModel response)
    {
        return new AlojamientoAvailabilityDataModel
        {
            FechaEntrada = response.FechaEntrada,
            FechaSalida = response.FechaSalida,
            PorTipoHabitacion = response.PorTipoHabitacion.Select(ToDataModel).ToList()
        };
    }

    private static AlojamientoAvailabilityByRoomTypeDataModel ToDataModel(AlojamientoAvailabilityByRoomTypeResponseModel response)
    {
        return new AlojamientoAvailabilityByRoomTypeDataModel
        {
            TipoHabitacionGuid = response.TipoHabitacionGuid,
            Nombre = response.Nombre,
            Disponibles = response.Disponibles
        };
    }
}

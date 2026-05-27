using Alojamiento.Contracts.Grpc.V1;
using Grpc.Core;
using HotelJJ.DataAccess.Grpc.Common;
using HotelJJ.DataAccess.Grpc.Interfaces;
using HotelJJ.DataAccess.Http.Models.Alojamiento.Responses;

namespace HotelJJ.DataAccess.Grpc.Clients;

public class AlojamientoGrpcClient : IAlojamientoGrpcClient
{
    private readonly Alojamiento.Contracts.Grpc.V1.AlojamientoGrpc.AlojamientoGrpcClient _client;

    public AlojamientoGrpcClient(Alojamiento.Contracts.Grpc.V1.AlojamientoGrpc.AlojamientoGrpcClient client)
    {
        _client = client;
    }

    public async Task<AlojamientoDetailResponseModel> GetDetailAsync(
        Guid sucursalGuid,
        DateTime? fechaEntrada,
        DateTime? fechaSalida,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var sucursal = await _client.GetSucursalByGuidAsync(new GuidRequest { Guid = sucursalGuid.ToString() }, cancellationToken: cancellationToken);

            return new AlojamientoDetailResponseModel
            {
                SucursalGuid = Guid.TryParse(sucursal.SucursalGuid, out var parsed) ? parsed : Guid.Empty,
                Nombre = sucursal.NombreSucursal,
                Ciudad = sucursal.Ciudad,
                Provincia = sucursal.Provincia,
                Pais = sucursal.Pais,
                Direccion = sucursal.Direccion,
                Descripcion = sucursal.DescripcionSucursal,
                Estrellas = sucursal.Estrellas,
                TipoAlojamiento = sucursal.TipoAlojamiento,
                HoraCheckIn = sucursal.HoraCheckin,
                HoraCheckOut = sucursal.HoraCheckout,
                AceptaNinos = sucursal.AceptaNinos,
                PermiteMascotas = sucursal.PermiteMascotas
            };
        }
        catch (RpcException ex)
        {
            throw GrpcStatusMapper.ToDownstream("Alojamiento", ex);
        }
    }

    public async Task<IReadOnlyList<AlojamientoHabitacionResponseModel>> GetHabitacionesAsync(
        Guid sucursalGuid,
        Guid? tipoHabitacionGuid,
        DateTime? fechaInicio,
        DateTime? fechaFin,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var sucursal = await _client.GetSucursalByGuidAsync(new GuidRequest { Guid = sucursalGuid.ToString() }, cancellationToken: cancellationToken);
            var habitaciones = await _client.GetHabitacionesDisponiblesAsync(new HabitacionesDisponiblesRequest
            {
                IdSucursal = sucursal.IdSucursal,
                FechaInicio = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime((fechaInicio ?? DateTime.UtcNow).ToUniversalTime()),
                FechaFin = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime((fechaFin ?? DateTime.UtcNow.AddDays(1)).ToUniversalTime())
            }, cancellationToken: cancellationToken);

            var results = new List<AlojamientoHabitacionResponseModel>();
            foreach (var item in habitaciones.Items)
            {
                var tipo = await _client.GetTipoHabitacionByIdAsync(new IdRequest { Id = item.IdTipoHabitacion }, cancellationToken: cancellationToken);
                var tipoGuidParsed = Guid.TryParse(tipo.TipoHabitacionGuid, out var tipoGuid) ? tipoGuid : Guid.Empty;
                if (tipoHabitacionGuid.HasValue && tipoHabitacionGuid.Value != Guid.Empty && tipoGuidParsed != tipoHabitacionGuid.Value)
                {
                    continue;
                }

                results.Add(new AlojamientoHabitacionResponseModel
                {
                    HabitacionGuid = Guid.TryParse(item.HabitacionGuid, out var habitacionGuid) ? habitacionGuid : Guid.Empty,
                    TipoHabitacionGuid = tipoGuidParsed,
                    TipoNombre = tipo.NombreTipoHabitacion,
                    NumeroHabitacion = item.NumeroHabitacion,
                    Piso = item.Piso,
                    CapacidadAdultos = tipo.CapacidadAdultos,
                    CapacidadNinos = tipo.CapacidadNinos,
                    PrecioBase = decimal.TryParse(item.PrecioBase, out var precio) ? precio : 0m,
                    EstadoHabitacion = item.EstadoHabitacion,
                    DisponibleEnRango = true
                });
            }

            return results;
        }
        catch (RpcException ex)
        {
            throw GrpcStatusMapper.ToDownstream("Alojamiento", ex);
        }
    }
}

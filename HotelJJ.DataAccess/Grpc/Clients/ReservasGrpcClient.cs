using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using HotelJJ.DataAccess.Grpc.Common;
using HotelJJ.DataAccess.Grpc.Interfaces;
using HotelJJ.DataAccess.Http.Models.Reservas.Requests;
using HotelJJ.DataAccess.Http.Models.Reservas.Responses;
using Reservas.Contracts.Grpc.V1;

namespace HotelJJ.DataAccess.Grpc.Clients;

public class ReservasGrpcClient : IReservasGrpcClient
{
    private readonly ReservaGrpc.ReservaGrpcClient _reservaClient;
    private readonly ClienteGrpc.ClienteGrpcClient _clienteClient;
    private readonly Alojamiento.Contracts.Grpc.V1.AlojamientoGrpc.AlojamientoGrpcClient _alojamientoClient;
    private const decimal IvaRate = 0.12m;

    public ReservasGrpcClient(
        ReservaGrpc.ReservaGrpcClient reservaClient,
        ClienteGrpc.ClienteGrpcClient clienteClient,
        Alojamiento.Contracts.Grpc.V1.AlojamientoGrpc.AlojamientoGrpcClient alojamientoClient)
    {
        _reservaClient = reservaClient;
        _clienteClient = clienteClient;
        _alojamientoClient = alojamientoClient;
    }

    public async Task<ReservaResponseModel> CreateAsync(
        CreateReservaRequestModel request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (request.Cliente is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Debe enviar datos de cliente para crear una reserva publica."));
            }

            var grpcRequest = new ReservaPublicCreateRequest
            {
                SucursalGuid = request.SucursalGuid.ToString(),
                FechaInicio = Timestamp.FromDateTime(request.FechaInicio.ToUniversalTime()),
                FechaFin = Timestamp.FromDateTime(request.FechaFin.ToUniversalTime()),
                DescuentoAplicado = FormatDecimal(0m),
                OrigenCanalReserva = request.OrigenCanalReserva ?? "WEB",
                Observaciones = request.Observaciones ?? string.Empty,
                EsWalkin = request.EsWalkin,
                Cliente = new ClienteReservaPublicRequest
                {
                    TipoIdentificacion = request.Cliente.TipoIdentificacion,
                    NumeroIdentificacion = request.Cliente.NumeroIdentificacion,
                    Nombres = request.Cliente.Nombres,
                    Apellidos = request.Cliente.Apellidos ?? string.Empty,
                    Correo = request.Cliente.Correo,
                    Telefono = request.Cliente.Telefono,
                    Direccion = request.Cliente.Direccion ?? string.Empty
                }
            };

            grpcRequest.Habitaciones.AddRange(request.Habitaciones.Select(h => new ReservaHabitacionTipoRequest
            {
                TipoHabitacionGuid = h.TipoHabitacionGuid.ToString(),
                NumHabitaciones = h.NumHabitaciones,
                NumAdultos = h.NumAdultos,
                NumNinos = h.NumNinos,
                DescuentoLinea = FormatDecimal(0m)
            }));

            var response = await _reservaClient.CrearReservaPublicaAsync(grpcRequest, cancellationToken: cancellationToken);

            var created = await ToDataAsync(response, cancellationToken);
            if (created.SucursalGuid == Guid.Empty)
                created.SucursalGuid = request.SucursalGuid;

            return created;
        }
        catch (RpcException ex)
        {
            throw GrpcStatusMapper.ToDownstream("Reservas", ex);
        }
    }

    public async Task<ReservaResponseModel> GetByGuidAsync(Guid reservaGuid, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _reservaClient.GetReservaByGuidAsync(new Reservas.Contracts.Grpc.V1.GuidRequest { Guid = reservaGuid.ToString() }, cancellationToken: cancellationToken);
            return await ToDataAsync(response, cancellationToken);
        }
        catch (RpcException ex)
        {
            throw GrpcStatusMapper.ToDownstream("Reservas", ex);
        }
    }

    public async Task<InternalReservaResponseModel> GetInternalForHospedajeAsync(Guid reservaGuid, string? authorizationHeader, CancellationToken cancellationToken = default)
    {
        try
        {
            var reserva = await _reservaClient.GetReservaByGuidAsync(new Reservas.Contracts.Grpc.V1.GuidRequest { Guid = reservaGuid.ToString() }, cancellationToken: cancellationToken);
            return new InternalReservaResponseModel
            {
                IdReserva = reserva.IdReserva,
                GuidReserva = Guid.TryParse(reserva.ReservaGuid, out var guid) ? guid : Guid.Empty,
                EstadoReserva = reserva.EstadoReserva,
                Habitaciones = reserva.Habitaciones.Select(h => new InternalReservaHabitacionResponseModel
                {
                    IdReservaHabitacion = h.IdReservaHabitacion,
                    ReservaHabitacionGuid = Guid.TryParse(h.ReservaHabitacionGuid, out var detalleGuid) ? detalleGuid : Guid.Empty,
                    EstadoDetalle = h.EstadoDetalle
                }).ToList()
            };
        }
        catch (RpcException ex)
        {
            throw GrpcStatusMapper.ToDownstream("Reservas", ex);
        }
    }

    public async Task<ReservaPrecioResponseModel> CalcularPrecioAsync(
        ReservaPrecioRequestModel request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var habitacion = await _alojamientoClient.GetHabitacionByGuidAsync(
                new Alojamiento.Contracts.Grpc.V1.GuidRequest { Guid = request.HabitacionGuid.ToString() },
                cancellationToken: cancellationToken);

            var nights = Math.Max(1, (int)(request.FechaFin.Date - request.FechaInicio.Date).TotalDays);
            var tarifa = await _alojamientoClient.GetTarifaVigenteRangoAsync(new Alojamiento.Contracts.Grpc.V1.GetTarifaVigenteRangoRequest
            {
                IdSucursal = habitacion.IdSucursal,
                IdTipoHabitacion = habitacion.IdTipoHabitacion,
                FechaInicio = Timestamp.FromDateTime(request.FechaInicio.ToUniversalTime()),
                FechaFin = Timestamp.FromDateTime(request.FechaFin.ToUniversalTime()),
                Canal = request.Canal ?? "WEB"
            }, cancellationToken: cancellationToken);

            var precioNoche = tarifa.Encontrada
                ? ParseDecimal(tarifa.Tarifa.PrecioPorNoche)
                : ParseDecimal(habitacion.PrecioBase);
            var subtotal = Math.Round(precioNoche * nights, 2);
            var iva = Math.Round(subtotal * IvaRate, 2);
            var total = subtotal + iva;

            return new ReservaPrecioResponseModel
            {
                HabitacionGuid = request.HabitacionGuid,
                PrecioNocheAplicado = precioNoche,
                SubtotalLinea = subtotal,
                ValorIvaLinea = iva,
                TotalLinea = total,
                OrigenPrecio = tarifa.Encontrada ? "TARIFA_VIGENTE" : "PRECIO_BASE_HABITACION"
            };
        }
        catch (RpcException ex)
        {
            throw GrpcStatusMapper.ToDownstream("Reservas", ex);
        }
    }

    public async Task CancelarAsync(
        Guid reservaGuid,
        CancelarReservaRequestModel request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var reserva = await _reservaClient.GetReservaByGuidAsync(
                new Reservas.Contracts.Grpc.V1.GuidRequest { Guid = reservaGuid.ToString() },
                cancellationToken: cancellationToken);

            await _reservaClient.CancelarReservaAsync(new CancelarReservaRequest
            {
                IdReserva = reserva.IdReserva,
                Motivo = string.IsNullOrWhiteSpace(request.Motivo) ? "Cancelacion solicitada desde Middleware" : request.Motivo,
                Usuario = "Middleware.HotelJJ"
            }, cancellationToken: cancellationToken);
        }
        catch (RpcException ex)
        {
            throw GrpcStatusMapper.ToDownstream("Reservas", ex);
        }
    }

    private async Task<ReservaResponseModel> ToDataAsync(Reserva response, CancellationToken cancellationToken)
    {
        var clienteGuid = Guid.Empty;
        if (response.IdCliente > 0)
        {
            try
            {
                var cliente = await _clienteClient.GetClienteByIdAsync(
                    new Reservas.Contracts.Grpc.V1.IdRequest { Id = response.IdCliente },
                    cancellationToken: cancellationToken);
                clienteGuid = Guid.TryParse(cliente.ClienteGuid, out var parsed) ? parsed : Guid.Empty;
            }
            catch (RpcException)
            {
                clienteGuid = Guid.Empty;
            }
        }

        var sucursalGuid = Guid.TryParse(response.SucursalGuid, out var sucursalParsed) ? sucursalParsed : Guid.Empty;
        if (sucursalGuid == Guid.Empty && response.IdSucursal > 0)
        {
            try
            {
                var sucursal = await _alojamientoClient.GetSucursalByIdAsync(
                    new Alojamiento.Contracts.Grpc.V1.IdRequest { Id = response.IdSucursal },
                    cancellationToken: cancellationToken);
                sucursalGuid = Guid.TryParse(sucursal.SucursalGuid, out var parsed) ? parsed : Guid.Empty;
            }
            catch (RpcException)
            {
                sucursalGuid = Guid.Empty;
            }
        }

        return new ReservaResponseModel
        {
            ReservaGuid = Guid.TryParse(response.ReservaGuid, out var reservaGuid) ? reservaGuid : Guid.Empty,
            CodigoReserva = response.CodigoReserva,
            ClienteGuid = clienteGuid,
            SucursalGuid = sucursalGuid,
            FechaReservaUtc = response.FechaReservaUtc.ToDateTime(),
            FechaInicio = response.FechaInicio.ToDateTime(),
            FechaFin = response.FechaFin.ToDateTime(),
            SubtotalReserva = ParseDecimal(response.SubtotalReserva),
            ValorIva = ParseDecimal(response.ValorIva),
            TotalReserva = ParseDecimal(response.TotalReserva),
            DescuentoAplicado = ParseDecimal(response.DescuentoAplicado),
            SaldoPendiente = ParseDecimal(response.SaldoPendiente),
            OrigenCanalReserva = response.OrigenCanalReserva,
            EstadoReserva = response.EstadoReserva,
            MotivoCancelacion = response.MotivoCancelacion,
            Observaciones = response.Observaciones,
            EsWalkin = response.EsWalkin,
            Habitaciones = response.Habitaciones.Select(h => new ReservaHabitacionResponseModel
            {
                ReservaHabitacionGuid = Guid.TryParse(h.ReservaHabitacionGuid, out var detalleGuid) ? detalleGuid : Guid.Empty,
                HabitacionGuid = Guid.TryParse(h.HabitacionGuid, out var habitacionGuid) ? habitacionGuid : Guid.Empty,
                FechaInicio = h.FechaInicio.ToDateTime(),
                FechaFin = h.FechaFin.ToDateTime(),
                NumAdultos = h.NumAdultos,
                NumNinos = h.NumNinos,
                PrecioNocheAplicado = ParseDecimal(h.PrecioNocheAplicado),
                SubtotalLinea = ParseDecimal(h.SubtotalLinea),
                ValorIvaLinea = ParseDecimal(h.ValorIvaLinea),
                DescuentoLinea = ParseDecimal(h.DescuentoLinea),
                TotalLinea = ParseDecimal(h.TotalLinea),
                EstadoDetalle = h.EstadoDetalle
            }).ToList()
        };
    }

    private static string FormatDecimal(decimal value)
    {
        return value.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
    }

    private static decimal ParseDecimal(string value)
    {
        return decimal.TryParse(value, out var parsed) ? parsed : 0m;
    }
}

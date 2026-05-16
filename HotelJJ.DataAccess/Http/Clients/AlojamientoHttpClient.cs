using System.Globalization;
using System.Net;
using System.Text.Json;
using HotelJJ.DataAccess.Http.Common;
using HotelJJ.DataAccess.Http.Interfaces;
using HotelJJ.DataAccess.Http.Models.Alojamiento.Requests;
using HotelJJ.DataAccess.Http.Models.Alojamiento.Responses;
using HotelJJ.DataAccess.Http.Routes;

namespace HotelJJ.DataAccess.Http.Clients;

public class AlojamientoHttpClient : IAlojamientoHttpClient
{
    private const string ServiceName = "Alojamiento";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _httpClient;

    public AlojamientoHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<PagedAlojamientoResponseModel<AlojamientoSearchItemResponseModel>> SearchAsync(
        SearchAlojamientoRequestModel request,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>
        {
            ["Destino"] = request.Destino,
            ["FechaEntrada"] = FormatDate(request.FechaEntrada),
            ["FechaSalida"] = FormatDate(request.FechaSalida),
            ["NumAdultos"] = FormatNumber(request.NumAdultos),
            ["NumNinos"] = FormatNumber(request.NumNinos),
            ["NumHabitaciones"] = FormatNumber(request.NumHabitaciones),
            ["TipoAlojamiento"] = request.TipoAlojamiento,
            ["PrecioMin"] = FormatNumber(request.PrecioMin),
            ["PrecioMax"] = FormatNumber(request.PrecioMax),
            ["CategoriaViaje"] = request.CategoriaViaje,
            ["OrdenarPor"] = request.OrdenarPor,
            ["Pagina"] = request.Pagina.ToString(CultureInfo.InvariantCulture),
            ["Limite"] = request.Limite.ToString(CultureInfo.InvariantCulture)
        };

        return GetAsync<PagedAlojamientoResponseModel<AlojamientoSearchItemResponseModel>>(
            BuildUri(AlojamientoRoutes.AccommodationsSearch, query),
            cancellationToken);
    }

    public Task<AlojamientoDetailResponseModel> GetDetailAsync(
        Guid sucursalGuid,
        AlojamientoDetailRequestModel request,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>
        {
            ["fechaEntrada"] = FormatDate(request.FechaEntrada),
            ["fechaSalida"] = FormatDate(request.FechaSalida)
        };

        return GetAsync<AlojamientoDetailResponseModel>(
            BuildUri(string.Format(AlojamientoRoutes.AccommodationsBySucursalTemplate, $"{sucursalGuid:D}"), query),
            cancellationToken);
    }

    public Task<PagedAlojamientoResponseModel<AlojamientoReviewResponseModel>> GetReviewsAsync(
        Guid sucursalGuid,
        AlojamientoReviewsRequestModel request,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>
        {
            ["pagina"] = request.Pagina.ToString(CultureInfo.InvariantCulture),
            ["limite"] = request.Limite.ToString(CultureInfo.InvariantCulture)
        };

        return GetAsync<PagedAlojamientoResponseModel<AlojamientoReviewResponseModel>>(
            BuildUri(string.Format(AlojamientoRoutes.AccommodationsReviewsTemplate, $"{sucursalGuid:D}"), query),
            cancellationToken);
    }

    public Task<IReadOnlyList<AlojamientoHabitacionResponseModel>> GetHabitacionesAsync(
        Guid sucursalGuid,
        AlojamientoHabitacionesRequestModel request,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>
        {
            ["tipoHabitacionGuid"] = request.TipoHabitacionGuid.HasValue && request.TipoHabitacionGuid.Value != Guid.Empty
                ? request.TipoHabitacionGuid.Value.ToString("D")
                : null,
            ["fechaInicio"] = FormatDate(request.FechaInicio),
            ["fechaFin"] = FormatDate(request.FechaFin)
        };

        return GetAsync<IReadOnlyList<AlojamientoHabitacionResponseModel>>(
            BuildUri(string.Format(AlojamientoRoutes.AccommodationsHabitacionesTemplate, $"{sucursalGuid:D}"), query),
            cancellationToken);
    }

    private async Task<TResponse> GetAsync<TResponse>(string requestUri, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync(requestUri, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new DownstreamApiException(
                ServiceName,
                response.StatusCode,
                $"El microservicio Alojamiento respondio {(int)response.StatusCode} ({response.ReasonPhrase}).",
                body,
                requestUri);
        }

        if (string.IsNullOrWhiteSpace(body))
        {
            throw new DownstreamApiException(
                ServiceName,
                HttpStatusCode.NoContent,
                "El microservicio Alojamiento devolvio una respuesta vacia.",
                null,
                requestUri);
        }

        var downstreamResponse = JsonSerializer.Deserialize<TResponse>(body, JsonOptions);
        if (downstreamResponse is null)
        {
            throw new DownstreamApiException(
                ServiceName,
                response.StatusCode,
                "No se pudo interpretar la respuesta del microservicio Alojamiento.",
                body,
                requestUri);
        }

        return downstreamResponse;
    }

    private static string BuildUri(string path, IReadOnlyDictionary<string, string?> query)
    {
        var queryString = string.Join(
            "&",
            query
                .Where(pair => !string.IsNullOrWhiteSpace(pair.Value))
                .Select(pair => $"{Uri.EscapeDataString(pair.Key)}={Uri.EscapeDataString(pair.Value!)}"));

        return string.IsNullOrWhiteSpace(queryString)
            ? path
            : $"{path}?{queryString}";
    }

    private static string? FormatDate(DateTime? value)
    {
        return value?.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture);
    }

    private static string? FormatNumber(int? value)
    {
        return value?.ToString(CultureInfo.InvariantCulture);
    }

    private static string? FormatNumber(decimal? value)
    {
        return value?.ToString(CultureInfo.InvariantCulture);
    }
}

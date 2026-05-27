using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using HotelJJ.DataAccess.Http.Common;
using HotelJJ.DataAccess.Http.Interfaces;
using HotelJJ.DataAccess.Http.Models.Reservas.Requests;
using HotelJJ.DataAccess.Http.Models.Reservas.Responses;
using HotelJJ.DataAccess.Http.Routes;

namespace HotelJJ.DataAccess.Http.Clients;

public class ReservasHttpClient : IReservasHttpClient
{
    private const string ServiceName = "Reservas";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _httpClient;

    public ReservasHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<ReservaResponseModel> CreateAsync(
        CreateReservaRequestModel request,
        CancellationToken cancellationToken = default)
    {
        return PostAsync<CreateReservaRequestModel, ReservaResponseModel>(
            ReservasRoutes.PublicReservas,
            request,
            cancellationToken);
    }

    public Task<ReservaResponseModel> GetByGuidAsync(
        Guid reservaGuid,
        CancellationToken cancellationToken = default)
    {
        return GetAsync<ReservaResponseModel>(
            string.Format(ReservasRoutes.PublicReservaByGuidTemplate, $"{reservaGuid:D}"),
            cancellationToken);
    }

    public async Task<ReservaResponseModel> GetByGuidAuthorizedAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var requestUri = string.Format(ReservasRoutes.PublicReservaByGuidTemplate, $"{reservaGuid:D}");
        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        ApplyAuthorization(request, authorizationHeader);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            ThrowDownstream(response, body);
        }

        return Deserialize<ReservaResponseModel>(response.StatusCode, body);
    }

    public async Task<InternalReservaResponseModel> GetInternalByGuidAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, ReservasRoutes.InternalReservas);
        ApplyAuthorization(request, authorizationHeader);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            ThrowDownstream(response, body);
        }

        var reservas = Deserialize<List<InternalReservaResponseModel>>(response.StatusCode, body);
        var reserva = reservas.FirstOrDefault(item => item.GuidReserva == reservaGuid);
        if (reserva is null)
        {
            throw new DownstreamApiException(
                ServiceName,
                HttpStatusCode.NotFound,
                "No se encontro la reserva solicitada en el endpoint interno de Reservas.",
                body,
                ReservasRoutes.InternalReservas);
        }

        return reserva;
    }

    public Task<ReservaPrecioResponseModel> CalcularPrecioAsync(
        ReservaPrecioRequestModel request,
        CancellationToken cancellationToken = default)
    {
        return PostAsync<ReservaPrecioRequestModel, ReservaPrecioResponseModel>(
            ReservasRoutes.PublicReservasCalcularPrecio,
            request,
            cancellationToken);
    }

    public async Task CancelarAsync(
        Guid reservaGuid,
        CancelarReservaRequestModel request,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PatchAsJsonAsync(
            string.Format(ReservasRoutes.PublicReservaCancelarByGuidTemplate, $"{reservaGuid:D}"),
            request,
            JsonOptions,
            cancellationToken);

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            ThrowDownstream(response, body);
        }
    }

    private async Task<TResponse> GetAsync<TResponse>(
        string requestUri,
        CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync(requestUri, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            ThrowDownstream(response, body);
        }

        return Deserialize<TResponse>(response.StatusCode, body);
    }

    private async Task<TResponse> PostAsync<TRequest, TResponse>(
        string requestUri,
        TRequest request,
        CancellationToken cancellationToken)
    {
        using var response = await _httpClient.PostAsJsonAsync(requestUri, request, JsonOptions, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            ThrowDownstream(response, body);
        }

        return Deserialize<TResponse>(response.StatusCode, body);
    }

    private static TResponse Deserialize<TResponse>(HttpStatusCode statusCode, string body)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            throw new DownstreamApiException(
                ServiceName,
                HttpStatusCode.NoContent,
                "El microservicio Reservas devolvio una respuesta vacia.",
                null,
                null);
        }

        var downstreamResponse = JsonSerializer.Deserialize<TResponse>(body, JsonOptions);
        if (downstreamResponse is null)
        {
            throw new DownstreamApiException(
                ServiceName,
                statusCode,
                "No se pudo interpretar la respuesta del microservicio Reservas.",
                body,
                null);
        }

        return downstreamResponse;
    }

    private static void ThrowDownstream(HttpResponseMessage response, string body)
    {
        throw new DownstreamApiException(
            ServiceName,
            response.StatusCode,
            DownstreamErrorMessageExtractor.BuildMessage(ServiceName, response, body),
            body,
            response.RequestMessage?.RequestUri?.PathAndQuery);
    }

    private static void ApplyAuthorization(HttpRequestMessage request, string? authorizationHeader)
    {
        if (string.IsNullOrWhiteSpace(authorizationHeader))
        {
            return;
        }

        if (AuthenticationHeaderValue.TryParse(authorizationHeader, out var headerValue))
        {
            request.Headers.Authorization = headerValue;
        }
    }
}

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using HotelJJ.DataAccess.Http.Common;
using HotelJJ.DataAccess.Http.Interfaces;
using HotelJJ.DataAccess.Http.Models.Hospedaje.Requests;
using HotelJJ.DataAccess.Http.Models.Hospedaje.Responses;
using HotelJJ.DataAccess.Http.Routes;

namespace HotelJJ.DataAccess.Http.Clients;

public class HospedajeHttpClient : IHospedajeHttpClient
{
    private const string ServiceName = "Hospedaje";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _httpClient;

    public HospedajeHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<IReadOnlyList<EstadiaHospedajeResponseModel>> CheckInAsync(
        int idReservaHabitacion,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<IReadOnlyList<EstadiaHospedajeResponseModel>>(
            HttpMethod.Post,
            string.Format(HospedajeRoutes.InternalCheckInByReservaIdTemplate, idReservaHabitacion),
            null,
            authorizationHeader,
            cancellationToken);
    }

    public Task CheckOutAsync(
        int idEstadia,
        CheckOutHospedajeRequestModel request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        return SendNoContentAsync(
            HttpMethod.Patch,
            string.Format(HospedajeRoutes.InternalCheckOutByEstadiaIdTemplate, idEstadia),
            request,
            authorizationHeader,
            cancellationToken);
    }

    public Task<EstadiaHospedajeResponseModel> GetByIdAsync(
        int idEstadia,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<EstadiaHospedajeResponseModel>(
            HttpMethod.Get,
            string.Format(HospedajeRoutes.InternalEstadiaByIdTemplate, idEstadia),
            null,
            authorizationHeader,
            cancellationToken);
    }

    public Task<IReadOnlyList<EstadiaHospedajeResponseModel>> GetAllAsync(
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<IReadOnlyList<EstadiaHospedajeResponseModel>>(
            HttpMethod.Get,
            HospedajeRoutes.InternalEstadias,
            null,
            authorizationHeader,
            cancellationToken);
    }

    public Task<CargoHospedajeResponseModel> AddCargoAsync(
        int idEstadia,
        CargoHospedajeRequestModel request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<CargoHospedajeResponseModel>(
            HttpMethod.Post,
            string.Format(HospedajeRoutes.InternalCargosByEstadiaIdTemplate, idEstadia),
            request,
            authorizationHeader,
            cancellationToken);
    }

    public Task<IReadOnlyList<CargoHospedajeResponseModel>> GetCargosByEstadiaAsync(
        int idEstadia,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<IReadOnlyList<CargoHospedajeResponseModel>>(
            HttpMethod.Get,
            string.Format(HospedajeRoutes.InternalCargosByEstadiaIdTemplate, idEstadia),
            null,
            authorizationHeader,
            cancellationToken);
    }

    public Task AnularCargoAsync(
        int idCargo,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        return SendNoContentAsync(
            HttpMethod.Patch,
            string.Format(HospedajeRoutes.InternalAnularCargoByIdTemplate, idCargo),
            null,
            authorizationHeader,
            cancellationToken);
    }

    private async Task<TResponse> SendAsync<TResponse>(
        HttpMethod method,
        string requestUri,
        object? requestBody,
        string? authorizationHeader,
        CancellationToken cancellationToken)
    {
        using var request = CreateRequest(method, requestUri, requestBody, authorizationHeader);
        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            ThrowDownstream(response, body);
        }

        return Deserialize<TResponse>(response.StatusCode, body);
    }

    private async Task SendNoContentAsync(
        HttpMethod method,
        string requestUri,
        object? requestBody,
        string? authorizationHeader,
        CancellationToken cancellationToken)
    {
        using var request = CreateRequest(method, requestUri, requestBody, authorizationHeader);
        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            ThrowDownstream(response, body);
        }
    }

    private static HttpRequestMessage CreateRequest(
        HttpMethod method,
        string requestUri,
        object? requestBody,
        string? authorizationHeader)
    {
        var request = new HttpRequestMessage(method, requestUri);
        if (requestBody is not null)
        {
            request.Content = JsonContent.Create(requestBody, options: JsonOptions);
        }

        if (!string.IsNullOrWhiteSpace(authorizationHeader) &&
            AuthenticationHeaderValue.TryParse(authorizationHeader, out var headerValue))
        {
            request.Headers.Authorization = headerValue;
        }

        return request;
    }

    private static TResponse Deserialize<TResponse>(HttpStatusCode statusCode, string body)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            throw new DownstreamApiException(
                ServiceName,
                HttpStatusCode.NoContent,
                "El microservicio Hospedaje devolvio una respuesta vacia.",
                null,
                null);
        }

        var downstreamResponse = JsonSerializer.Deserialize<TResponse>(body, JsonOptions);
        if (downstreamResponse is null)
        {
            throw new DownstreamApiException(
                ServiceName,
                statusCode,
                "No se pudo interpretar la respuesta del microservicio Hospedaje.",
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
}

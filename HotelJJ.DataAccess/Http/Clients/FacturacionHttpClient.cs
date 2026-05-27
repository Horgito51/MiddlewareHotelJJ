using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using HotelJJ.DataAccess.Http.Common;
using HotelJJ.DataAccess.Http.Interfaces;
using HotelJJ.DataAccess.Http.Models.Facturacion.Requests;
using HotelJJ.DataAccess.Http.Models.Facturacion.Responses;
using HotelJJ.DataAccess.Http.Routes;

namespace HotelJJ.DataAccess.Http.Clients;

public class FacturacionHttpClient : IFacturacionHttpClient
{
    private const string ServiceName = "Facturacion";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _httpClient;

    public FacturacionHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<IReadOnlyList<FacturaFacturacionResponseModel>> GetFacturasAsync(
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<IReadOnlyList<FacturaFacturacionResponseModel>>(
            HttpMethod.Get,
            FacturacionRoutes.InternalFacturas,
            null,
            authorizationHeader,
            cancellationToken);
    }

    public Task<FacturaFacturacionResponseModel> GetFacturaByIdAsync(
        int idFactura,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<FacturaFacturacionResponseModel>(
            HttpMethod.Get,
            string.Format(FacturacionRoutes.InternalFacturaByIdTemplate, idFactura),
            null,
            authorizationHeader,
            cancellationToken);
    }

    public Task<FacturaFacturacionResponseModel> GenerarFacturaReservaAsync(
        int idReserva,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<FacturaFacturacionResponseModel>(
            HttpMethod.Post,
            string.Format(FacturacionRoutes.InternalFacturaGenerarReservaByIdTemplate, idReserva),
            null,
            authorizationHeader,
            cancellationToken);
    }

    public Task<FacturaFacturacionResponseModel> GenerarFacturaFinalAsync(
        int idReserva,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<FacturaFacturacionResponseModel>(
            HttpMethod.Post,
            string.Format(FacturacionRoutes.InternalFacturaGenerarFinalByIdTemplate, idReserva),
            null,
            authorizationHeader,
            cancellationToken);
    }

    public Task<PagoFacturacionResponseModel> RegistrarPagoAsync(
        PagoCreateFacturacionRequestModel request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<PagoFacturacionResponseModel>(
            HttpMethod.Post,
            FacturacionRoutes.InternalPagos,
            request,
            authorizationHeader,
            cancellationToken);
    }

    public Task<PagoSimuladoFacturacionResponseModel> SimularPagoAsync(
        PagoSimularFacturacionRequestModel request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        return SendAsync<PagoSimuladoFacturacionResponseModel>(
            HttpMethod.Post,
            FacturacionRoutes.PagosSimular,
            request,
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
                "El microservicio Facturacion devolvio una respuesta vacia.",
                null,
                null);
        }

        var downstreamResponse = JsonSerializer.Deserialize<TResponse>(body, JsonOptions);
        if (downstreamResponse is null)
        {
            throw new DownstreamApiException(
                ServiceName,
                statusCode,
                "No se pudo interpretar la respuesta del microservicio Facturacion.",
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

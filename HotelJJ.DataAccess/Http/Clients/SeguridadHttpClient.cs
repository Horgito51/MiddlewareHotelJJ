using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using HotelJJ.DataAccess.Http.Common;
using HotelJJ.DataAccess.Http.Interfaces;
using HotelJJ.DataAccess.Http.Models.Seguridad.Requests;
using HotelJJ.DataAccess.Http.Models.Seguridad.Responses;
using HotelJJ.DataAccess.Http.Routes;

namespace HotelJJ.DataAccess.Http.Clients;

public class SeguridadHttpClient : ISeguridadHttpClient
{
    private const string ServiceName = "Seguridad";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _httpClient;

    public SeguridadHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginSeguridadResponseModel> LoginAsync(
        LoginSeguridadRequestModel request,
        CancellationToken cancellationToken = default)
    {
        return await PostAsync<LoginSeguridadRequestModel, LoginSeguridadResponseModel>(
            SeguridadRoutes.AuthLogin,
            request,
            cancellationToken);
    }

    public async Task<RefreshTokenSeguridadResponseModel> RefreshTokenAsync(
        RefreshTokenSeguridadRequestModel request,
        CancellationToken cancellationToken = default)
    {
        return await PostAsync<RefreshTokenSeguridadRequestModel, RefreshTokenSeguridadResponseModel>(
            SeguridadRoutes.AuthRefresh,
            request,
            cancellationToken);
    }

    public async Task<LoginSeguridadResponseModel> RegisterClienteAsync(
        LoginSeguridadRequestModel request,
        CancellationToken cancellationToken = default)
    {
        return await PostAsync<LoginSeguridadRequestModel, LoginSeguridadResponseModel>(
            SeguridadRoutes.AuthRegisterCliente,
            request,
            cancellationToken);
    }

    public async Task LogoutAsync(
        LogoutSeguridadRequestModel request,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync(
            SeguridadRoutes.AuthLogout,
            request,
            JsonOptions,
            cancellationToken);

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new DownstreamApiException(
                ServiceName,
                response.StatusCode,
                DownstreamErrorMessageExtractor.BuildMessage(ServiceName, response, body),
                body,
                SeguridadRoutes.AuthLogout);
        }
    }

    public async Task CambiarPasswordAsync(
        CambiarPasswordSeguridadRequestModel request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        using var message = new HttpRequestMessage(HttpMethod.Post, SeguridadRoutes.AuthCambiarPassword)
        {
            Content = JsonContent.Create(request, options: JsonOptions)
        };

        if (!string.IsNullOrWhiteSpace(authorizationHeader) &&
            AuthenticationHeaderValue.TryParse(authorizationHeader, out var headerValue))
        {
            message.Headers.Authorization = headerValue;
        }

        using var response = await _httpClient.SendAsync(message, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new DownstreamApiException(
                ServiceName,
                response.StatusCode,
                DownstreamErrorMessageExtractor.BuildMessage(ServiceName, response, body),
                body,
                SeguridadRoutes.AuthCambiarPassword);
        }
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
            throw new DownstreamApiException(
                ServiceName,
                response.StatusCode,
                DownstreamErrorMessageExtractor.BuildMessage(ServiceName, response, body),
                body,
                requestUri);
        }

        if (string.IsNullOrWhiteSpace(body))
        {
            throw new DownstreamApiException(
                ServiceName,
                HttpStatusCode.NoContent,
                "El microservicio Seguridad devolvio una respuesta vacia.",
                null,
                requestUri);
        }

        var wrappedResponse = JsonSerializer.Deserialize<SeguridadApiResponseModel<TResponse>>(body, JsonOptions);
        if (wrappedResponse is null || wrappedResponse.Data is null)
        {
            throw new DownstreamApiException(
                ServiceName,
                response.StatusCode,
                "No se pudo interpretar la respuesta de login del microservicio Seguridad.",
                body,
                requestUri);
        }

        return wrappedResponse.Data;
    }
}

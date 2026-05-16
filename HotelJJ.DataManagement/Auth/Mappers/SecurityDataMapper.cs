using HotelJJ.DataAccess.Http.Models.Seguridad.Requests;
using HotelJJ.DataAccess.Http.Models.Seguridad.Responses;
using HotelJJ.DataManagement.Auth.Models;

namespace HotelJJ.DataManagement.Auth.Mappers;

public static class SecurityDataMapper
{
    public static LoginSeguridadRequestModel ToSeguridadRequest(LoginDataRequest request)
    {
        return new LoginSeguridadRequestModel
        {
            Username = request.Username,
            Password = request.Password
        };
    }

    public static RefreshTokenSeguridadRequestModel ToSeguridadRefreshRequest(RefreshTokenDataRequest request)
    {
        return new RefreshTokenSeguridadRequestModel
        {
            RefreshToken = request.RefreshToken
        };
    }

    public static LoginSeguridadRequestModel ToSeguridadRegisterRequest(RegisterClienteDataRequest request)
    {
        return new LoginSeguridadRequestModel
        {
            Username = request.Username,
            Password = request.Password
        };
    }

    public static LogoutSeguridadRequestModel ToSeguridadLogoutRequest(LogoutDataRequest request)
    {
        return new LogoutSeguridadRequestModel
        {
            RefreshToken = request.RefreshToken
        };
    }

    public static CambiarPasswordSeguridadRequestModel ToSeguridadCambiarPasswordRequest(CambiarPasswordDataRequest request)
    {
        return new CambiarPasswordSeguridadRequestModel
        {
            PasswordActual = request.PasswordActual,
            PasswordNuevo = request.PasswordNuevo
        };
    }

    public static TokenDataModel ToTokenDataModel(LoginSeguridadResponseModel response)
    {
        return new TokenDataModel
        {
            AccessToken = response.AccessToken,
            RefreshToken = response.RefreshToken,
            ExpiresIn = response.ExpiresIn,
            IdCliente = response.IdCliente,
            ClienteGuid = response.ClienteGuid,
            UsuarioGuid = response.UsuarioGuid,
            Username = response.Username,
            Correo = response.Correo,
            NombreCompleto = response.NombreCompleto,
            Roles = response.Roles
        };
    }
}

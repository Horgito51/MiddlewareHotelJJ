using HotelJJ.Business.DTOs.Auth;
using HotelJJ.DataManagement.Auth.Models;

namespace HotelJJ.Business.Mappers.Auth;

public static class AuthBusinessMapper
{
    public static LoginDataRequest ToDataRequest(LoginIntegrationDTO request)
    {
        return new LoginDataRequest
        {
            Username = request.Username.Trim(),
            Password = request.Password
        };
    }

    public static RefreshTokenDataRequest ToDataRequest(RefreshTokenIntegrationDTO request)
    {
        return new RefreshTokenDataRequest
        {
            RefreshToken = request.RefreshToken
        };
    }

    public static RegisterClienteDataRequest ToDataRequest(RegisterClienteIntegrationDTO request)
    {
        return new RegisterClienteDataRequest
        {
            Username = request.Username.Trim(),
            Password = request.Password
        };
    }

    public static LogoutDataRequest ToDataRequest(LogoutIntegrationDTO request)
    {
        return new LogoutDataRequest
        {
            RefreshToken = request.RefreshToken
        };
    }

    public static CambiarPasswordDataRequest ToDataRequest(CambiarPasswordIntegrationDTO request)
    {
        return new CambiarPasswordDataRequest
        {
            PasswordActual = request.PasswordActual,
            PasswordNuevo = request.PasswordNuevo
        };
    }

    public static TokenIntegrationDTO ToIntegrationToken(TokenDataModel token)
    {
        return new TokenIntegrationDTO
        {
            AccessToken = token.AccessToken,
            RefreshToken = token.RefreshToken,
            ExpiresIn = token.ExpiresIn,
            ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn),
            IdCliente = token.IdCliente,
            ClienteGuid = token.ClienteGuid,
            UsuarioGuid = token.UsuarioGuid,
            Username = token.Username,
            Correo = token.Correo,
            NombreCompleto = token.NombreCompleto,
            Roles = token.Roles
        };
    }
}

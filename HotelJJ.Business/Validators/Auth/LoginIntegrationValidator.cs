using HotelJJ.Business.DTOs.Auth;
using HotelJJ.Business.Exceptions;

namespace HotelJJ.Business.Validators.Auth;

public static class LoginIntegrationValidator
{
    public static void Validate(LoginIntegrationDTO request)
    {
        if (request is null)
        {
            throw new IntegrationValidationException("MID-AUTH-001", "La solicitud de login no puede ser nula.");
        }

        if (string.IsNullOrWhiteSpace(request.Username))
        {
            throw new IntegrationValidationException("MID-AUTH-002", "El usuario o correo es requerido.");
        }

        if (request.Username.Length > 100)
        {
            throw new IntegrationValidationException("MID-AUTH-003", "El usuario o correo no debe superar 100 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new IntegrationValidationException("MID-AUTH-004", "La contrasena es requerida.");
        }
    }
}

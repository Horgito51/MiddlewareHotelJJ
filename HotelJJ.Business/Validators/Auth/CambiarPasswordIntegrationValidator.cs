using HotelJJ.Business.DTOs.Auth;
using HotelJJ.Business.Exceptions;

namespace HotelJJ.Business.Validators.Auth;

public static class CambiarPasswordIntegrationValidator
{
    public static void Validate(CambiarPasswordIntegrationDTO request)
    {
        if (request is null)
        {
            throw new IntegrationValidationException("MID-AUTH-016", "La solicitud de cambio de contrasena no puede ser nula.");
        }

        if (string.IsNullOrWhiteSpace(request.PasswordActual))
        {
            throw new IntegrationValidationException("MID-AUTH-017", "La contrasena actual es requerida.");
        }

        if (string.IsNullOrWhiteSpace(request.PasswordNuevo))
        {
            throw new IntegrationValidationException("MID-AUTH-018", "La nueva contrasena es requerida.");
        }
    }
}

using HotelJJ.Business.DTOs.Auth;
using HotelJJ.Business.Exceptions;

namespace HotelJJ.Business.Validators.Auth;

public static class LogoutIntegrationValidator
{
    public static void Validate(LogoutIntegrationDTO request)
    {
        if (request is null)
        {
            throw new IntegrationValidationException("MID-AUTH-014", "La solicitud de logout no puede ser nula.");
        }

        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            throw new IntegrationValidationException("MID-AUTH-015", "El refresh token es requerido para logout.");
        }
    }
}

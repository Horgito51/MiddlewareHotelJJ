using HotelJJ.Business.DTOs.Auth;
using HotelJJ.Business.Exceptions;

namespace HotelJJ.Business.Validators.Auth;

public static class RefreshTokenIntegrationValidator
{
    public static void Validate(RefreshTokenIntegrationDTO request)
    {
        if (request is null)
        {
            throw new IntegrationValidationException("MID-AUTH-009", "La solicitud de refresh token no puede ser nula.");
        }

        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            throw new IntegrationValidationException("MID-AUTH-010", "El refresh token es requerido.");
        }
    }
}

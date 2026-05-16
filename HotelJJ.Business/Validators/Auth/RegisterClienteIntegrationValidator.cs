using HotelJJ.Business.DTOs.Auth;
using HotelJJ.Business.Exceptions;

namespace HotelJJ.Business.Validators.Auth;

public static class RegisterClienteIntegrationValidator
{
    public static void Validate(RegisterClienteIntegrationDTO request)
    {
        if (request is null)
        {
            throw new IntegrationValidationException("MID-AUTH-011", "La solicitud de registro no puede ser nula.");
        }

        if (string.IsNullOrWhiteSpace(request.Username))
        {
            throw new IntegrationValidationException("MID-AUTH-012", "El usuario es requerido para el registro.");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new IntegrationValidationException("MID-AUTH-013", "La contrasena es requerida para el registro.");
        }
    }
}

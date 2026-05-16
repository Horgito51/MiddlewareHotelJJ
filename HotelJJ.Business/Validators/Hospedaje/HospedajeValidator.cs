using HotelJJ.Business.DTOs.Hospedaje;
using HotelJJ.Business.Exceptions;

namespace HotelJJ.Business.Validators.Hospedaje;

public static class HospedajeValidator
{
    public static void ValidateGuid(Guid guid, string fieldName)
    {
        if (guid == Guid.Empty)
        {
            throw new IntegrationValidationException("MID-HOS-001", $"{fieldName} es obligatorio.");
        }
    }

    public static void ValidateCargo(CargoHospedajeCreateDTO request)
    {
        if (string.IsNullOrWhiteSpace(request.DescripcionCargo))
        {
            throw new IntegrationValidationException("MID-HOS-002", "descripcionCargo es obligatorio.");
        }

        if (request.Cantidad <= 0)
        {
            throw new IntegrationValidationException("MID-HOS-003", "cantidad debe ser mayor a cero.");
        }

        if (request.PrecioUnitario < 0 || request.ValorIva < 0)
        {
            throw new IntegrationValidationException("MID-HOS-004", "precioUnitario y valorIva no pueden ser negativos.");
        }
    }
}

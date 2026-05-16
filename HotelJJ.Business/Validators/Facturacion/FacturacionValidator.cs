using HotelJJ.Business.DTOs.Facturacion;
using HotelJJ.Business.Exceptions;

namespace HotelJJ.Business.Validators.Facturacion;

public static class FacturacionValidator
{
    public static void ValidateGuid(Guid guid, string fieldName)
    {
        if (guid == Guid.Empty)
        {
            throw new IntegrationValidationException("MID-FAC-001", $"{fieldName} es obligatorio.");
        }
    }

    public static void ValidatePago(PagoCreateDTO request)
    {
        ValidateGuid(request.FacturaGuid, "facturaGuid");

        if (request.Monto <= 0)
        {
            throw new IntegrationValidationException("MID-FAC-002", "monto debe ser mayor a cero.");
        }

        if (string.IsNullOrWhiteSpace(request.MetodoPago))
        {
            throw new IntegrationValidationException("MID-FAC-003", "metodoPago es obligatorio.");
        }

        if (request.TipoCambio <= 0)
        {
            throw new IntegrationValidationException("MID-FAC-004", "tipoCambio debe ser mayor a cero.");
        }
    }

    public static void ValidateSimulacion(PagoSimularDTO request)
    {
        ValidateGuid(request.ReservaGuid, "reservaGuid");

        if (request.Monto <= 0)
        {
            throw new IntegrationValidationException("MID-FAC-005", "monto debe ser mayor a cero.");
        }

        if (string.IsNullOrWhiteSpace(request.TokenPago))
        {
            throw new IntegrationValidationException("MID-FAC-006", "tokenPago es obligatorio.");
        }
    }
}

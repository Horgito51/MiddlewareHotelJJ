using HotelJJ.API.Models.Responses.Facturacion;
using HotelJJ.API.Models.Responses.Hospedaje;
using HotelJJ.API.Models.Responses.Reservas;

namespace HotelJJ.API.Models.Responses.Flujos;

public class IntegratedBookingResponse
{
    public ReservationResponse Reserva { get; set; } = new();
    public FacturaResponse? FacturaInicial { get; set; }
    public PagoResponse? Pago { get; set; }
    public PagoSimuladoResponse? PagoSimulado { get; set; }
}

public class IntegratedCheckInResponse
{
    public ReservationResponse Reserva { get; set; } = new();
    public EstadiaResponse Estadia { get; set; } = new();
}

public class IntegratedCheckOutResponse
{
    public EstadiaResponse Estadia { get; set; } = new();
    public FacturaResponse? FacturaFinal { get; set; }
}

namespace HotelJJ.DataAccess.Http.Models.Reservas.Responses;

public class ReservaPrecioResponseModel
{
    public Guid HabitacionGuid { get; set; }
    public decimal PrecioNocheAplicado { get; set; }
    public decimal SubtotalLinea { get; set; }
    public decimal ValorIvaLinea { get; set; }
    public decimal TotalLinea { get; set; }
    public string OrigenPrecio { get; set; } = string.Empty;
}

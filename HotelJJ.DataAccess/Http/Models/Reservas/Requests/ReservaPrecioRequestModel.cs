namespace HotelJJ.DataAccess.Http.Models.Reservas.Requests;

public class ReservaPrecioRequestModel
{
    public Guid HabitacionGuid { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string? Canal { get; set; }
}

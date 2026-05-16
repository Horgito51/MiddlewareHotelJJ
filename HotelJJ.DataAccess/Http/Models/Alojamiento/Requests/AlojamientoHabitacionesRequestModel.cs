namespace HotelJJ.DataAccess.Http.Models.Alojamiento.Requests;

public class AlojamientoHabitacionesRequestModel
{
    public Guid? TipoHabitacionGuid { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
}

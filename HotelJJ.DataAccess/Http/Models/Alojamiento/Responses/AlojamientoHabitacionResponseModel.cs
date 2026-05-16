namespace HotelJJ.DataAccess.Http.Models.Alojamiento.Responses;

public class AlojamientoHabitacionResponseModel
{
    public Guid HabitacionGuid { get; set; }
    public Guid TipoHabitacionGuid { get; set; }
    public string TipoNombre { get; set; } = string.Empty;
    public string NumeroHabitacion { get; set; } = string.Empty;
    public int? Piso { get; set; }
    public int CapacidadAdultos { get; set; }
    public int CapacidadNinos { get; set; }
    public decimal PrecioBase { get; set; }
    public string Moneda { get; set; } = "USD";
    public string EstadoHabitacion { get; set; } = string.Empty;
    public bool? DisponibleEnRango { get; set; }
}

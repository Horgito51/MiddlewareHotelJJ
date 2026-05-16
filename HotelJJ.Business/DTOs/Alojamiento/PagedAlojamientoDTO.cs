namespace HotelJJ.Business.DTOs.Alojamiento;

public class PagedAlojamientoDTO<T>
{
    public List<T> Items { get; set; } = new();
    public int Pagina { get; set; }
    public int Limite { get; set; }
    public int TotalResultados { get; set; }
    public int TotalPaginas { get; set; }
    public bool TieneSiguiente { get; set; }
    public bool TieneAnterior { get; set; }
}

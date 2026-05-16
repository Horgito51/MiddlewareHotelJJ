namespace HotelJJ.DataAccess.Http.Models.Alojamiento.Requests;

public class AlojamientoReviewsRequestModel
{
    public int Pagina { get; set; } = 1;
    public int Limite { get; set; } = 10;
}

namespace HotelJJ.API.Models.Requests.Auth;

public class LogoutIntegrationRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}

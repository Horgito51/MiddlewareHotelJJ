namespace HotelJJ.API.Models.Requests.Auth;

public class LoginIntegrationRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

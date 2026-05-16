namespace HotelJJ.Business.DTOs.Auth;

public class TokenIntegrationDTO
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public int? IdCliente { get; set; }
    public Guid? ClienteGuid { get; set; }
    public Guid UsuarioGuid { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public IReadOnlyCollection<string> Roles { get; set; } = [];
}

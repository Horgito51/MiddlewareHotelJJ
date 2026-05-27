namespace HotelJJ.API.Models.Requests.Seguridad;

public class SeguridadInhabilitarRequest
{
    public string Motivo { get; set; } = string.Empty;
}

public class RolUpsertRequest
{
    public string NombreRol { get; set; } = string.Empty;
    public string DescripcionRol { get; set; } = string.Empty;
    public string EstadoRol { get; set; } = "ACT";
    public bool Activo { get; set; } = true;
}

public class RolPermisosUpsertRequest
{
    public List<string> Permisos { get; set; } = new();
}

public class UsuarioCreateRequest
{
    public int? IdCliente { get; set; }
    public Guid? ClienteGuid { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string EstadoUsuario { get; set; } = "ACT";
    public bool Activo { get; set; } = true;
    public int? IdRol { get; set; }
    public Guid? RolGuid { get; set; }
    public List<RolRequest> Roles { get; set; } = new();
}

public class UsuarioUpdateRequest
{
    public string Correo { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string EstadoUsuario { get; set; } = "ACT";
    public bool Activo { get; set; } = true;
    public List<RolRequest> Roles { get; set; } = new();
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class RolRequest
{
    public int IdRol { get; set; }
    public Guid RolGuid { get; set; }
    public string NombreRol { get; set; } = string.Empty;
    public string EstadoRol { get; set; } = string.Empty;
}

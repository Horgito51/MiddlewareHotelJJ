namespace HotelJJ.DataAccess.Http.Routes;

public static class SeguridadRoutes
{
    public const string AuthLogin = "api/v1/auth/login";
    public const string AuthRefresh = "api/v1/auth/refresh";
    public const string AuthRegisterCliente = "api/v1/auth/register-cliente";
    public const string AuthLogout = "api/v1/auth/logout";
    public const string AuthCambiarPassword = "api/v1/auth/cambiar-password";

    public const string InternalAuthLogin = "api/v1/internal/auth/login";
    public const string InternalAuthRefresh = "api/v1/internal/auth/refresh";
    public const string InternalAuthRegisterCliente = "api/v1/internal/auth/register-cliente";
    public const string InternalAuthLogout = "api/v1/internal/auth/logout";
    public const string InternalAuthCambiarPassword = "api/v1/internal/auth/cambiar-password";

    public const string InternalUsuarios = "api/v1/internal/usuarios";
    public const string InternalRoles = "api/v1/internal/roles";
    public const string InternalPermisos = "api/v1/internal/permisos";
    public const string InternalAuditoria = "api/v1/internal/auditoria";
}

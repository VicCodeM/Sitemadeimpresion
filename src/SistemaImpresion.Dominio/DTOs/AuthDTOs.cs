namespace SistemaImpresion.Dominio.DTOs;

/// <summary>
/// DTO para solicitud de inicio de sesión
/// </summary>
public class LoginRequestDTO
{
    /// <summary>
    /// Nombre de usuario
    /// </summary>
    public string NombreUsuario { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña en texto plano (solo en tránsito, HTTPS requerido)
    /// </summary>
    public string Contrasena { get; set; } = string.Empty;
}

/// <summary>
/// DTO para respuesta de inicio de sesión exitoso
/// </summary>
public class LoginResponseDTO
{
    /// <summary>
    /// Token JWT para autenticación
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de expiración del token (UTC)
    /// </summary>
    public DateTime Expiracion { get; set; }

    /// <summary>
    /// Información del usuario autenticado
    /// </summary>
    public UsuarioDTO Usuario { get; set; } = null!;
}

/// <summary>
/// DTO básico de usuario (sin información sensible)
/// </summary>
public class UsuarioDTO
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public bool Activo { get; set; }
}

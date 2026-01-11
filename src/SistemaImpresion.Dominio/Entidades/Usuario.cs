namespace SistemaImpresion.Dominio.Entidades;

/// <summary>
/// Representa un usuario del sistema de administración
/// NO confundir con Empleado (que solicita impresiones desde PC)
/// </summary>
public class Usuario : EntidadBase
{
    #region Propiedades

    /// <summary>
    /// Nombre de usuario único para inicio de sesión
    /// </summary>
    public string NombreUsuario { get; set; } = string.Empty;

    /// <summary>
    /// Hash de la contraseña (BCrypt)
    /// NUNCA almacenar contraseñas en texto plano
    /// </summary>
    public string HashContrasena { get; set; } = string.Empty;

    /// <summary>
    /// Nombre completo del usuario
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del usuario
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del rol asignado
    /// </summary>
    public int RolId { get; set; }

    /// <summary>
    /// Fecha y hora del último acceso al sistema (UTC)
    /// </summary>
    public DateTime? UltimoAcceso { get; set; }

    /// <summary>
    /// Número de intentos fallidos de inicio de sesión consecutivos
    /// Se resetea al iniciar sesión exitosamente
    /// </summary>
    public int IntentosFallidos { get; set; }

    /// <summary>
    /// Indica si la cuenta está bloqueada por múltiples intentos fallidos
    /// </summary>
    public bool Bloqueado { get; set; }

    #endregion

    #region Relaciones

    /// <summary>
    /// Rol asignado a este usuario
    /// </summary>
    public virtual Rol? Rol { get; set; }

    /// <summary>
    /// Auditorías realizadas por este usuario
    /// </summary>
    public virtual ICollection<Auditoria> Auditorias { get; set; } = new List<Auditoria>();

    /// <summary>
    /// Reglas de impresión creadas por este usuario
    /// </summary>
    public virtual ICollection<ReglaImpresion> ReglasCreadas { get; set; } = new List<ReglaImpresion>();

    /// <summary>
    /// Impresiones autorizadas por este usuario (cuando es manual)
    /// </summary>
    public virtual ICollection<Impresion> ImpresionesAutorizadas { get; set; } = new List<Impresion>();

    #endregion
}

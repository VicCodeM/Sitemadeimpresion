using SistemaImpresion.Dominio.Enumeraciones;

namespace SistemaImpresion.Dominio.Entidades;

/// <summary>
/// Representa un rol del sistema que define permisos y niveles de acceso
/// </summary>
public class Rol : EntidadBase
{
    #region Propiedades

    /// <summary>
    /// Nombre único del rol
    /// Ejemplo: "Administrador", "Supervisor", "Operador"
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Descripción detallada del rol y sus responsabilidades
    /// </summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de rol según la enumeración
    /// </summary>
    public RolTipo Tipo { get; set; }

    /// <summary>
    /// Indica si este rol puede solicitar impresiones desde las PCs
    /// </summary>
    public bool PuedeImprimir { get; set; }

    /// <summary>
    /// Indica si este rol puede autorizar o denegar impresiones manualmente
    /// </summary>
    public bool PuedeAutorizar { get; set; }

    /// <summary>
    /// Indica si este rol tiene acceso al panel de administración
    /// </summary>
    public bool PuedeAdministrar { get; set; }

    #endregion

    #region Relaciones

    /// <summary>
    /// Colección de usuarios que tienen este rol asignado
    /// </summary>
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    #endregion
}

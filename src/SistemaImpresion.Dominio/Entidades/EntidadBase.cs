namespace SistemaImpresion.Dominio.Entidades;

/// <summary>
/// Clase base para todas las entidades del sistema
/// Proporciona propiedades comunes de auditoría
/// </summary>
public abstract class EntidadBase
{
    #region Propiedades

    /// <summary>
    /// Identificador único de la entidad
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Fecha y hora de creación del registro (UTC)
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha y hora de la última modificación (UTC)
    /// Null si nunca ha sido modificado
    /// </summary>
    public DateTime? FechaModificacion { get; set; }

    /// <summary>
    /// Indica si el registro está activo o ha sido eliminado lógicamente
    /// </summary>
    public bool Activo { get; set; } = true;

    #endregion
}

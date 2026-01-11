namespace SistemaImpresion.Dominio.Entidades;

/// <summary>
/// Tabla intermedia que relaciona Lotes con Máquinas
/// Permite asignar un lote a múltiples máquinas y viceversa
/// </summary>
public class LoteMaquina : EntidadBase
{
    #region Propiedades

    /// <summary>
    /// Identificador del lote
    /// </summary>
    public int LoteId { get; set; }

    /// <summary>
    /// Identificador de la máquina
    /// </summary>
    public int MaquinaId { get; set; }

    /// <summary>
    /// Fecha en que se asignó este lote a la máquina
    /// </summary>
    public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha en que se desactivó la asignación (null si sigue activa)
    /// </summary>
    public DateTime? FechaDesasignacion { get; set; }

    /// <summary>
    /// Prioridad de este lote en la máquina (menor número = mayor prioridad)
    /// Útil cuando una máquina tiene múltiples lotes asignados
    /// </summary>
    public int Prioridad { get; set; } = 0;

    #endregion

    #region Relaciones

    /// <summary>
    /// Lote asignado
    /// </summary>
    public virtual Lote? Lote { get; set; }

    /// <summary>
    /// Máquina asignada
    /// </summary>
    public virtual Maquina? Maquina { get; set; }

    #endregion
}

namespace SistemaImpresion.Dominio.Entidades;

/// <summary>
/// Representa un lote de producción
/// Define qué etiqueta se imprime y cuántas veces
/// </summary>
public class Lote : EntidadBase
{
    #region Propiedades

    /// <summary>
    /// Número único del lote
    /// </summary>
    public string Numero { get; set; } = string.Empty;

    /// <summary>
    /// Descripción del lote de producción
    /// </summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>
    /// Identificador de la etiqueta que se imprime en este lote
    /// </summary>
    public int EtiquetaId { get; set; }

    /// <summary>
    /// Cantidad máxima de impresiones permitidas para este lote
    /// 0 = sin límite
    /// </summary>
    public int CantidadMaxima { get; set; } = 0;

    /// <summary>
    /// Fecha de inicio del lote
    /// </summary>
    public DateTime FechaInicio { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha de finalización del lote (puede ser null si está activo)
    /// </summary>
    public DateTime? FechaFin { get; set; }

    /// <summary>
    /// Código de producto o referencia externa (opcional)
    /// </summary>
    public string? CodigoProducto { get; set; }

    /// <summary>
    /// Cliente para el que se produce este lote (opcional)
    /// </summary>
    public string? Cliente { get; set; }

    #endregion

    #region Relaciones

    /// <summary>
    /// Etiqueta asignada a este lote
    /// </summary>
    public virtual Etiqueta? Etiqueta { get; set; }

    /// <summary>
    /// Máquinas a las que está asignado este lote
    /// </summary>
    public virtual ICollection<LoteMaquina> MaquinasAsignadas { get; set; } = new List<LoteMaquina>();

    /// <summary>
    /// Historial de impresiones de este lote
    /// </summary>
    public virtual ICollection<Impresion> Impresiones { get; set; } = new List<Impresion>();

    #endregion
}

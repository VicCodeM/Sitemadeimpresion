namespace SistemaImpresion.Dominio.Entidades;

/// <summary>
/// Define las reglas de autorización: qué máquina puede imprimir qué etiqueta
/// Implementa el principio de lista blanca (solo lo autorizado explícitamente)
/// </summary>
public class ReglaImpresion : EntidadBase
{
    #region Propiedades

    /// <summary>
    /// Identificador de la máquina
    /// </summary>
    public int MaquinaId { get; set; }

    /// <summary>
    /// Identificador de la etiqueta
    /// </summary>
    public int EtiquetaId { get; set; }

    /// <summary>
    /// Indica si esta combinación está autorizada
    /// False = explícitamente denegada
    /// </summary>
    public bool Autorizada { get; set; } = true;

    /// <summary>
    /// Límite de impresiones para esta combinación específica
    /// 0 = sin límite (sujeto a límites generales de máquina/lote)
    /// </summary>
    public int LimiteImpresiones { get; set; } = 0;

    /// <summary>
    /// Identificador del usuario que creó esta regla
    /// </summary>
    public int CreadoPorUsuarioId { get; set; }

    /// <summary>
    /// Motivo o justificación de la regla (opcional)
    /// </summary>
    public string? Motivo { get; set; }

    #endregion

    #region Relaciones

    /// <summary>
    /// Máquina a la que aplica la regla
    /// </summary>
    public virtual Maquina? Maquina { get; set; }

    /// <summary>
    /// Etiqueta a la que aplica la regla
    /// </summary>
    public virtual Etiqueta? Etiqueta { get; set; }

    /// <summary>
    /// Usuario que creó la regla
    /// </summary>
    public virtual Usuario? CreadoPor { get; set; }

    #endregion
}

namespace SistemaImpresion.Dominio.Entidades;

/// <summary>
/// Representa una impresora Zebra conectada a una máquina
/// Relación 1:1 con Maquina (una impresora por máquina)
/// </summary>
public class Impresora : EntidadBase
{
    #region Propiedades

    /// <summary>
    /// Código único de la impresora
    /// </summary>
    public string Codigo { get; set; } = string.Empty;

    /// <summary>
    /// Modelo de la impresora Zebra
    /// Ejemplo: "ZT230", "ZT410", "GX420d", etc.
    /// </summary>
    public string Modelo { get; set; } = string.Empty;

    /// <summary>
    /// Número de serie del fabricante
    /// </summary>
    public string NumeroSerie { get; set; } = string.Empty;

    /// <summary>
    /// Puerto USB al que está conectada la impresora
    /// Ejemplo: "USB001", "USB002", etc.
    /// </summary>
    public string PuertoUSB { get; set; } = string.Empty;

    /// <summary>
    /// Identificador de la máquina a la que está asignada
    /// </summary>
    public int MaquinaId { get; set; }

    /// <summary>
    /// Resolución en DPI (dots per inch)
    /// Ejemplo: 203, 300, 600
    /// </summary>
    public int? ResolucionDPI { get; set; }

    /// <summary>
    /// Ancho máximo de impresión en milímetros
    /// </summary>
    public int? AnchoMaximoMM { get; set; }

    /// <summary>
    /// Fecha de instalación de la impresora
    /// </summary>
    public DateTime FechaInstalacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha del último mantenimiento
    /// </summary>
    public DateTime? UltimoMantenimiento { get; set; }

    /// <summary>
    /// Notas adicionales sobre la impresora
    /// </summary>
    public string? Notas { get; set; }

    #endregion

    #region Relaciones

    /// <summary>
    /// Máquina a la que está asignada esta impresora
    /// </summary>
    public virtual Maquina? Maquina { get; set; }

    /// <summary>
    /// Historial de impresiones realizadas en esta impresora
    /// </summary>
    public virtual ICollection<Impresion> Impresiones { get; set; } = new List<Impresion>();

    #endregion
}

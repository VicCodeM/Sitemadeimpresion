using SistemaImpresion.Dominio.Enumeraciones;

namespace SistemaImpresion.Dominio.Entidades;

/// <summary>
/// Representa una solicitud de impresión y su ciclo de vida completo
/// PRINCIPIO: Registrar TODO para auditoría completa
/// </summary>
public class Impresion : EntidadBase
{
    #region Propiedades

    /// <summary>
    /// Identificador de la máquina desde donde se solicitó
    /// </summary>
    public int MaquinaId { get; set; }

    /// <summary>
    /// Identificador de la impresora que ejecutará/ejecutó la impresión
    /// </summary>
    public int ImpresoraId { get; set; }

    /// <summary>
    /// Identificador del empleado que solicitó la impresión
    /// </summary>
    public int EmpleadoId { get; set; }

    /// <summary>
    /// Identificador del lote al que pertenece esta impresión
    /// </summary>
    public int LoteId { get; set; }

    /// <summary>
    /// Identificador de la etiqueta que se imprimió/imprimirá
    /// </summary>
    public int EtiquetaId { get; set; }

    /// <summary>
    /// Cantidad de etiquetas solicitadas
    /// </summary>
    public int Cantidad { get; set; }

    /// <summary>
    /// Estado actual de la impresión
    /// </summary>
    public EstadoImpresion Estado { get; set; } = EstadoImpresion.Solicitada;

    /// <summary>
    /// Fecha y hora en que se solicitó la impresión (UTC)
    /// </summary>
    public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha y hora en que se autorizó la impresión (UTC)
    /// Null si fue denegada o aún no se ha procesado
    /// </summary>
    public DateTime? FechaAutorizacion { get; set; }

    /// <summary>
    /// Fecha y hora en que se ejecutó la impresión en la PC (UTC)
    /// </summary>
    public DateTime? FechaEjecucion { get; set; }

    /// <summary>
    /// Resultado de la impresión
    /// Ejemplo: "Exitosa", "Error de comunicación", "Sin papel"
    /// </summary>
    public string? Resultado { get; set; }

    /// <summary>
    /// Mensaje de error detallado si la impresión falló
    /// </summary>
    public string? MensajeError { get; set; }

    /// <summary>
    /// Motivo de denegación si aplica
    /// </summary>
    public string? MotivoDenegacion { get; set; }

    /// <summary>
    /// Usuario que autorizó manualmente (si aplica)
    /// Null si fue autorización automática del motor de reglas
    /// </summary>
    public int? AutorizadoPorUsuarioId { get; set; }

    /// <summary>
    /// Dirección IP de la PC desde donde se solicitó
    /// </summary>
    public string? DireccionIPOrigen { get; set; }

    /// <summary>
    /// Hash del contenido ZPL enviado (para verificación)
    /// </summary>
    public string? HashZPL { get; set; }

    #endregion

    #region Relaciones

    /// <summary>
    /// Máquina desde donde se solicitó
    /// </summary>
    public virtual Maquina? Maquina { get; set; }

    /// <summary>
    /// Impresora utilizada
    /// </summary>
    public virtual Impresora? Impresora { get; set; }

    /// <summary>
    /// Empleado que solicitó
    /// </summary>
    public virtual Empleado? Empleado { get; set; }

    /// <summary>
    /// Lote al que pertenece
    /// </summary>
    public virtual Lote? Lote { get; set; }

    /// <summary>
    /// Etiqueta impresa
    /// </summary>
    public virtual Etiqueta? Etiqueta { get; set; }

    /// <summary>
    /// Usuario que autorizó manualmente (si aplica)
    /// </summary>
    public virtual Usuario? AutorizadoPor { get; set; }

    #endregion
}

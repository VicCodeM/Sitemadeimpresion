using SistemaImpresion.Dominio.Enumeraciones;

namespace SistemaImpresion.Dominio.Entidades;

/// <summary>
/// Registra todas las acciones administrativas en el sistema
/// Cumplimiento de auditoría IT y trazabilidad completa
/// </summary>
public class Auditoria : EntidadBase
{
    #region Propiedades

    /// <summary>
    /// Identificador del usuario que realizó la acción
    /// Null si fue una acción del sistema
    /// </summary>
    public int? UsuarioId { get; set; }

    /// <summary>
    /// Tipo de acción realizada
    /// </summary>
    public TipoAccionAuditoria  TipoAccion { get; set; }

    /// <summary>
    /// Nombre de la entidad afectada
    /// Ejemplo: "Maquina", "Usuario", "Lote"
    /// </summary>
    public string Entidad { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del registro afectado
    /// </summary>
    public int? EntidadId { get; set; }

    /// <summary>
    /// Valores anteriores en formato JSON (para modificaciones)
    /// </summary>
    public string? ValoresAnteriores { get; set; }

    /// <summary>
    /// Valores nuevos en formato JSON (para creaciones/modificaciones)
    /// </summary>
    public string? ValoresNuevos { get; set; }

    /// <summary>
    /// Dirección IP desde donde se realizó la acción
    /// </summary>
    public string? DireccionIP { get; set; }

    /// <summary>
    /// User-Agent del navegador (si aplica)
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// Fecha y hora de la acción (UTC)
    /// </summary>
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Descripción adicional o comentarios
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Indica si la acción fue exitosa
    /// </summary>
    public bool Exitosa { get; set; } = true;

    /// <summary>
    /// Mensaje de error si la acción falló
    /// </summary>
    public string? MensajeError { get; set; }

    #endregion

    #region Relaciones

    /// <summary>
    /// Usuario que realizó la acción
    /// </summary>
    public virtual Usuario? Usuario { get; set; }

    #endregion
}

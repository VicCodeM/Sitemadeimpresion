namespace SistemaImpresion.Dominio.DTOs;

/// <summary>
/// DTO para solicitar una impresión desde la aplicación de producción
/// </summary>
public class SolicitudImpresionDTO
{
    /// <summary>
    /// Identificador de la máquina (hostname o MAC)
    /// </summary>
    public string IdentificadorMaquina { get; set; } = string.Empty;

    /// <summary>
    /// Número de empleado que solicita
    /// </summary>
    public string NumeroEmpleado { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad de etiquetas a imprimir
    /// </summary>
    public int Cantidad { get; set; }

    /// <summary>
    /// Dirección IP de origen (opcional, se puede obtener del request)
    /// </summary>
    public string? DireccionIP { get; set; }
}

/// <summary>
/// DTO para respuesta de solicitud autorizada
/// </summary>
public class RespuestaAutorizacionDTO
{
    /// <summary>
    /// Indica si la impresión fue autorizada
    /// </summary>
    public bool Autorizada { get; set; }

    /// <summary>
    /// Identificador único de la impresión (para confirmación posterior)
    /// </summary>
    public int? ImpresionId { get; set; }

    /// <summary>
    /// Contenido ZPL a enviar a la impresora (solo si autorizada)
    /// </summary>
    public string? ContenidoZPL { get; set; }

    /// <summary>
    /// Cantidad autorizada
    /// </summary>
    public int? Cantidad { get; set; }

    /// <summary>
    /// Motivo de denegación (solo si no autorizada)
    /// </summary>
    public string? MotivoDenegacion { get; set; }

    /// <summary>
    /// Mensaje para mostrar al operador
    /// </summary>
    public string Mensaje { get; set; } = string.Empty;
}

/// <summary>
/// DTO para confirmar ejecución de impresión
/// </summary>
public class ConfirmacionImpresionDTO
{
    /// <summary>
    /// Identificador de la impresión (recibido en la autorización)
    /// </summary>
    public int ImpresionId { get; set; }

    /// <summary>
    /// Indica si la impresión fue exitosa
    /// </summary>
    public bool Exitosa { get; set; }

    /// <summary>
    /// Resultado detallado
    /// </summary>
    public string? Resultado { get; set; }

    /// <summary>
    /// Mensaje de error (si aplica)
    /// </summary>
    public string? MensajeError { get; set; }

    /// <summary>
    /// Fecha y hora de ejecución (UTC)
    /// </summary>
    public DateTime FechaEjecucion { get; set; } = DateTime.UtcNow;
}

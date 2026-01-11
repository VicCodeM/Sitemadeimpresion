namespace SistemaImpresion.Dominio.Enumeraciones;

/// <summary>
/// Estados posibles de una solicitud de impresión
/// Permite trazabilidad completa del ciclo de vida
/// </summary>
public enum EstadoImpresion
{
    /// <summary>
    /// Solicitud recibida, pendiente de validación
    /// </summary>
    Solicitada = 1,

    /// <summary>
    /// Solicitud validada y autorizada por el sistema
    /// </summary>
    Autorizada = 2,

    /// <summary>
    /// Impresión ejecutada exitosamente
    /// </summary>
    Ejecutada = 3,

    /// <summary>
    /// Impresión ejecutada pero con errores
    /// </summary>
    Fallida = 4,

    /// <summary>
    /// Solicitud denegada por el motor de reglas
    /// </summary>
    Denegada = 5
}

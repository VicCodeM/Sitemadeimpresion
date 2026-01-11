using SistemaImpresion.Dominio.Entidades;

namespace SistemaImpresion.Dominio.Interfaces;

/// <summary>
/// Interfaz del servicio de motor de reglas de impresión
/// Responsable de validar y autorizar solicitudes de impresión
/// </summary>
public interface IMotorReglasImpresion
{
    /// <summary>
    /// Evalúa si una solicitud de impresión debe ser autorizada
    /// Implementa TODA la lógica de validación según el pseudocódigo definido
    /// </summary>
    /// <param name="solicitud">Datos de la solicitud</param>
    /// <returns>Resultado de la evaluación con detalles</returns>
    Task<ResultadoAutorizacion> EvaluarAutorizacionAsync(
        string identificadorMaquina,
        string numeroEmpleado,
        int cantidad,
        string? direccionIP = null);
}

/// <summary>
/// Resultado de la evaluación del motor de reglas
/// </summary>
public class ResultadoAutorizacion
{
    /// <summary>
    /// Indica si la impresión fue autorizada
    /// </summary>
    public bool Autorizada { get; set; }

    /// <summary>
    /// Motivo de denegación (si aplica)
    /// </summary>
    public string? MotivoDenegacion { get; set; }

    /// <summary>
    /// Impresión creada (con ID asignado)
    /// </summary>
    public Impresion? Impresion { get; set; }

    /// <summary>
    /// Contenido ZPL autorizado para enviar (solo si autorizada)
    /// </summary>
    public string? ContenidoZPL { get; set; }

    /// <summary>
    /// Máquina identificada
    /// </summary>
    public Maquina? Maquina { get; set; }

    /// <summary>
    /// Empleado identificado
    /// </summary>
    public Empleado? Empleado { get; set; }

    /// <summary>
    /// Lote activo encontrado
    /// </summary>
    public Lote? Lote { get; set; }

    /// <summary>
    /// Etiqueta a imprimir
    /// </summary>
    public Etiqueta? Etiqueta { get; set; }
}

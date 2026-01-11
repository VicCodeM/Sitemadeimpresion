namespace SistemaImpresion.Dominio.Entidades;

/// <summary>
/// Representa un empleado de planta que solicita impresiones
/// DIFERENTE de Usuario (que administra el sistema)
/// </summary>
public class Empleado : EntidadBase
{
    #region Propiedades

    /// <summary>
    /// Número único de empleado asignado por la empresa
    /// Se usa para identificar al empleado al solicitar impresiones
    /// </summary>
    public string NumeroEmpleado { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del empleado
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Apellido del empleado
    /// </summary>
    public string Apellido { get; set; } = string.Empty;

    /// <summary>
    /// Departamento al que pertenece el empleado
    /// </summary>
    public string Departamento { get; set; } = string.Empty;

    /// <summary>
    /// Puesto o cargo del empleado
    /// </summary>
    public string? Puesto { get; set; }

    /// <summary>
    /// Fecha de alta del empleado en el sistema
    /// </summary>
    public DateTime FechaAlta { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Nombre completo calculado (solo lectura)
    /// </summary>
    public string NombreCompleto => $"{Nombre} {Apellido}";

    #endregion

    #region Relaciones

    /// <summary>
    /// Historial de impresiones solicitadas por este empleado
    /// </summary>
    public virtual ICollection<Impresion> Impresiones { get; set; } = new List<Impresion>();

    #endregion
}

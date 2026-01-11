namespace SistemaImpresion.Dominio.Enumeraciones;

/// <summary>
/// Tipos de acciones que se registran en la auditoría
/// Permite clasificar los eventos del sistema
/// </summary>
public enum TipoAccionAuditoria
{
    /// <summary>
    /// Creación de un nuevo registro
    /// </summary>
    Crear = 1,

    /// <summary>
    /// Modificación de un registro existente
    /// </summary>
    Modificar = 2,

    /// <summary>
    /// Eliminación de un registro
    /// </summary>
    Eliminar = 3,

    /// <summary>
    /// Inicio de sesión en el sistema
    /// </summary>
    InicioSesion = 4,

    /// <summary>
    /// Cierre de sesión del sistema
    /// </summary>
    CierreSesion = 5,

    /// <summary>
    /// Autorización de impresión
    /// </summary>
    AutorizarImpresion = 6,

    /// <summary>
    /// Denegación de impresión
    /// </summary>
    DenegarImpresion = 7,

    /// <summary>
    /// Consulta de información sensible
    /// </summary>
    Consultar = 8
}

namespace SistemaImpresion.Dominio.Enumeraciones;

/// <summary>
/// Tipos de roles disponibles en el sistema
/// Define los niveles de acceso y permisos
/// </summary>
public enum RolTipo
{
    /// <summary>
    /// Operador de planta - Solo puede imprimir desde PC
    /// </summary>
    Operador = 1,

    /// <summary>
    /// Supervisor - Puede autorizar impresiones y consultar reportes
    /// </summary>
    Supervisor = 2,

    /// <summary>
    /// Administrador - Control total del sistema
    /// </summary>
    Administrador = 3
}

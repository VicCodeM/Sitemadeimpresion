namespace SistemaImpresion.Dominio.Interfaces;

/// <summary>
/// Interfaz base para repositorios genéricos
/// Implementa patrón Repository para abstracción de acceso a datos
/// </summary>
/// <typeparam name="T">Tipo de entidad</typeparam>
public interface IRepositorioBase<T> where T : class
{
    /// <summary>
    /// Obtener todas las entidades
    /// </summary>
    Task<IEnumerable<T>> ObtenerTodosAsync();

    /// <summary>
    /// Obtener entidad por ID
    /// </summary>
    Task<T?> ObtenerPorIdAsync(int id);

    /// <summary>
    /// Agregar nueva entidad
    /// </summary>
    Task<T> AgregarAsync(T entidad);

    /// <summary>
    /// Actualizar entidad existente
    /// </summary>
    Task ActualizarAsync(T entidad);

    /// <summary>
    /// Eliminar entidad (puede ser lógico o físico según implementación)
    /// </summary>
    Task EliminarAsync(int id);

    /// <summary>
    /// Guardar cambios en la base de datos
    /// </summary>
    Task<int> GuardarCambiosAsync();
}

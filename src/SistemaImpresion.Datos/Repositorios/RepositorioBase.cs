using Microsoft.EntityFrameworkCore;
using SistemaImpresion.Datos.Contexto;
using SistemaImpresion.Dominio.Entidades;
using SistemaImpresion.Dominio.Interfaces;

namespace SistemaImpresion.Datos.Repositorios;

/// <summary>
/// Implementación base de repositorio genérico
/// Proporciona operaciones CRUD comunes para todas las entidades
/// </summary>
/// <typeparam name="T">Tipo de entidad que hereda de EntidadBase</typeparam>
public class RepositorioBase<T> : IRepositorioBase<T> where T : EntidadBase
{
    #region Campos Privados

    protected readonly SistemaImpresionDbContext _contexto;
    protected readonly DbSet<T> _dbSet;

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor del repositorio base
    /// </summary>
    public RepositorioBase(SistemaImpresionDbContext contexto)
    {
        _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        _dbSet = contexto.Set<T>();
    }

    #endregion

    #region Implementación de IRepositorioBase

    /// <summary>
    /// Obtiene todas las entidades activas
    /// </summary>
    public virtual async Task<IEnumerable<T>> ObtenerTodosAsync()
    {
        return await _dbSet
            .Where(e => e.Activo)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene una entidad por su ID
    /// </summary>
    public virtual async Task<T?> ObtenerPorIdAsync(int id)
    {
        return await _dbSet
            .FirstOrDefaultAsync(e => e.Id == id && e.Activo);
    }

    /// <summary>
    /// Agrega una nueva entidad
    /// </summary>
    public virtual async Task<T> AgregarAsync(T entidad)
    {
        if (entidad == null)
            throw new ArgumentNullException(nameof(entidad));

        await _dbSet.AddAsync(entidad);
        return entidad;
    }

    /// <summary>
    /// Actualiza una entidad existente
    /// </summary>
    public virtual Task ActualizarAsync(T entidad)
    {
        if (entidad == null)
            throw new ArgumentNullException(nameof(entidad));

        _dbSet.Update(entidad);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Elimina una entidad (eliminación lógica por defecto)
    /// </summary>
    public virtual async Task EliminarAsync(int id)
    {
        var entidad = await _dbSet.FindAsync(id);
        if (entidad != null)
        {
            // Eliminación lógica
            entidad.Activo = false;
            entidad.FechaModificacion = DateTime.UtcNow;
            _dbSet.Update(entidad);
        }
    }

    /// <summary>
    /// Guarda todos los cambios pendientes en la base de datos
    /// </summary>
    public virtual async Task<int> GuardarCambiosAsync()
    {
        return await _contexto.SaveChangesAsync();
    }

    #endregion

    #region Métodos Protegidos Auxiliares

    /// <summary>
    /// Incluye propiedades de navegación para evitar lazy loading
    /// </summary>
    protected virtual IQueryable<T> IncluirRelaciones(IQueryable<T> query)
    {
        // Las clases derivadas pueden sobrescribir para incluir relaciones específicas
        return query;
    }

    #endregion
}

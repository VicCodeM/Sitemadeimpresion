using Microsoft.EntityFrameworkCore;
using SistemaImpresion.Dominio.Entidades;

namespace SistemaImpresion.Datos.Contexto;

/// <summary>
/// Contexto principal de Entity Framework Core
/// Representa la sesión con la base de datos PostgreSQL
/// </summary>
public class SistemaImpresionDbContext : DbContext
{
    #region Constructor

    /// <summary>
    /// Constructor del contexto
    /// </summary>
    public SistemaImpresionDbContext(DbContextOptions<SistemaImpresionDbContext> options)
        : base(options)
    {
    }

    #endregion

    #region DbSets - Tablas de la base de datos

    /// <summary>
    /// Tabla de roles
    /// </summary>
    public DbSet<Rol> Roles { get; set; }

    /// <summary>
    /// Tabla de usuarios del sistema
    /// </summary>
    public DbSet<Usuario> Usuarios { get; set; }

    /// <summary>
    /// Tabla de empleados de planta
    /// </summary>
    public DbSet<Empleado> Empleados { get; set; }

    /// <summary>
    /// Tabla de máquinas (PCs de producción)
    /// </summary>
    public DbSet<Maquina> Maquinas { get; set; }

    /// <summary>
    /// Tabla de impresoras Zebra
    /// </summary>
    public DbSet<Impresora> Impresoras { get; set; }

    /// <summary>
    /// Tabla de etiquetas ZPL
    /// </summary>
    public DbSet<Etiqueta> Etiquetas { get; set; }

    /// <summary>
    /// Tabla de lotes de producción
    /// </summary>
    public DbSet<Lote> Lotes { get; set; }

    /// <summary>
    /// Tabla intermedia lotes-máquinas
    /// </summary>
    public DbSet<LoteMaquina> LotesMaquinas { get; set; }

    /// <summary>
    /// Tabla de reglas de impresión (autorizaciones)
    /// </summary>
    public DbSet<ReglaImpresion> ReglasImpresion { get; set; }

    /// <summary>
    /// Tabla de impresiones (historial completo)
    /// </summary>
    public DbSet<Impresion> Impresiones { get; set; }

    /// <summary>
    /// Tabla de auditoría
    /// </summary>
    public DbSet<Auditoria> Auditorias { get; set; }

    #endregion

    #region Configuración del Modelo

    /// <summary>
    /// Configuración del modelo de datos
    /// Aplica las configuraciones de cada entidad
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar todas las configuraciones del ensamblado actual
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SistemaImpresionDbContext).Assembly);

        // Configuración adicional si es necesaria
        ConfigurarIndices(modelBuilder);
        ConfigurarConstraints(modelBuilder);
    }

    /// <summary>
    /// Configura índices para optimizar consultas
    /// </summary>
    private void ConfigurarIndices(ModelBuilder modelBuilder)
    {
        // Índice único en NombreUsuario
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.NombreUsuario)
            .IsUnique();

        // Índice único en NumeroEmpleado
        modelBuilder.Entity<Empleado>()
            .HasIndex(e => e.NumeroEmpleado)
            .IsUnique();

        // Índice único en Codigo de Maquina
        modelBuilder.Entity<Maquina>()
            .HasIndex(m => m.Codigo)
            .IsUnique();

        // Índice en IdentificadorRed para búsquedas rápidas
        modelBuilder.Entity<Maquina>()
            .HasIndex(m => m.IdentificadorRed);

        // Índice en DireccionMAC para búsquedas rápidas
        modelBuilder.Entity<Maquina>()
            .HasIndex(m => m.DireccionMAC);

        // Índice único en Codigo de Impresora
        modelBuilder.Entity<Impresora>()
            .HasIndex(i => i.Codigo)
            .IsUnique();

        // Índice único en Codigo de Etiqueta
        modelBuilder.Entity<Etiqueta>()
            .HasIndex(e => e.Codigo)
            .IsUnique();

        // Índice único en Numero de Lote
        modelBuilder.Entity<Lote>()
            .HasIndex(l => l.Numero)
            .IsUnique();

        // Índice compuesto en Impresiones para reportes
        modelBuilder.Entity<Impresion>()
            .HasIndex(i => new { i.FechaSolicitud, i.MaquinaId, i.Estado });

        // Índice en Auditorias para búsquedas
        modelBuilder.Entity<Auditoria>()
            .HasIndex(a => new { a.Fecha, a.UsuarioId, a.TipoAccion });
    }

    /// <summary>
    /// Configura restricciones adicionales
    /// </summary>
    private void ConfigurarConstraints(ModelBuilder modelBuilder)
    {
        // Relación 1:1 entre Maquina e Impresora
        modelBuilder.Entity<Impresora>()
            .HasOne(i => i.Maquina)
            .WithOne(m => m.Impresora)
            .HasForeignKey<Impresora>(i => i.MaquinaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Eliminar en cascada lógica (soft delete) para la mayoría de relaciones
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    #endregion

    #region Sobrescrituras para Auditoría Automática

    /// <summary>
    /// Intercepta SaveChanges para aplicar lógica de auditoría automática
    /// </summary>
    public override int SaveChanges()
    {
        AplicarAuditoria();
        return base.SaveChanges();
    }

    /// <summary>
    /// Intercepta SaveChangesAsync para aplicar lógica de auditoría automática
    /// </summary>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AplicarAuditoria();
        return base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Aplica timestamps automáticos a entidades
    /// </summary>
    private void AplicarAuditoria()
    {
        var entradas = ChangeTracker.Entries<EntidadBase>();

        foreach (var entrada in entradas)
        {
            if (entrada.State == EntityState.Added)
            {
                entrada.Entity.FechaCreacion = DateTime.UtcNow;
            }
            else if (entrada.State == EntityState.Modified)
            {
                entrada.Entity.FechaModificacion = DateTime.UtcNow;
            }
        }
    }

    #endregion
}

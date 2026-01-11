using Microsoft.EntityFrameworkCore;
using SistemaImpresion.Datos.Contexto;
using SistemaImpresion.Datos.Utilidades;
using SistemaImpresion.Dominio.Entidades;
using SistemaImpresion.Dominio.Enumeraciones;

namespace SistemaImpresion.Datos;

/// <summary>
/// Clase para inicializar datos semilla en la base de datos
/// Crea roles y usuario administrador inicial
/// </summary>
public static class InicializadorDatos
{
    #region Métodos Públicos

    /// <summary>
    /// Inicializa los datos semilla necesarios para el sistema
    /// </summary>
    public static async Task InicializarAsync(SistemaImpresionDbContext contexto)
    {
        await InicializarRolesAsync(contexto);
        await InicializarUsuarioAdminAsync(contexto);
    }

    #endregion

    #region Métodos Privados

    /// <summary>
    /// Crea los roles predeterminados del sistema
    /// </summary>
    private static async Task InicializarRolesAsync(SistemaImpresionDbContext contexto)
    {
        // Verificar si ya existen roles
        if (await contexto.Roles.AnyAsync())
            return;

        var roles = new List<Rol>
        {
            new Rol
            {
                Nombre = "Administrador",
                Descripcion = "Control total del sistema",
                Tipo = RolTipo.Administrador,
                PuedeImprimir = false,
                PuedeAutorizar = true,
                PuedeAdministrar = true,
                Activo = true
            },
            new Rol
            {
                Nombre = "Supervisor",
                Descripcion = "Puede autorizar impresiones y consultar reportes",
                Tipo = RolTipo.Supervisor,
                PuedeImprimir = false,
                PuedeAutorizar = true,
                PuedeAdministrar = false,
                Activo = true
            },
            new Rol
            {
                Nombre = "Operador",
                Descripcion = "Solo puede imprimir desde PC de producción",
                Tipo = RolTipo.Operador,
                PuedeImprimir = true,
                PuedeAutorizar = false,
                PuedeAdministrar = false,
                Activo = true
            }
        };

        await contexto.Roles.AddRangeAsync(roles);
        await contexto.SaveChangesAsync();
    }

    /// <summary>
    /// Crea el usuario administrador inicial
    /// </summary>
    private static async Task InicializarUsuarioAdminAsync(SistemaImpresionDbContext contexto)
    {
        // Verificar si ya existe el usuario admin
        if (await contexto.Usuarios.AnyAsync(u => u.NombreUsuario == "admin"))
            return;

        // Obtener el rol de administrador
        var rolAdmin = await contexto.Roles
            .FirstOrDefaultAsync(r => r.Tipo == RolTipo.Administrador);

        if (rolAdmin == null)
            return;

        // Crear usuario administrador con contraseña: Admin123
        var usuarioAdmin = new Usuario
        {
            NombreUsuario = "admin",
            HashContrasena = UtilidadContrasenas.GenerarHash("Admin123"),
            Nombre = "Administrador del Sistema",
            Email = "admin@sistema.local",
            RolId = rolAdmin.Id,
            Activo = true,
            Bloqueado = false,
            IntentosFallidos = 0
        };

        await contexto.Usuarios.AddAsync(usuarioAdmin);
        await contexto.SaveChangesAsync();
    }

    #endregion
}

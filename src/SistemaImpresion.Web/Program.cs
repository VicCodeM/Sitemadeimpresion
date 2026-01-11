using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SistemaImpresion.Datos;
using SistemaImpresion.Datos.Contexto;

namespace SistemaImpresion.Web;

/// <summary>
/// Punto de entrada de la aplicación Web de Administración
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        // Habilitar compatibilidad con DateTime de Kind=Unspecified para Npgsql/PostgreSQL
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var builder = WebApplication.CreateBuilder(args);

        // ============================================
        // CONFIGURACIÓN DE SERVICIOS
        // ============================================

        #region Base de Datos

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? "Host=localhost;Database=sistema_impresion;Username=postgres;Password=postgres";

        builder.Services.AddDbContext<SistemaImpresionDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            if (builder.Environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        #endregion

        #region MVC y Vistas

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        #endregion

        #region Autenticación

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.LogoutPath = "/Auth/Logout";
                options.AccessDeniedPath = "/Auth/AccesoDenegado";
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;
            });

        builder.Services.AddAuthorization();

        #endregion

        #region Sesiones

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromHours(8);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        #endregion

        // ============================================
        // CONSTRUCCIÓN DE LA APLICACIÓN
        // ============================================

        var app = builder.Build();

        // ============================================
        // CONFIGURACIÓN DEL PIPELINE HTTP
        // ============================================

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSession();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapRazorPages();

        // ============================================
        // INICIALIZACIÓN DE DATOS
        // ============================================

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SistemaImpresionDbContext>();
            try
            {
                // Aplicar migraciones en desarrollo
                if (app.Environment.IsDevelopment())
                {
                    if (dbContext.Database.GetPendingMigrations().Any())
                    {
                        app.Logger.LogInformation("Aplicando migraciones...");
                        dbContext.Database.Migrate();
                    }
                }

                // Inicializar datos semilla
                await InicializadorDatos.InicializarAsync(dbContext);
                app.Logger.LogInformation("Datos iniciales verificados");
            }
            catch (Exception ex)
            {
                app.Logger.LogError(ex, "Error al inicializar base de datos");
            }
        }

        app.Logger.LogInformation("==============================================");
        app.Logger.LogInformation("Sistema de Impresión Zebra - Web Admin");
        app.Logger.LogInformation("Usuario: admin / Contraseña: Admin123");
        app.Logger.LogInformation("==============================================");

        await app.RunAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using SistemaImpresion.Datos.Contexto;
using SistemaImpresion.Dominio.Interfaces;
using SistemaImpresion.Negocio.MotorReglas;
using SistemaImpresion.Datos;

namespace SistemaImpresion.API;

/// <summary>
/// Punto de entrada de la aplicación API
/// Configura servicios, middleware y pipeline
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

        // Configurar PostgreSQL con Entity Framework Core
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? "Host=localhost;Database=sistema_impresion;Username=postgres;Password=6433";

        builder.Services.AddDbContext<SistemaImpresionDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            // En desarrollo, habilitar logging detallado
            if (builder.Environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        #endregion

        #region Inyección de Dependencias

        // Registrar Motor de Reglas
        builder.Services.AddScoped<IMotorReglasImpresion, MotorReglasImpresion>();

        // Registrar Repositorios Base (se pueden agregar repositorios específicos después)
        // builder.Services.AddScoped(typeof(IRepositorioBase<>), typeof(RepositorioBase<>));

        #endregion

        #region Controladores y API

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Configurar serialización JSON
                options.JsonSerializerOptions.PropertyNamingPolicy = null; // Mantener PascalCase
                options.JsonSerializerOptions.WriteIndented = true; // JSON legible
            });

        // Configurar Swagger/OpenAPI para documentación
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        #endregion

        #region CORS

        // Configurar CORS para permitir acceso desde la aplicación web admin
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowWebAdmin", policy =>
            {
                policy.WithOrigins("https://localhost:5000", "http://localhost:5000")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });

        #endregion

        #region Logging

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();

        // TODO: Agregar logging a archivo en producción
        // builder.Logging.AddFile("logs/api_{Date}.log");

        #endregion

        // ============================================
        // CONSTRUCCIÓN DE LA APLICACIÓN
        // ============================================

        var app = builder.Build();

        // ============================================
        // CONFIGURACIÓN DEL PIPELINE HTTP
        // ============================================

        #region Middleware

        // Swagger solo en desarrollo
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                options.RoutePrefix = string.Empty; // Swagger en la raíz
            });
        }

        // HTTPS Redirection
        app.UseHttpsRedirection();

        // CORS
        app.UseCors("AllowWebAdmin");

        // Autenticación y Autorización (se configurará JWT después)
        // app.UseAuthentication();
        // app.UseAuthorization();

        // Mapear controladores
        app.MapControllers();

        #endregion

        #region Inicialización de Base de Datos

        // Aplicar migraciones automáticamente en desarrollo
        if (app.Environment.IsDevelopment())
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<SistemaImpresionDbContext>();
                try
                {
                    // Verificar conexión
                    dbContext.Database.CanConnect();
                    app.Logger.LogInformation("Conexión a base de datos exitosa");

                    // Aplicar migraciones pendientes
                    if (dbContext.Database.GetPendingMigrations().Any())
                    {
                        app.Logger.LogInformation("Aplicando migraciones pendientes...");
                        dbContext.Database.Migrate();
                        app.Logger.LogInformation("Migraciones aplicadas correctamente");
                    }

                    // Inicializar datos de prueba para VICTOR-HP
                    await SeederPruebas.SeedAsync(dbContext, "VICTOR-HP");
                    app.Logger.LogInformation("Datos de prueba para VICTOR-HP configurados");
                }
                catch (Exception ex)
                {
                    app.Logger.LogError(ex, "Error al inicializar la base de datos");
                    // No lanzar excepción para permitir que la aplicación inicie
                    // En producción, esto podría ser crítico
                }
            }
        }

        #endregion

        #region Información de Inicio

        app.Logger.LogInformation("==============================================");
        app.Logger.LogInformation("Sistema de Impresión Zebra - API Backend");
        app.Logger.LogInformation("Ambiente: {Environment}", app.Environment.EnvironmentName);
        app.Logger.LogInformation("==============================================");

        #endregion

        // Iniciar la aplicación
        app.Run();
    }
}

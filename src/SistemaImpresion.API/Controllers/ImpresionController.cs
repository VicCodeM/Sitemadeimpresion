using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaImpresion.Datos.Contexto;
using SistemaImpresion.Dominio.DTOs;
using SistemaImpresion.Dominio.Interfaces;

namespace SistemaImpresion.API.Controllers;

/// <summary>
/// Controlador para operaciones de impresión
/// Usado por la aplicación de escritorio de producción
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ImpresionController : ControllerBase
{
    #region Campos Privados

    private readonly IMotorReglasImpresion _motorReglas;
    private readonly SistemaImpresionDbContext _contexto;
    private readonly ILogger<ImpresionController> _logger;

    #endregion

    #region Constructor

    public ImpresionController(
        IMotorReglasImpresion motorReglas,
        SistemaImpresionDbContext contexto,
        ILogger<ImpresionController> logger)
    {
        _motorReglas = motorReglas ?? throw new ArgumentNullException(nameof(motorReglas));
        _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #endregion

    #region Endpoints

    /// <summary>
    /// Verifica el estado del servidor (health check)
    /// </summary>
    /// <returns>Estado del servidor</returns>
    [HttpGet("health")]
    [ProducesResponseType(typeof(RespuestaDTO), StatusCodes.Status200OK)]
    public ActionResult<RespuestaDTO> HealthCheck()
    {
        return Ok(new RespuestaDTO
        {
            Exitosa = true,
            Mensaje = "API funcionando correctamente"
        });
    }

    /// <summary>
    /// Solicita autorización para imprimir
    /// </summary>
    /// <param name="solicitud">Datos de la solicitud</param>
    /// <returns>Respuesta con autorización o denegación</returns>
    [HttpPost("solicitar")]
    [ProducesResponseType(typeof(RespuestaAutorizacionDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespuestaAutorizacionDTO), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RespuestaAutorizacionDTO>> SolicitarImpresion(
        [FromBody] SolicitudImpresionDTO solicitud)
    {
        try
        {
            // Registrar solicitud
            _logger.LogInformation(
                "Solicitud de impresión: Máquina={Maquina}, Empleado={Empleado}, Cantidad={Cantidad}",
                solicitud.IdentificadorMaquina,
                solicitud.NumeroEmpleado,
                solicitud.Cantidad);

            // Validar datos básicos
            if (string.IsNullOrWhiteSpace(solicitud.IdentificadorMaquina))
                return BadRequest(new RespuestaAutorizacionDTO
                {
                    Autorizada = false,
                    MotivoDenegacion = "Identificador de máquina requerido",
                    Mensaje = "Error: Identificador de máquina no proporcionado"
                });

            if (string.IsNullOrWhiteSpace(solicitud.NumeroEmpleado))
                return BadRequest(new RespuestaAutorizacionDTO
                {
                    Autorizada = false,
                    MotivoDenegacion = "Número de empleado requerido",
                    Mensaje = "Error: Número de empleado no proporcionado"
                });

            if (solicitud.Cantidad <= 0)
                return BadRequest(new RespuestaAutorizacionDTO
                {
                    Autorizada = false,
                    MotivoDenegacion = "Cantidad inválida",
                    Mensaje = "Error: La cantidad debe ser mayor a cero"
                });

            // Obtener IP del cliente
            var direccionIP = solicitud.DireccionIP ?? HttpContext.Connection.RemoteIpAddress?.ToString();

            // Evaluar con el motor de reglas
            var resultado = await _motorReglas.EvaluarAutorizacionAsync(
                solicitud.IdentificadorMaquina,
                solicitud.NumeroEmpleado,
                solicitud.Cantidad,
                direccionIP);

            // Preparar respuesta
            var respuesta = new RespuestaAutorizacionDTO
            {
                Autorizada = resultado.Autorizada,
                ImpresionId = resultado.Impresion?.Id,
                ContenidoZPL = resultado.ContenidoZPL,
                Cantidad = resultado.Autorizada ? solicitud.Cantidad : null,
                MotivoDenegacion = resultado.MotivoDenegacion,
                Mensaje = resultado.Autorizada
                    ? $"Impresión autorizada: {solicitud.Cantidad} etiqueta(s)"
                    : $"Impresión denegada: {resultado.MotivoDenegacion}"
            };

            // Registrar resultado
            if (resultado.Autorizada)
            {
                _logger.LogInformation(
                    "Impresión AUTORIZADA - ID={Id}, Máquina={Maquina}, Empleado={Empleado}",
                    resultado.Impresion?.Id,
                    resultado.Maquina?.Codigo,
                    resultado.Empleado?.NumeroEmpleado);

                return Ok(respuesta);
            }
            else
            {
                _logger.LogWarning(
                    "Impresión DENEGADA - Máquina={Maquina}, Empleado={Empleado}, Motivo={Motivo}",
                    solicitud.IdentificadorMaquina,
                    solicitud.NumeroEmpleado,
                    resultado.MotivoDenegacion);

                return StatusCode(StatusCodes.Status403Forbidden, respuesta);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar solicitud de impresión");
            return StatusCode(StatusCodes.Status500InternalServerError, new RespuestaAutorizacionDTO
            {
                Autorizada = false,
                MotivoDenegacion = "Error interno del sistema",
                Mensaje = "Error interno del servidor. Por favor, contacte al administrador."
            });
        }
    }

    /// <summary>
    /// Confirma la ejecución de una impresión autorizada
    /// </summary>
    /// <param name="confirmacion">Datos de confirmación</param>
    /// <returns>Resultado de la confirmación</returns>
    [HttpPost("confirmar")]
    [ProducesResponseType(typeof(RespuestaDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespuestaDTO>> ConfirmarImpresion(
        [FromBody] ConfirmacionImpresionDTO confirmacion)
    {
        try
        {
            // Buscar la impresión pendiente
            var impresion = await _contexto.Impresiones.FindAsync(confirmacion.ImpresionId);

            if (impresion == null)
            {
                _logger.LogWarning("Intento de confirmar impresión inexistente: ID={Id}", confirmacion.ImpresionId);
                return NotFound(new RespuestaDTO
                {
                    Exitosa = false,
                    Mensaje = "Impresión no encontrada"
                });
            }

            // Actualizar estado de la impresión
            impresion.FechaEjecucion = confirmacion.FechaEjecucion;
            impresion.Estado = confirmacion.Exitosa
                ? Dominio.Enumeraciones.EstadoImpresion.Ejecutada
                : Dominio.Enumeraciones.EstadoImpresion.Fallida;
            impresion.Resultado = confirmacion.Resultado ?? (confirmacion.Exitosa ? "Exitosa" : "Fallida");
            impresion.MensajeError = confirmacion.MensajeError;

            await _contexto.SaveChangesAsync();

            _logger.LogInformation(
                "Impresión confirmada - ID={Id}, Estado={Estado}",
                impresion.Id,
                impresion.Estado);

            return Ok(new RespuestaDTO
            {
                Exitosa = true,
                Mensaje = "Confirmación registrada correctamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al confirmar impresión ID={Id}", confirmacion.ImpresionId);
            return StatusCode(StatusCodes.Status500InternalServerError, new RespuestaDTO
            {
                Exitosa = false,
                Mensaje = "Error interno del sistema"
            });
        }
    }

    /// <summary>
    /// Obtiene el detalle de una impresión
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ObtenerImpresion(int id)
    {
        try
        {
            var impresion = await _contexto.Impresiones
                .Include(i => i.Maquina)
                .Include(i => i.Empleado)
                .Include(i => i.Lote)
                .Include(i => i.Etiqueta)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (impresion == null)
                return NotFound();

            return Ok(impresion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener impresión ID={Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    #endregion
}

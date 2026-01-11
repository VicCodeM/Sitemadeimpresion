using Microsoft.EntityFrameworkCore;
using SistemaImpresion.Datos.Contexto;
using SistemaImpresion.Dominio.Entidades;
using SistemaImpresion.Dominio.Enumeraciones;
using SistemaImpresion.Dominio.Interfaces;

namespace SistemaImpresion.Negocio.MotorReglas;

/// <summary>
/// Implementación del motor de reglas de impresión
/// PRINCIPIO: "Imprimir no es mandar un archivo, es ejecutar una decisión del sistema"
/// Este motor valida TODAS las condiciones antes de autorizar una impresión
/// </summary>
public class MotorReglasImpresion : IMotorReglasImpresion
{
    #region Campos Privados

    private readonly SistemaImpresionDbContext _contexto;

    #endregion

    #region Constructor

    public MotorReglasImpresion(SistemaImpresionDbContext contexto)
    {
        _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
    }

    #endregion

    #region Implementación de IMotorReglasImpresion

    /// <summary>
    /// Evalúa si una solicitud de impresión debe ser autorizada
    /// Implementa el algoritmo completo de validación según pseudocódigo
    /// </summary>
    public async Task<ResultadoAutorizacion> EvaluarAutorizacionAsync(
        string identificadorMaquina,
        string numeroEmpleado,
        int cantidad,
        string? direccionIP = null)
    {
        // Variable para acumular el resultado
        var resultado = new ResultadoAutorizacion
        {
            Autorizada = false
        };

        try
        {
            // ============================================
            // 1. VALIDAR MÁQUINA
            // ============================================
            var maquina = await ObtenerMaquinaPorIdentificadorAsync(identificadorMaquina);
            if (maquina == null || !maquina.Activo)
            {
                resultado.MotivoDenegacion = "Máquina no registrada o inactiva";
                await RegistrarIntentoDenegadoAsync(null, null, null, null, null, cantidad, resultado.MotivoDenegacion, direccionIP);
                return resultado;
            }
            resultado.Maquina = maquina;

            // ============================================
            // 2. VALIDAR IMPRESORA
            // ============================================
            var impresora = await _contexto.Impresoras
                .FirstOrDefaultAsync(i => i.MaquinaId == maquina.Id && i.Activo);

            if (impresora == null)
            {
                resultado.MotivoDenegacion = "Impresora no asignada o inactiva";
                await RegistrarIntentoDenegadoAsync(maquina, null, null, null, null, cantidad, resultado.MotivoDenegacion, direccionIP);
                return resultado;
            }

            // ============================================
            // 3. VALIDAR EMPLEADO
            // ============================================
            var empleado = await _contexto.Empleados
                .FirstOrDefaultAsync(e => e.NumeroEmpleado == numeroEmpleado && e.Activo);

            if (empleado == null)
            {
                resultado.MotivoDenegacion = "Empleado no válido o inactivo";
                await RegistrarIntentoDenegadoAsync(maquina, impresora, null, null, null, cantidad, resultado.MotivoDenegacion, direccionIP);
                return resultado;
            }
            resultado.Empleado = empleado;

            // ============================================
            // 4. VALIDAR LOTE ACTIVO
            // ============================================
            var loteMaquina = await _contexto.LotesMaquinas
                .Include(lm => lm.Lote)
                .ThenInclude(l => l!.Etiqueta)
                .Where(lm => lm.MaquinaId == maquina.Id && lm.Activo)
                .OrderBy(lm => lm.Prioridad)
                .FirstOrDefaultAsync();

            if (loteMaquina == null || loteMaquina.Lote == null)
            {
                resultado.MotivoDenegacion = "No hay lote activo asignado a esta máquina";
                await RegistrarIntentoDenegadoAsync(maquina, impresora, empleado, null, null, cantidad, resultado.MotivoDenegacion, direccionIP);
                return resultado;
            }

            var lote = loteMaquina.Lote;
            if (!lote.Activo)
            {
                resultado.MotivoDenegacion = "Lote no está activo";
                await RegistrarIntentoDenegadoAsync(maquina, impresora, empleado, null, null, cantidad, resultado.MotivoDenegacion, direccionIP);
                return resultado;
            }
            resultado.Lote = lote;

            // ============================================
            // 5. VALIDAR ETIQUETA AUTORIZADA
            // ============================================
            var etiqueta = lote.Etiqueta;
            if (etiqueta == null)
            {
                resultado.MotivoDenegacion = "No hay etiqueta asignada al lote";
                await RegistrarIntentoDenegadoAsync(maquina, impresora, empleado, lote, null, cantidad, resultado.MotivoDenegacion, direccionIP);
                return resultado;
            }

            if (!etiqueta.Activo)
            {
                resultado.MotivoDenegacion = "La etiqueta no está activa";
                await RegistrarIntentoDenegadoAsync(maquina, impresora, empleado, lote, etiqueta, cantidad, resultado.MotivoDenegacion, direccionIP);
                return resultado;
            }
            resultado.Etiqueta = etiqueta;

            // Verificar regla de autorización Máquina-Etiqueta
            var regla = await _contexto.ReglasImpresion
                .FirstOrDefaultAsync(r =>
                    r.MaquinaId == maquina.Id &&
                    r.EtiquetaId == etiqueta.Id &&
                    r.Activo);

            if (regla == null || !regla.Autorizada)
            {
                resultado.MotivoDenegacion = "Esta máquina no está autorizada para imprimir esta etiqueta";
                await RegistrarIntentoDenegadoAsync(maquina, impresora, empleado, lote, etiqueta, cantidad, resultado.MotivoDenegacion, direccionIP);
                return resultado;
            }

            // ============================================
            // 6. VALIDAR LÍMITES DE IMPRESIÓN
            // ============================================

            // 6.1: Límite diario de la máquina
            if (maquina.LimiteImpresionDiario > 0)
            {
                var inicioDiaUtc = DateTime.UtcNow.Date;
                var totalHoy = await _contexto.Impresiones
                    .Where(i => i.MaquinaId == maquina.Id &&
                                i.FechaSolicitud >= inicioDiaUtc &&
                                (i.Estado == EstadoImpresion.Autorizada || i.Estado == EstadoImpresion.Ejecutada))
                    .SumAsync(i => i.Cantidad);

                if (totalHoy >= maquina.LimiteImpresionDiario)
                {
                    resultado.MotivoDenegacion = $"Límite diario de impresiones alcanzado ({maquina.LimiteImpresionDiario})";
                    await RegistrarIntentoDenegadoAsync(maquina, impresora, empleado, lote, etiqueta, cantidad, resultado.MotivoDenegacion, direccionIP);
                    return resultado;
                }

                if ((totalHoy + cantidad) > maquina.LimiteImpresionDiario)
                {
                    resultado.MotivoDenegacion = $"La cantidad solicitada excede el límite diario ({maquina.LimiteImpresionDiario - totalHoy} restantes)";
                    await RegistrarIntentoDenegadoAsync(maquina, impresora, empleado, lote, etiqueta, cantidad, resultado.MotivoDenegacion, direccionIP);
                    return resultado;
                }
            }

            // 6.2: Límite del lote
            if (lote.CantidadMaxima > 0)
            {
                var totalLote = await _contexto.Impresiones
                    .Where(i => i.LoteId == lote.Id &&
                                (i.Estado == EstadoImpresion.Autorizada || i.Estado == EstadoImpresion.Ejecutada))
                    .SumAsync(i => i.Cantidad);

                if (totalLote >= lote.CantidadMaxima)
                {
                    resultado.MotivoDenegacion = $"Límite de impresiones del lote alcanzado ({lote.CantidadMaxima})";
                    await RegistrarIntentoDenegadoAsync(maquina, impresora, empleado, lote, etiqueta, cantidad, resultado.MotivoDenegacion, direccionIP);
                    return resultado;
                }

                if ((totalLote + cantidad) > lote.CantidadMaxima)
                {
                    resultado.MotivoDenegacion = $"La cantidad solicitada excede el límite del lote ({lote.CantidadMaxima - totalLote} restantes)";
                    await RegistrarIntentoDenegadoAsync(maquina, impresora, empleado, lote, etiqueta, cantidad, resultado.MotivoDenegacion, direccionIP);
                    return resultado;
                }
            }

            // 6.3: Límite de la regla específica (si está configurado)
            if (regla.LimiteImpresiones > 0)
            {
                var totalRegla = await _contexto.Impresiones
                    .Where(i => i.MaquinaId == maquina.Id &&
                                i.EtiquetaId == etiqueta.Id &&
                                (i.Estado == EstadoImpresion.Autorizada || i.Estado == EstadoImpresion.Ejecutada))
                    .SumAsync(i => i.Cantidad);

                if (totalRegla >= regla.LimiteImpresiones)
                {
                    resultado.MotivoDenegacion = $"Límite de impresiones para esta combinación máquina-etiqueta alcanzado";
                    await RegistrarIntentoDenegadoAsync(maquina, impresora, empleado, lote, etiqueta, cantidad, resultado.MotivoDenegacion, direccionIP);
                    return resultado;
                }
            }

            // ============================================
            // 7. TODO VALIDADO - AUTORIZAR
            // ============================================
            var impresion = new Impresion
            {
                MaquinaId = maquina.Id,
                ImpresoraId = impresora.Id,
                EmpleadoId = empleado.Id,
                LoteId = lote.Id,
                EtiquetaId = etiqueta.Id,
                Cantidad = cantidad,
                Estado = EstadoImpresion.Autorizada,
                FechaSolicitud = DateTime.UtcNow,
                FechaAutorizacion = DateTime.UtcNow,
                DireccionIPOrigen = direccionIP,
                Resultado = "Autorizada por motor de reglas",
                AutorizadoPorUsuarioId = null // Autorización automática
            };

            _contexto.Impresiones.Add(impresion);
            await _contexto.SaveChangesAsync();

            // Preparar respuesta exitosa
            resultado.Autorizada = true;
            resultado.Impresion = impresion;
            resultado.ContenidoZPL = etiqueta.ContenidoZPL;

            return resultado;
        }
        catch (Exception ex)
        {
            resultado.Autorizada = false;
            resultado.MotivoDenegacion = $"Error interno del sistema: {ex.Message}";
            return resultado;
        }
    }

    #endregion

    #region Métodos Privados Auxiliares

    /// <summary>
    /// Obtiene una máquina por su identificador (hostname o MAC)
    /// </summary>
    private async Task<Maquina?> ObtenerMaquinaPorIdentificadorAsync(string identificador)
    {
        return await _contexto.Maquinas
            .FirstOrDefaultAsync(m =>
                m.IdentificadorRed == identificador ||
                m.DireccionMAC == identificador ||
                m.Codigo == identificador);
    }

    /// <summary>
    /// Registra un intento denegado para auditoría
    /// </summary>
    private async Task RegistrarIntentoDenegadoAsync(
        Maquina? maquina,
        Impresora? impresora,
        Empleado? empleado,
        Lote? lote,
        Etiqueta? etiqueta,
        int cantidad,
        string motivoDenegacion,
        string? direccionIP)
    {
        try
        {
            var impresion = new Impresion
            {
                MaquinaId = maquina?.Id ?? 0,
                ImpresoraId = impresora?.Id ?? 0,
                EmpleadoId = empleado?.Id ?? 0,
                LoteId = lote?.Id ?? 0,
                EtiquetaId = etiqueta?.Id ?? 0,
                Cantidad = cantidad,
                Estado = EstadoImpresion.Denegada,
                FechaSolicitud = DateTime.UtcNow,
                MotivoDenegacion = motivoDenegacion,
                DireccionIPOrigen = direccionIP,
                Resultado = "Denegada"
            };

            _contexto.Impresiones.Add(impresion);
            await _contexto.SaveChangesAsync();
        }
        catch
        {
            // No propagar errores de auditoría
            // En producción, esto debería ir a un log
        }
    }

    #endregion
}

using Microsoft.EntityFrameworkCore;
using SistemaImpresion.Datos.Contexto;
using SistemaImpresion.Dominio.Entidades;
using SistemaImpresion.Dominio.Enumeraciones;

namespace SistemaImpresion.Datos;

public static class SeederPruebas
{
    public static async Task SeedAsync(SistemaImpresionDbContext contexto, string computerName)
    {
        // 1. Asegurar Empleado
        if (!await contexto.Empleados.AnyAsync())
        {
            contexto.Empleados.Add(new Empleado 
            { 
                NumeroEmpleado = "123456", 
                Nombre = "Operador de Pruebas", 
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            });
        }

        // 2. Asegurar Etiqueta
        var etiqueta = await contexto.Etiquetas.FirstOrDefaultAsync();
        if (etiqueta == null)
        {
            etiqueta = new Etiqueta
            {
                Codigo = "TBL-TEST",
                Nombre = "Zebra 4x6 Test",
                ContenidoZPL = "^XA^FO50,50^A0N,50,50^FDETIQUETA DE PRUEBA^FS^XZ",
                AnchoMM = 100,
                AltoMM = 150,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            };
            contexto.Etiquetas.Add(etiqueta);
            await contexto.SaveChangesAsync();
        }

        // 3. Asegurar Máquina
        var maquina = await contexto.Maquinas.FirstOrDefaultAsync(m => m.IdentificadorRed == computerName);
        if (maquina == null)
        {
            maquina = new Maquina
            {
                Codigo = "PC-PRUEBA",
                Nombre = "Estación de Trabajo Local",
                IdentificadorRed = computerName,
                Activo = true,
                FechaCreacion = DateTime.UtcNow,
                LimiteImpresionDiario = 100
            };
            contexto.Maquinas.Add(maquina);
            await contexto.SaveChangesAsync();
        }

        // 4. Asegurar Lote y Asociación
        var lote = await contexto.Lotes.FirstOrDefaultAsync(l => l.Activo);
        if (lote == null)
        {
            lote = new Lote
            {
                Numero = "L-001",
                Descripcion = "Lote de Pruebas Inicial",
                EtiquetaId = etiqueta.Id,
                CantidadMaxima = 1000,
                Activo = true,
                FechaCreacion = DateTime.UtcNow,
                FechaInicio = DateTime.UtcNow
            };
            contexto.Lotes.Add(lote);
            await contexto.SaveChangesAsync();
        }

        if (!await contexto.LotesMaquinas.AnyAsync(lm => lm.MaquinaId == maquina.Id && lm.LoteId == lote.Id))
        {
            contexto.LotesMaquinas.Add(new LoteMaquina
            {
                MaquinaId = maquina.Id,
                LoteId = lote.Id,
                Activo = true,
                Prioridad = 1
            });
        }

        // 5. Asegurar Impresora
        if (!await contexto.Impresoras.AnyAsync(i => i.MaquinaId == maquina.Id))
        {
            contexto.Impresoras.Add(new Impresora
            {
                Codigo = "ZEBRA-USB",
                Modelo = "Zebra GK420t Local",
                PuertoUSB = "USB001",
                NumeroSerie = "TEST-SERIAL",
                MaquinaId = maquina.Id,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            });
        }

        // 6. Asegurar Regla de Autorización
        if (!await contexto.ReglasImpresion.AnyAsync(r => r.MaquinaId == maquina.Id && r.EtiquetaId == etiqueta.Id))
        {
            contexto.ReglasImpresion.Add(new ReglaImpresion
            {
                MaquinaId = maquina.Id,
                EtiquetaId = etiqueta.Id,
                Autorizada = true,
                Activo = true,
                LimiteImpresiones = 0,
                FechaCreacion = DateTime.UtcNow
            });
        }

        await contexto.SaveChangesAsync();
    }
}

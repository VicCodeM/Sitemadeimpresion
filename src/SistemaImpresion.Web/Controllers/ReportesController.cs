using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaImpresion.Datos.Contexto;

namespace SistemaImpresion.Web.Controllers;

[Authorize]
public class ReportesController : Controller
{
    private readonly SistemaImpresionDbContext _context;

    public ReportesController(SistemaImpresionDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.TotalImpresiones = await _context.Impresiones.CountAsync();
        ViewBag.ImpresionesAutorizadas = await _context.Impresiones
            .Where(i => i.Estado == Dominio.Enumeraciones.EstadoImpresion.Autorizada ||
                        i.Estado == Dominio.Enumeraciones.EstadoImpresion.Ejecutada)
            .CountAsync();
        ViewBag.ImpresionesDenegadas = await _context.Impresiones
            .Where(i => i.Estado == Dominio.Enumeraciones.EstadoImpresion.Denegada)
            .CountAsync();
        
        var impresionesRecientes = await _context.Impresiones
            .Include(i => i.Maquina)
            .Include(i => i.Empleado)
            .Include(i => i.Etiqueta)
            .OrderByDescending(i => i.FechaSolicitud)
            .Take(50)
            .ToListAsync();
        
        return View(impresionesRecientes);
    }
}

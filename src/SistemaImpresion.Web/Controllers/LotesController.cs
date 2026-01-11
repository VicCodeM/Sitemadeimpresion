using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaImpresion.Datos.Contexto;
using SistemaImpresion.Dominio.Entidades;

namespace SistemaImpresion.Web.Controllers;

[Authorize]
public class LotesController : Controller
{
    private readonly SistemaImpresionDbContext _context;

    public LotesController(SistemaImpresionDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var lotes = await _context.Lotes
            .Include(l => l.Etiqueta)
            .OrderByDescending(l => l.FechaInicio)
            .ToListAsync();
        return View(lotes);
    }

    // GET: Lotes/Crear
    public async Task<IActionResult> Crear()
    {
        ViewBag.Etiquetas = await _context.Etiquetas.Where(e => e.Activo).ToListAsync();
        return View();
    }

    // POST: Lotes/Crear
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Lote lote)
    {
        if (ModelState.IsValid)
        {
            lote.FechaCreacion = DateTime.UtcNow;
            if (lote.FechaInicio == default) lote.FechaInicio = DateTime.UtcNow;
            _context.Add(lote);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Lote creado exitosamente";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Etiquetas = await _context.Etiquetas.Where(e => e.Activo).ToListAsync();
        return View(lote);
    }

    // GET: Lotes/Editar/5
    public async Task<IActionResult> Editar(int? id)
    {
        if (id == null) return NotFound();

        var lote = await _context.Lotes.FindAsync(id);
        if (lote == null) return NotFound();

        ViewBag.Etiquetas = await _context.Etiquetas.Where(e => e.Activo).ToListAsync();
        return View(lote);
    }

    // POST: Lotes/Editar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Lote lote)
    {
        if (id != lote.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                lote.FechaModificacion = DateTime.UtcNow;
                _context.Update(lote);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Lote actualizado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoteExists(lote.Id)) return NotFound();
                throw;
            }
        }
        ViewBag.Etiquetas = await _context.Etiquetas.Where(e => e.Activo).ToListAsync();
        return View(lote);
    }

    // GET: Lotes/Eliminar/5
    public async Task<IActionResult> Eliminar(int? id)
    {
        if (id == null) return NotFound();

        var lote = await _context.Lotes
            .Include(l => l.Etiqueta)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (lote == null) return NotFound();

        return View(lote);
    }

    // POST: Lotes/Eliminar/5
    [HttpPost, ActionName("Eliminar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarConfirmado(int id)
    {
        var lote = await _context.Lotes.FindAsync(id);
        if (lote != null)
        {
            _context.Lotes.Remove(lote);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Lote eliminado exitosamente";
        }
        return RedirectToAction(nameof(Index));
    }

    private bool LoteExists(int id)
    {
        return _context.Lotes.Any(e => e.Id == id);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaImpresion.Datos.Contexto;
using SistemaImpresion.Dominio.Entidades;

namespace SistemaImpresion.Web.Controllers;

[Authorize]
public class EtiquetasController : Controller
{
    private readonly SistemaImpresionDbContext _context;

    public EtiquetasController(SistemaImpresionDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var etiquetas = await _context.Etiquetas
            .OrderBy(e => e.Nombre)
            .ToListAsync();
        return View(etiquetas);
    }

    // GET: Etiquetas/Crear
    public IActionResult Crear()
    {
        return View();
    }

    // POST: Etiquetas/Crear
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Etiqueta etiqueta)
    {
        if (ModelState.IsValid)
        {
            etiqueta.FechaCreacion = DateTime.UtcNow;
            _context.Add(etiqueta);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Etiqueta creada exitosamente";
            return RedirectToAction(nameof(Index));
        }
        return View(etiqueta);
    }

    // GET: Etiquetas/Editar/5
    public async Task<IActionResult> Editar(int? id)
    {
        if (id == null) return NotFound();

        var etiqueta = await _context.Etiquetas.FindAsync(id);
        if (etiqueta == null) return NotFound();

        return View(etiqueta);
    }

    // POST: Etiquetas/Editar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Etiqueta etiqueta)
    {
        if (id != etiqueta.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                etiqueta.FechaModificacion = DateTime.UtcNow;
                _context.Update(etiqueta);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Etiqueta actualizada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EtiquetaExists(etiqueta.Id)) return NotFound();
                throw;
            }
        }
        return View(etiqueta);
    }

    // GET: Etiquetas/Eliminar/5
    public async Task<IActionResult> Eliminar(int? id)
    {
        if (id == null) return NotFound();

        var etiqueta = await _context.Etiquetas
            .FirstOrDefaultAsync(m => m.Id == id);
        if (etiqueta == null) return NotFound();

        return View(etiqueta);
    }

    // POST: Etiquetas/Eliminar/5
    [HttpPost, ActionName("Eliminar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarConfirmado(int id)
    {
        var etiqueta = await _context.Etiquetas.FindAsync(id);
        if (etiqueta != null)
        {
            _context.Etiquetas.Remove(etiqueta);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Etiqueta eliminada exitosamente";
        }
        return RedirectToAction(nameof(Index));
    }

    private bool EtiquetaExists(int id)
    {
        return _context.Etiquetas.Any(e => e.Id == id);
    }
}

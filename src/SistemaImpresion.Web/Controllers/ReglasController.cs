using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaImpresion.Datos.Contexto;
using SistemaImpresion.Dominio.Entidades;

namespace SistemaImpresion.Web.Controllers;

[Authorize]
public class ReglasController : Controller
{
    private readonly SistemaImpresionDbContext _context;

    public ReglasController(SistemaImpresionDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var reglas = await _context.ReglasImpresion
            .Include(r => r.Maquina)
            .Include(r => r.Etiqueta)
            .OrderBy(r => r.Id)
            .ToListAsync();
        return View(reglas);
    }

    // GET: Reglas/Crear
    public async Task<IActionResult> Crear()
    {
        ViewBag.Maquinas = await _context.Maquinas.Where(m => m.Activo).ToListAsync();
        ViewBag.Etiquetas = await _context.Etiquetas.Where(e => e.Activo).ToListAsync();
        return View();
    }

    // POST: Reglas/Crear
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(ReglaImpresion regla)
    {
        if (ModelState.IsValid)
        {
            regla.FechaCreacion = DateTime.UtcNow;
            _context.Add(regla);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Regla creada exitosamente";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Maquinas = await _context.Maquinas.Where(m => m.Activo).ToListAsync();
        ViewBag.Etiquetas = await _context.Etiquetas.Where(e => e.Activo).ToListAsync();
        return View(regla);
    }

    // GET: Reglas/Editar/5
    public async Task<IActionResult> Editar(int? id)
    {
        if (id == null) return NotFound();

        var regla = await _context.ReglasImpresion.FindAsync(id);
        if (regla == null) return NotFound();

        ViewBag.Maquinas = await _context.Maquinas.Where(m => m.Activo).ToListAsync();
        ViewBag.Etiquetas = await _context.Etiquetas.Where(e => e.Activo).ToListAsync();
        return View(regla);
    }

    // POST: Reglas/Editar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, ReglaImpresion regla)
    {
        if (id != regla.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                regla.FechaModificacion = DateTime.UtcNow;
                _context.Update(regla);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Regla actualizada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReglaExists(regla.Id)) return NotFound();
                throw;
            }
        }
        ViewBag.Maquinas = await _context.Maquinas.Where(m => m.Activo).ToListAsync();
        ViewBag.Etiquetas = await _context.Etiquetas.Where(e => e.Activo).ToListAsync();
        return View(regla);
    }

    // GET: Reglas/Eliminar/5
    public async Task<IActionResult> Eliminar(int? id)
    {
        if (id == null) return NotFound();

        var regla = await _context.ReglasImpresion
            .Include(r => r.Maquina)
            .Include(r => r.Etiqueta)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (regla == null) return NotFound();

        return View(regla);
    }

    // POST: Reglas/Eliminar/5
    [HttpPost, ActionName("Eliminar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarConfirmado(int id)
    {
        var regla = await _context.ReglasImpresion.FindAsync(id);
        if (regla != null)
        {
            _context.ReglasImpresion.Remove(regla);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Regla eliminada exitosamente";
        }
        return RedirectToAction(nameof(Index));
    }

    private bool ReglaExists(int id)
    {
        return _context.ReglasImpresion.Any(e => e.Id == id);
    }
}

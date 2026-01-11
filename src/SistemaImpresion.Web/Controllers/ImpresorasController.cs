using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaImpresion.Datos.Contexto;
using SistemaImpresion.Dominio.Entidades;

namespace SistemaImpresion.Web.Controllers;

[Authorize]
public class ImpresorasController : Controller
{
    private readonly SistemaImpresionDbContext _context;

    public ImpresorasController(SistemaImpresionDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var impresoras = await _context.Impresoras
            .Include(i => i.Maquina)
            .OrderBy(i => i.Codigo)
            .ToListAsync();
        return View(impresoras);
    }

    // GET: Impresoras/Crear
    public async Task<IActionResult> Crear()
    {
        ViewBag.Maquinas = await _context.Maquinas.Where(m => m.Activo).ToListAsync();
        return View();
    }

    // POST: Impresoras/Crear
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Impresora impresora)
    {
        if (ModelState.IsValid)
        {
            impresora.FechaCreacion = DateTime.UtcNow;
            _context.Add(impresora);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Impresora creada exitosamente";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Maquinas = await _context.Maquinas.Where(m => m.Activo).ToListAsync();
        return View(impresora);
    }

    // GET: Impresoras/Editar/5
    public async Task<IActionResult> Editar(int? id)
    {
        if (id == null) return NotFound();

        var impresora = await _context.Impresoras.FindAsync(id);
        if (impresora == null) return NotFound();

        ViewBag.Maquinas = await _context.Maquinas.Where(m => m.Activo).ToListAsync();
        return View(impresora);
    }

    // POST: Impresoras/Editar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Impresora impresora)
    {
        if (id != impresora.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                impresora.FechaModificacion = DateTime.UtcNow;
                _context.Update(impresora);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Impresora actualizada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImpresoraExists(impresora.Id)) return NotFound();
                throw;
            }
        }
        ViewBag.Maquinas = await _context.Maquinas.Where(m => m.Activo).ToListAsync();
        return View(impresora);
    }

    // GET: Impresoras/Eliminar/5
    public async Task<IActionResult> Eliminar(int? id)
    {
        if (id == null) return NotFound();

        var impresora = await _context.Impresoras
            .Include(i => i.Maquina)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (impresora == null) return NotFound();

        return View(impresora);
    }

    // POST: Impresoras/Eliminar/5
    [HttpPost, ActionName("Eliminar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarConfirmado(int id)
    {
        var impresora = await _context.Impresoras.FindAsync(id);
        if (impresora != null)
        {
            _context.Impresoras.Remove(impresora);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Impresora eliminada exitosamente";
        }
        return RedirectToAction(nameof(Index));
    }

    private bool ImpresoraExists(int id)
    {
        return _context.Impresoras.Any(e => e.Id == id);
    }
}

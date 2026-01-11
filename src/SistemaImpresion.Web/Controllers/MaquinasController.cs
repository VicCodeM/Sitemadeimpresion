using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaImpresion.Datos.Contexto;
using SistemaImpresion.Dominio.Entidades;

namespace SistemaImpresion.Web.Controllers;

[Authorize]
public class MaquinasController : Controller
{
    private readonly SistemaImpresionDbContext _context;

    public MaquinasController(SistemaImpresionDbContext context)
    {
        _context = context;
    }

    // GET: Maquinas
    public async Task<IActionResult> Index()
    {
        var maquinas = await _context.Maquinas
            .OrderBy(m => m.Codigo)
            .ToListAsync();
        return View(maquinas);
    }

    // GET: Maquinas/Crear
    public IActionResult Crear()
    {
        return View();
    }

    // POST: Maquinas/Crear
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Maquina maquina)
    {
        if (ModelState.IsValid)
        {
            maquina.FechaCreacion = DateTime.UtcNow;
            _context.Add(maquina);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Máquina creada exitosamente";
            return RedirectToAction(nameof(Index));
        }
        return View(maquina);
    }

    // GET: Maquinas/Editar/5
    public async Task<IActionResult> Editar(int? id)
    {
        if (id == null) return NotFound();

        var maquina = await _context.Maquinas.FindAsync(id);
        if (maquina == null) return NotFound();

        return View(maquina);
    }

    // POST: Maquinas/Editar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Maquina maquina)
    {
        if (id != maquina.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                maquina.FechaModificacion = DateTime.UtcNow;
                _context.Update(maquina);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Máquina actualizada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaquinaExists(maquina.Id))
                    return NotFound();
                throw;
            }
        }
        return View(maquina);
    }

    // GET: Maquinas/Eliminar/5
    public async Task<IActionResult> Eliminar(int? id)
    {
        if (id == null) return NotFound();

        var maquina = await _context.Maquinas
            .FirstOrDefaultAsync(m => m.Id == id);
        if (maquina == null) return NotFound();

        return View(maquina);
    }

    // POST: Maquinas/Eliminar/5
    [HttpPost, ActionName("Eliminar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarConfirmado(int id)
    {
        var maquina = await _context.Maquinas.FindAsync(id);
        if (maquina != null)
        {
            _context.Maquinas.Remove(maquina);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Máquina eliminada exitosamente";
        }
        return RedirectToAction(nameof(Index));
    }

    private bool MaquinaExists(int id)
    {
        return _context.Maquinas.Any(e => e.Id == id);
    }
}

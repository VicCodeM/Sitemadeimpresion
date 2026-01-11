using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaImpresion.Datos.Contexto;
using SistemaImpresion.Dominio.Entidades;

namespace SistemaImpresion.Web.Controllers;

[Authorize]
public class EmpleadosController : Controller
{
    private readonly SistemaImpresionDbContext _context;

    public EmpleadosController(SistemaImpresionDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var empleados = await _context.Empleados
            .OrderBy(e => e.NumeroEmpleado)
            .ToListAsync();
        return View(empleados);
    }

    // GET: Empleados/Crear
    public IActionResult Crear()
    {
        return View();
    }

    // POST: Empleados/Crear
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Empleado empleado)
    {
        if (ModelState.IsValid)
        {
            empleado.FechaCreacion = DateTime.UtcNow;
            empleado.FechaAlta = DateTime.UtcNow;
            _context.Add(empleado);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Empleado creado exitosamente";
            return RedirectToAction(nameof(Index));
        }
        return View(empleado);
    }

    // GET: Empleados/Editar/5
    public async Task<IActionResult> Editar(int? id)
    {
        if (id == null) return NotFound();

        var empleado = await _context.Empleados.FindAsync(id);
        if (empleado == null) return NotFound();

        return View(empleado);
    }

    // POST: Empleados/Editar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Empleado empleado)
    {
        if (id != empleado.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                empleado.FechaModificacion = DateTime.UtcNow;
                _context.Update(empleado);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Empleado actualizado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpleadoExists(empleado.Id)) return NotFound();
                throw;
            }
        }
        return View(empleado);
    }

    // GET: Empleados/Eliminar/5
    public async Task<IActionResult> Eliminar(int? id)
    {
        if (id == null) return NotFound();

        var empleado = await _context.Empleados
            .FirstOrDefaultAsync(m => m.Id == id);
        if (empleado == null) return NotFound();

        return View(empleado);
    }

    // POST: Empleados/Eliminar/5
    [HttpPost, ActionName("Eliminar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarConfirmado(int id)
    {
        var empleado = await _context.Empleados.FindAsync(id);
        if (empleado != null)
        {
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Empleado eliminado exitosamente";
        }
        return RedirectToAction(nameof(Index));
    }

    private bool EmpleadoExists(int id)
    {
        return _context.Empleados.Any(e => e.Id == id);
    }
}

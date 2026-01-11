using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaImpresion.Datos.Contexto;
using SistemaImpresion.Dominio.Entidades;

namespace SistemaImpresion.Web.Controllers;

[Authorize]
public class UsuariosController : Controller
{
    private readonly SistemaImpresionDbContext _context;

    public UsuariosController(SistemaImpresionDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var usuarios = await _context.Usuarios
            .Include(u => u.Rol)
            .OrderBy(u => u.NombreUsuario)
            .ToListAsync();
        return View(usuarios);
    }

    // GET: Usuarios/Crear
    public async Task<IActionResult> Crear()
    {
        ViewBag.Roles = await _context.Roles.ToListAsync();
        return View();
    }

    // POST: Usuarios/Crear
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Usuario usuario, string password)
    {
        if (ModelState.IsValid)
        {
            usuario.FechaCreacion = DateTime.UtcNow;
            usuario.HashContrasena = password; // En producción usar BCrypt
            _context.Add(usuario);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Usuario creado exitosamente";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Roles = await _context.Roles.ToListAsync();
        return View(usuario);
    }

    // GET: Usuarios/Editar/5
    public async Task<IActionResult> Editar(int? id)
    {
        if (id == null) return NotFound();

        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return NotFound();

        ViewBag.Roles = await _context.Roles.ToListAsync();
        return View(usuario);
    }

    // POST: Usuarios/Editar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Usuario usuario, string? newPassword)
    {
        if (id != usuario.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var usuarioDb = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (usuarioDb == null) return NotFound();

                usuario.FechaModificacion = DateTime.UtcNow;
                if (!string.IsNullOrEmpty(newPassword))
                {
                    usuario.HashContrasena = newPassword;
                }
                else
                {
                    usuario.HashContrasena = usuarioDb.HashContrasena;
                }

                _context.Update(usuario);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Usuario actualizado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.Id)) return NotFound();
                throw;
            }
        }
        ViewBag.Roles = await _context.Roles.ToListAsync();
        return View(usuario);
    }

    // GET: Usuarios/Eliminar/5
    public async Task<IActionResult> Eliminar(int? id)
    {
        if (id == null) return NotFound();

        var usuario = await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (usuario == null) return NotFound();

        return View(usuario);
    }

    // POST: Usuarios/Eliminar/5
    [HttpPost, ActionName("Eliminar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarConfirmado(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Usuario eliminado exitosamente";
        }
        return RedirectToAction(nameof(Index));
    }

    private bool UsuarioExists(int id)
    {
        return _context.Usuarios.Any(e => e.Id == id);
    }
}

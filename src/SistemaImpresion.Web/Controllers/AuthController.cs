using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaImpresion.Datos.Contexto;
using SistemaImpresion.Datos.Utilidades;
using System.Security.Claims;

namespace SistemaImpresion.Web.Controllers;

/// <summary>
/// Controlador de autenticación para login/logout
/// </summary>
public class AuthController : Controller
{
    #region Campos Privados

    private readonly SistemaImpresionDbContext _contexto;
    private readonly ILogger<AuthController> _logger;

    #endregion

    #region Constructor

    public AuthController(
        SistemaImpresionDbContext contexto,
        ILogger<AuthController> logger)
    {
        _contexto = contexto;
        _logger = logger;
    }

    #endregion

    #region Acciones

    /// <summary>
    /// Muestra el formulario de login
    /// </summary>
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    /// <summary>
    /// Procesa el login del usuario
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string nombreUsuario, string contrasena, string? returnUrl = null)
    {
        try
        {
            // Validar datos
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(contrasena))
            {
                ModelState.AddModelError("", "Usuario y contraseña son requeridos");
                return View();
            }

            // Buscar usuario
            var usuario = await _contexto.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario && u.Activo);

            if (usuario == null)
            {
                _logger.LogWarning("Intento de login con usuario inexistente: {Usuario}", nombreUsuario);
                ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                return View();
            }

            // Verificar si está bloqueado
            if (usuario.Bloqueado)
            {
                _logger.LogWarning("Intento de login con usuario bloqueado: {Usuario}", nombreUsuario);
                ModelState.AddModelError("", "Usuario bloqueado. Contacte al administrador");
                return View();
            }

            // Verificar contraseña
            if (!UtilidadContrasenas.VerificarContrasena(contrasena, usuario.HashContrasena))
            {
                // Incrementar intentos fallidos
                usuario.IntentosFallidos++;
                if (usuario.IntentosFallidos >= 5)
                {
                    usuario.Bloqueado = true;
                    _logger.LogWarning("Usuario bloqueado por intentos fallidos: {Usuario}", nombreUsuario);
                }
                await _contexto.SaveChangesAsync();

                ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                return View();
            }

            // Login exitoso
            usuario.IntentosFallidos = 0;
            usuario.UltimoAcceso = DateTime.UtcNow;
            await _contexto.SaveChangesAsync();

            // Crear claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim("NombreCompleto", usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "Usuario"),
                new Claim("RolId", usuario.RolId.ToString()),
                new Claim("PuedeAdministrar", usuario.Rol?.PuedeAdministrar.ToString() ?? "false"),
                new Claim("PuedeAutorizar", usuario.Rol?.PuedeAutorizar.ToString() ?? "false")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            _logger.LogInformation("Login exitoso: {Usuario}", nombreUsuario);

            // Redireccionar
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en login");
            ModelState.AddModelError("", "Error interno del sistema");
            return View();
        }
    }

    /// <summary>
    /// Cierra la sesión del usuario
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        _logger.LogInformation("Usuario cerró sesión");
        return RedirectToAction("Login");
    }

    /// <summary>
    /// Página de acceso denegado
    /// </summary>
    public IActionResult AccesoDenegado()
    {
        return View();
    }

    #endregion
}

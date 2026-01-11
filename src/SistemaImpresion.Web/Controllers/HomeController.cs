using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SistemaImpresion.Web.Controllers;

/// <summary>
/// Controlador principal de la aplicación web
/// </summary>
[Authorize]
public class HomeController : Controller
{
    #region Campos Privados

    private readonly ILogger<HomeController> _logger;

    #endregion

    #region Constructor

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    #endregion

    #region Acciones

    /// <summary>
    /// Dashboard principal
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Página de error
    /// </summary>
    public IActionResult Error()
    {
        return View();
    }

    #endregion
}

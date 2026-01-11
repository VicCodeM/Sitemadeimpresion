using SistemaImpresion.Dominio.DTOs;
using SistemaImpresion.Dominio.Entidades;

namespace SistemaImpresion.Negocio.Servicios;

/// <summary>
/// Interfaz del servicio de autenticación
/// </summary>
public interface IServicioAutenticacion
{
    /// <summary>
    /// Autentica a un usuario y genera un token
    /// </summary>
    Task<LoginResponseDTO?> AutenticarAsync(string nombreUsuario, string contrasena);

    /// <summary>
    /// Valida un token JWT
    /// </summary>
    Task<Usuario?> ValidarTokenAsync(string token);
}

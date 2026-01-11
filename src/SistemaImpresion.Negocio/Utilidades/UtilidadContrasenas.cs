using BCrypt.Net;

namespace SistemaImpresion.Negocio.Utilidades;

/// <summary>
/// Utilidad para hash de contraseñas con BCrypt
/// Proporciona métodos seguros para manejar contraseñas
/// </summary>
public static class UtilidadContrasenas
{
    #region Constantes

    /// <summary>
    /// Factor de trabajo para BCrypt (mayor = más seguro pero más lento)
    /// 12 es un buen balance entre seguridad y rendimiento
    /// </summary>
    private const int FACTOR_TRABAJO = 12;

    #endregion

    #region Métodos Públicos

    /// <summary>
    /// Genera un hash seguro de una contraseña
    /// </summary>
    /// <param name="contrasena">Contraseña en texto plano</param>
    /// <returns>Hash BCrypt de la contraseña</returns>
    public static string GenerarHash(string contrasena)
    {
        if (string.IsNullOrWhiteSpace(contrasena))
            throw new ArgumentException("La contraseña no puede estar vacía", nameof(contrasena));

        return BCrypt.Net.BCrypt.HashPassword(contrasena, FACTOR_TRABAJO);
    }

    /// <summary>
    /// Verifica si una contraseña coincide con un hash
    /// </summary>
    /// <param name="contrasena">Contraseña en texto plano a verificar</param>
    /// <param name="hash">Hash almacenado</param>
    /// <returns>True si la contraseña es correcta</returns>
    public static bool VerificarContrasena(string contrasena, string hash)
    {
        if (string.IsNullOrWhiteSpace(contrasena) || string.IsNullOrWhiteSpace(hash))
            return false;

        try
        {
            return BCrypt.Net.BCrypt.Verify(contrasena, hash);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Valida que una contraseña cumpla con requisitos mínimos de seguridad
    /// </summary>
    /// <param name="contrasena">Contraseña a validar</param>
    /// <param name="mensajeError">Mensaje de error si no es válida</param>
    /// <returns>True si la contraseña es válida</returns>
    public static bool ValidarFortaleza(string contrasena, out string mensajeError)
    {
        mensajeError = string.Empty;

        if (string.IsNullOrWhiteSpace(contrasena))
        {
            mensajeError = "La contraseña no puede estar vacía";
            return false;
        }

        if (contrasena.Length < 8)
        {
            mensajeError = "La contraseña debe tener al menos 8 caracteres";
            return false;
        }

        if (!contrasena.Any(char.IsUpper))
        {
            mensajeError = "La contraseña debe contener al menos una letra mayúscula";
            return false;
        }

        if (!contrasena.Any(char.IsLower))
        {
            mensajeError = "La contraseña debe contener al menos una letra minúscula";
            return false;
        }

        if (!contrasena.Any(char.IsDigit))
        {
            mensajeError = "La contraseña debe contener al menos un número";
            return false;
        }

        return true;
    }

    #endregion
}

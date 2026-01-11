namespace SistemaImpresion.Dominio.DTOs;

/// <summary>
/// DTO para crear o actualizar una máquina
/// </summary>
public class MaquinaDTO
{
    public int? Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? IdentificadorRed { get; set; }
    public string? DireccionMAC { get; set; }
    public int LimiteImpresionDiario { get; set; }
    public bool Activa { get; set; } = true;
}

/// <summary>
/// DTO para crear o actualizar una impresora
/// </summary>
public class ImpresoraDTO
{
    public int? Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string NumeroSerie { get; set; } = string.Empty;
    public string PuertoUSB { get; set; } = string.Empty;
    public int MaquinaId { get; set; }
    public int? ResolucionDPI { get; set; }
    public int? AnchoMaximoMM { get; set; }
    public bool Activa { get; set; } = true;
    public string? Notas { get; set; }
}

/// <summary>
/// DTO para crear o actualizar una etiqueta
/// </summary>
public class EtiquetaDTO
{
    public int? Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string ContenidoZPL { get; set; } = string.Empty;
    public int AnchoMM { get; set; }
    public int AltoMM { get; set; }
    public string? Categoria { get; set; }
    public string? Version { get; set; }
    public bool Activa { get; set; } = true;
}

/// <summary>
/// DTO para crear o actualizar un lote
/// </summary>
public class LoteDTO
{
    public int? Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int EtiquetaId { get; set; }
    public int CantidadMaxima { get; set; }
    public DateTime FechaInicio { get; set; } = DateTime.UtcNow;
    public DateTime? FechaFin { get; set; }
    public string? CodigoProducto { get; set; }
    public string? Cliente { get; set; }
    public bool Activo { get; set; } = true;
}

/// <summary>
/// DTO para crear o actualizar un empleado
/// </summary>
public class EmpleadoDTO
{
    public int? Id { get; set; }
    public string NumeroEmpleado { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Departamento { get; set; } = string.Empty;
    public string? Puesto { get; set; }
    public bool Activo { get; set; } = true;
}

/// <summary>
/// DTO para respuesta estándar de operaciones
/// </summary>
public class RespuestaDTO
{
    public bool Exitosa { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public object? Datos { get; set; }
}

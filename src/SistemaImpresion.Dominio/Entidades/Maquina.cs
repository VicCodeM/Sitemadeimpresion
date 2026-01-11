namespace SistemaImpresion.Dominio.Entidades;

/// <summary>
/// Representa una PC de producción en la planta
/// Cada máquina tiene una impresora Zebra asignada
/// </summary>
public class Maquina : EntidadBase
{
    #region Propiedades

    /// <summary>
    /// Código único de la máquina
    /// Ejemplo: "MAQ-01", "PRODUCCION-A", etc.
    /// </summary>
    public string Codigo { get; set; } = string.Empty;

    /// <summary>
    /// Nombre descriptivo de la máquina
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Descripción adicional o ubicación física
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Identificador de red de la máquina (hostname)
    /// Se usa para validar solicitudes de impresión
    /// </summary>
    public string? IdentificadorRed { get; set; }

    /// <summary>
    /// Dirección MAC de la interfaz de red principal
    /// Se usa como alternativa para identificar la máquina
    /// </summary>
    public string? DireccionMAC { get; set; }

    /// <summary>
    /// Límite máximo de impresiones permitidas por día
    /// 0 = sin límite
    /// </summary>
    public int LimiteImpresionDiario { get; set; } = 0;

    /// <summary>
    /// Fecha de registro de la máquina en el sistema
    /// </summary>
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    #endregion

    #region Relaciones

    /// <summary>
    /// Impresora Zebra asignada a esta máquina (relación 1:1)
    /// </summary>
    public virtual Impresora? Impresora { get; set; }

    /// <summary>
    /// Historial de impresiones ejecutadas en esta máquina
    /// </summary>
    public virtual ICollection<Impresion> Impresiones { get; set; } = new List<Impresion>();

    /// <summary>
    /// Lotes asignados a esta máquina
    /// </summary>
    public virtual ICollection<LoteMaquina> LotesAsignados { get; set; } = new List<LoteMaquina>();

    /// <summary>
    /// Reglas de impresión específicas para esta máquina
    /// </summary>
    public virtual ICollection<ReglaImpresion> Reglas { get; set; } = new List<ReglaImpresion>();

    #endregion
}

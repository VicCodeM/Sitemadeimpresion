namespace SistemaImpresion.Dominio.Entidades;

/// <summary>
/// Representa una plantilla de etiqueta en formato ZPL
/// Las etiquetas se almacenan en el servidor y se envían a las PCs al autorizar
/// </summary>
public class Etiqueta : EntidadBase
{
    #region Propiedades

    /// <summary>
    /// Código único de la etiqueta
    /// </summary>
    public string Codigo { get; set; } = string.Empty;

    /// <summary>
    /// Nombre descriptivo de la etiqueta
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Descripción detallada del uso de la etiqueta
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Contenido completo de la etiqueta en formato ZPL
    /// Este es el código que se envía a la impresora Zebra
    /// </summary>
    public string ContenidoZPL { get; set; } = string.Empty;

    /// <summary>
    /// Ancho de la etiqueta en milímetros
    /// </summary>
    public int AnchoMM { get; set; }

    /// <summary>
    /// Alto de la etiqueta en milímetros
    /// </summary>
    public int AltoMM { get; set; }

    /// <summary>
    /// Categoría o tipo de etiqueta
    /// Ejemplo: "Producto", "Envío", "Inventario"
    /// </summary>
    public string? Categoria { get; set; }

    /// <summary>
    /// Versión de la etiqueta (para control de cambios)
    /// </summary>
    public string? Version { get; set; }

    #endregion

    #region Relaciones

    /// <summary>
    /// Lotes que utilizan esta etiqueta
    /// </summary>
    public virtual ICollection<Lote> Lotes { get; set; } = new List<Lote>();

    /// <summary>
    /// Reglas de impresión que autorizan esta etiqueta
    /// </summary>
    public virtual ICollection<ReglaImpresion> Reglas { get; set; } = new List<ReglaImpresion>();

    /// <summary>
    /// Historial de impresiones de esta etiqueta
    /// </summary>
    public virtual ICollection<Impresion> Impresiones { get; set; } = new List<Impresion>();

    #endregion
}

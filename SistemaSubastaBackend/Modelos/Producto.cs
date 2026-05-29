namespace SistemaSubastaBackend.Modelos;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int? CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
    public ICollection<Subasta> Subastas { get; set; } = new List<Subasta>();
    public ICollection<ImagenProducto> Imagenes { get; set; } = new List<ImagenProducto>();
}

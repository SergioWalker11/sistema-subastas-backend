namespace SistemaSubastaBackend.Modelos;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public Subasta? Subasta { get; set; }
}

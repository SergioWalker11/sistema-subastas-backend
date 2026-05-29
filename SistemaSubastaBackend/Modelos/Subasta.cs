namespace SistemaSubastaBackend.Modelos;

public class Subasta
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public Producto Producto { get; set; } = null!;
    public int VendedorId { get; set; }
    public Usuario Vendedor { get; set; } = null!;
    public decimal PrecioInicial { get; set; }
    public decimal PrecioActual { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string Estado { get; set; } = "activa";
    public ICollection<Puja> Pujas { get; set; } = new List<Puja>();
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}

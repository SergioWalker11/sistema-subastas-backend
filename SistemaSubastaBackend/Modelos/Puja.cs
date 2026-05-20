namespace SistemaSubastaBackend.Modelos;

public class Puja
{
    public int Id { get; set; }
    public int SubastaId { get; set; }
    public Subasta Subasta { get; set; } = null!;
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public decimal Monto { get; set; }
    public DateTime FechaCreacion { get; set; }
}

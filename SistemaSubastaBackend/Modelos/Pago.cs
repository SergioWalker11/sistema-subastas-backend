namespace SistemaSubastaBackend.Modelos;

public class Pago
{
    public int Id { get; set; }
    public int SubastaId { get; set; }
    public Subasta Subasta { get; set; } = null!;
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public decimal Monto { get; set; }
    public string CodigoTransaccion { get; set; } = string.Empty;
    public string EstadoPago { get; set; } = "pendiente";
    public DateTime FechaPago { get; set; }
}

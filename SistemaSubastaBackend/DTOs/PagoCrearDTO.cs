namespace SistemaSubastaBackend.DTOs;

public class PagoCrearDTO
{
    public int SubastaId { get; set; }
    public int UsuarioId { get; set; }
    public decimal Monto { get; set; }
}

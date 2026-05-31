namespace SistemaSubastaBackend.DTOs;

public class PujaRespuestaDTO
{
    public int Id { get; set; }
    public int SubastaId { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public DateTime FechaCreacion { get; set; }
    public bool EsPropia { get; set; }
}

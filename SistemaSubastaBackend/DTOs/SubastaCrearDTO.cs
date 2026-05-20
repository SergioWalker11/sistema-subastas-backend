namespace SistemaSubastaBackend.DTOs;

public class SubastaCrearDTO
{
    public int ProductoId { get; set; }
    public decimal PrecioInicial { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}

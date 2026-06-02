namespace SistemaSubastaBackend.DTOs;

public class VentaDTO
{
    public int SubastaId { get; set; }
    public string NombreProducto { get; set; } = string.Empty;
    public string? NombreGanador { get; set; }
    public string? CorreoGanador { get; set; }
    public decimal PrecioFinal { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaFin { get; set; }
    public DateTime? FechaLimitePago { get; set; }
    public DateTime? FechaPago { get; set; }
}

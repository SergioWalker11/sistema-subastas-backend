namespace SistemaSubastaBackend.DTOs;

public class SubastaGanadaDTO
{
    public int Id { get; set; }
    public string NombreProducto { get; set; } = string.Empty;
    public decimal MontoGanado { get; set; }
    public DateTime FechaFin { get; set; }
    public DateTime? FechaLimitePago { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string NombreVendedor { get; set; } = string.Empty;
    public string CorreoVendedor { get; set; } = string.Empty;
    public bool Pagado { get; set; }
}

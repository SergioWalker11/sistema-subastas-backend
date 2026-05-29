namespace SistemaSubastaBackend.DTOs;

public class SubastaGanadaDTO
{
    public int Id { get; set; }
    public string NombreProducto { get; set; } = string.Empty;
    public decimal MontoGanado { get; set; }
    public DateTime FechaFin { get; set; }
    public string NombreVendedor { get; set; } = string.Empty;
    public bool Pagado { get; set; }
}

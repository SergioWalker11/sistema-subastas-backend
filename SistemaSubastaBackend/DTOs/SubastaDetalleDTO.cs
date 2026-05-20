using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.DTOs;

public class SubastaDetalleDTO
{
    public int Id { get; set; }
    public string NombreProducto { get; set; } = string.Empty;
    public string DescripcionProducto { get; set; } = string.Empty;
    public decimal PrecioInicial { get; set; }
    public decimal PrecioActual { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string Estado { get; set; } = string.Empty;
    public int CantidadPujas { get; set; }
}

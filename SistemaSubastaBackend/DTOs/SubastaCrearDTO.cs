namespace SistemaSubastaBackend.DTOs;

public class SubastaCrearDTO
{
    public int ProductoId { get; set; }
    public int VendedorId { get; set; }
    public string NombreProducto { get; set; } = string.Empty;
    public string DescripcionProducto { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public decimal PrecioInicial { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}

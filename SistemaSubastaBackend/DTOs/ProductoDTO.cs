namespace SistemaSubastaBackend.DTOs;

public class ProductoDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int? CategoriaId { get; set; }
    public string CategoriaNombre { get; set; } = string.Empty;
}

public class ProductoCrearDTO
{
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int? CategoriaId { get; set; }
}

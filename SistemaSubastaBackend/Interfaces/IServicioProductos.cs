using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Interfaces;

public interface IServicioProductos
{
    Task<List<ProductoDTO>> ListarAsync();
    Task<ProductoDTO?> ObtenerAsync(int id);
    Task<ProductoDTO> CrearAsync(ProductoCrearDTO dto);
    Task<List<Categoria>> ListarCategoriasAsync();
}

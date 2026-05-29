using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Interfaces;

public interface IRepositorioSubastas
{
    Task<List<Subasta>> ObtenerTodasAsync();
    Task<Subasta?> ObtenerPorIdAsync(int id);
    Task<Subasta> CrearAsync(Subasta subasta);
    Task<Subasta> ActualizarAsync(Subasta subasta);
    Task<List<Subasta>> ObtenerPorEstadoAsync(string estado);
    Task<List<Subasta>> ObtenerPorVendedorAsync(int vendedorId);
    Task<List<Subasta>> ObtenerTodasConPujasAsync();
}

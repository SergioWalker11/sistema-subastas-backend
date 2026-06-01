using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Interfaces;

public interface IRepositorioDenuncias
{
    Task<Denuncia> CrearAsync(Denuncia denuncia);
    Task<List<Denuncia>> ObtenerTodasAsync();
    Task<Denuncia?> ObtenerPorIdAsync(int id);
    Task<Denuncia> ActualizarAsync(Denuncia denuncia);
}

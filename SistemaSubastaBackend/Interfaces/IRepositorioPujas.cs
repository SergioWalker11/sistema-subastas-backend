using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Interfaces;

public interface IRepositorioPujas
{
    Task<List<Puja>> ObtenerPorSubastaAsync(int subastaId);
    Task<Puja> CrearAsync(Puja puja);
    Task<Puja?> ObtenerUltimaPujaAsync(int subastaId);
    Task<int> ContarPujasAsync(int subastaId);
}

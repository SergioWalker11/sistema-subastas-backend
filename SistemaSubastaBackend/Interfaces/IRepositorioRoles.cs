using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Interfaces;

public interface IRepositorioRoles
{
    Task<List<Rol>> ObtenerTodosAsync();
    Task<Rol?> ObtenerPorIdAsync(int id);
}

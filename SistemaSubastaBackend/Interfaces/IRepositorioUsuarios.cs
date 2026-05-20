using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Interfaces;

public interface IRepositorioUsuarios
{
    Task<Usuario?> ObtenerPorIdAsync(int id);
    Task<Usuario?> ObtenerPorCorreoAsync(string correo);
    Task<Usuario> CrearAsync(Usuario usuario);
}

using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Interfaces;

public interface IServicioAdmin
{
    Task<List<UsuarioDTO>> ListarUsuariosAsync();
    Task CambiarRolUsuarioAsync(int usuarioId, int rolId);
    Task CancelarSubastaAsync(int subastaId);
    Task<Usuario?> ObtenerUsuarioAsync(int usuarioId);
    Task ActualizarUsuarioAsync(Usuario usuario);
}

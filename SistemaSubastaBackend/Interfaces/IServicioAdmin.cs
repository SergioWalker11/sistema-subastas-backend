using SistemaSubastaBackend.DTOs;

namespace SistemaSubastaBackend.Interfaces;

public interface IServicioAdmin
{
    Task<List<UsuarioDTO>> ListarUsuariosAsync();
    Task CambiarRolUsuarioAsync(int usuarioId, int rolId);
    Task CancelarSubastaAsync(int subastaId);
}

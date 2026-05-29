using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Interfaces;

namespace SistemaSubastaBackend.Servicios;

public class ServicioAdmin : IServicioAdmin
{
    private readonly IRepositorioUsuarios _repoUsuarios;
    private readonly IRepositorioSubastas _repoSubastas;
    private readonly IRepositorioRoles _repoRoles;

    public ServicioAdmin(IRepositorioUsuarios repoUsuarios, IRepositorioSubastas repoSubastas, IRepositorioRoles repoRoles)
    {
        _repoUsuarios = repoUsuarios;
        _repoSubastas = repoSubastas;
        _repoRoles = repoRoles;
    }

    public async Task<List<UsuarioDTO>> ListarUsuariosAsync()
    {
        var usuarios = await _repoUsuarios.ObtenerTodosAsync();
        return usuarios.Select(u => new UsuarioDTO
        {
            Id = u.Id,
            NombreCompleto = u.NombreCompleto,
            Correo = u.Correo,
            RolId = u.RolId,
            RolNombre = u.Rol?.Nombre ?? string.Empty
        }).ToList();
    }

    public async Task CambiarRolUsuarioAsync(int usuarioId, int rolId)
    {
        var usuario = await _repoUsuarios.ObtenerPorIdAsync(usuarioId)
            ?? throw new KeyNotFoundException("Usuario no encontrado");

        var rol = await _repoRoles.ObtenerPorIdAsync(rolId)
            ?? throw new KeyNotFoundException("Rol no encontrado");

        usuario.RolId = rolId;
        await _repoUsuarios.ActualizarAsync(usuario);
    }

    public async Task CancelarSubastaAsync(int subastaId)
    {
        var subasta = await _repoSubastas.ObtenerPorIdAsync(subastaId)
            ?? throw new KeyNotFoundException("Subasta no encontrada");

        subasta.Estado = "cancelada";
        await _repoSubastas.ActualizarAsync(subasta);
    }
}

using SistemaSubastaBackend.DTOs;

namespace SistemaSubastaBackend.Interfaces;

public interface IServicioAutenticacion
{
    Task<AuthRespuestaDTO> LoginAsync(LoginDTO dto);
    Task<AuthRespuestaDTO> RegistroAsync(RegistroDTO dto);
}

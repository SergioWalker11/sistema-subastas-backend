using SistemaSubastaBackend.DTOs;

namespace SistemaSubastaBackend.Interfaces;

public interface IServicioPujas
{
    Task<PujaRespuestaDTO> RegistrarPujaAsync(PujaCrearDTO dto);
    Task<List<PujaRespuestaDTO>> ObtenerHistorialAsync(int subastaId, int? usuarioId = null);
}

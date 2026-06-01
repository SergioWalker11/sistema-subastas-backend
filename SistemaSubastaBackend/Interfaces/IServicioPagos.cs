using SistemaSubastaBackend.DTOs;

namespace SistemaSubastaBackend.Interfaces;

public interface IServicioPagos
{
    Task<PagoRespuestaDTO> ProcesarPagoAsync(PagoCrearDTO dto);
    Task<PagoRespuestaDTO> ProcesarPagoConTarjetaAsync(PagoTarjetaDTO dto);
    Task<PagoRespuestaDTO?> ObtenerPagoAsync(int id);
    Task<List<PagoRespuestaDTO>> ObtenerPagosUsuarioAsync(int usuarioId);
}

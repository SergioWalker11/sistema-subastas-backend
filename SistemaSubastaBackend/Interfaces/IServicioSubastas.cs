using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Interfaces;

public interface IServicioSubastas
{
    Task<List<SubastaDetalleDTO>> ListarSubastasAsync();
    Task<SubastaDetalleDTO?> ObtenerDetalleAsync(int id);
    Task<Subasta> CrearSubastaAsync(SubastaCrearDTO dto);
    Task<Subasta> ActualizarEstadoAsync(int id, string estado);
    Task<Subasta> EditarSubastaAsync(int id, SubastaEditarDTO dto);
    Task<Subasta> CancelarSubastaAsync(int id, int vendedorId);
    Task<List<SubastaDetalleDTO>> ListarPorVendedorAsync(int vendedorId);
    Task<List<SubastaGanadaDTO>> ListarGanadasPorUsuarioAsync(int usuarioId);
    Task<List<SubastaGanadaDTO>> ListarPendientesPagoAsync(int usuarioId);
    Task<List<VentaDTO>> ListarVentasAsync(int vendedorId);
}

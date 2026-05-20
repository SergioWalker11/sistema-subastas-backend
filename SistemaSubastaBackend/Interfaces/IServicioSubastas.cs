using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Interfaces;

public interface IServicioSubastas
{
    Task<List<SubastaDetalleDTO>> ListarSubastasAsync();
    Task<SubastaDetalleDTO?> ObtenerDetalleAsync(int id);
    Task<Subasta> CrearSubastaAsync(SubastaCrearDTO dto);
    Task<Subasta> ActualizarEstadoAsync(int id, string estado);
}

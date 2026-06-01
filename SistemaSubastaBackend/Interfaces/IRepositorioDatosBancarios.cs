using SistemaSubastaBackend.DTOs;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Interfaces;

public interface IRepositorioDatosBancarios
{
    Task<DatosBancarios?> ObtenerPorUsuarioAsync(int usuarioId);
    Task<DatosBancarios> GuardarAsync(DatosBancarios datos);
}

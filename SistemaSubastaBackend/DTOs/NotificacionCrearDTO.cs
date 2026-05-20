namespace SistemaSubastaBackend.DTOs;

public class NotificacionCrearDTO
{
    public int UsuarioId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
}

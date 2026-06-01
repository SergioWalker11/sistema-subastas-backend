namespace SistemaSubastaBackend.DTOs;

public class PerfilDTO
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public bool EstaSuspendido { get; set; }
    public DatosBancariosDTO? DatosBancarios { get; set; }
}

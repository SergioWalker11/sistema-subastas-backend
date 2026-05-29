namespace SistemaSubastaBackend.DTOs;

public class RegistroDTO
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
    public int RolId { get; set; }
}

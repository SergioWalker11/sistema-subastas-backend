namespace SistemaSubastaBackend.Modelos;

public class Denuncia
{
    public int Id { get; set; }
    public int DenuncianteId { get; set; }
    public Usuario Denunciante { get; set; } = null!;
    public int DenunciadoId { get; set; }
    public Usuario Denunciado { get; set; } = null!;
    public string Motivo { get; set; } = string.Empty;
    public string Estado { get; set; } = "pendiente"; // pendiente, resuelta, rechazada
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}

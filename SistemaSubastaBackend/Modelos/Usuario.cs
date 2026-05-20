namespace SistemaSubastaBackend.Modelos;

public class Usuario
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string ContrasenaHash { get; set; } = string.Empty;
    public int RolId { get; set; }
    public Rol Rol { get; set; } = null!;
    public ICollection<Puja> Pujas { get; set; } = new List<Puja>();
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    public ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
}

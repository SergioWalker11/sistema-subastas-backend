namespace SistemaSubastaBackend.Modelos;

public class DatosBancarios
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public string Banco { get; set; } = string.Empty;
    public string TipoCuenta { get; set; } = "ahorros";
    public string NumeroCuenta { get; set; } = string.Empty;
    public string Titular { get; set; } = string.Empty;
}

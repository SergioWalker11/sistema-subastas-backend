using System.Text.RegularExpressions;

namespace SistemaSubastaBackend.Utilidades;

public static partial class ValidadorEntrada
{
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex RegexCorreo();

    public static List<string> ValidarLogin(string correo, string contrasena)
    {
        var errores = new List<string>();
        if (string.IsNullOrWhiteSpace(correo)) errores.Add("El correo es obligatorio");
        else if (!RegexCorreo().IsMatch(correo)) errores.Add("El formato del correo no es valido");
        if (string.IsNullOrWhiteSpace(contrasena)) errores.Add("La contrasena es obligatoria");
        return errores;
    }

    public static List<string> ValidarRegistro(string nombre, string correo, string contrasena)
    {
        var errores = new List<string>();
        if (string.IsNullOrWhiteSpace(nombre)) errores.Add("El nombre es obligatorio");
        if (string.IsNullOrWhiteSpace(correo)) errores.Add("El correo es obligatorio");
        else if (!RegexCorreo().IsMatch(correo)) errores.Add("El formato del correo no es valido");
        if (string.IsNullOrWhiteSpace(contrasena)) errores.Add("La contrasena es obligatoria");
        else if (contrasena.Length < 6) errores.Add("La contrasena debe tener al menos 6 caracteres");
        return errores;
    }

    public static List<string> ValidarMonto(decimal monto)
    {
        var errores = new List<string>();
        if (monto <= 0) errores.Add("El monto debe ser mayor a cero");
        return errores;
    }
}

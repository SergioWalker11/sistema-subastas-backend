using System;
using System.Collections.Generic;

namespace SistemaSubastaBackend.Refactorizacion;

public class SubastasRefactorizado
{
    private const int MaxNombreLongitud = 100;
    private const decimal PrecioMaximo = 100000m;
    private const int DuracionMaximaDias = 30;

    public ResultadoSubasta CrearSubasta(string nombre, decimal precio, int diasDuracion, string categoria)
    {
        // 1. Validacion centralizada
        var errores = ValidarDatos(nombre, precio, diasDuracion);
        if (errores.Count > 0)
            return ResultadoSubasta.Fallo(errores);

        // 2. Logica de negocio separada
        var estado = DeterminarEstado(categoria);
        var fechaFin = DateTime.Now.AddDays(diasDuracion);

        return ResultadoSubasta.Exitoso(nombre, precio, fechaFin, estado);
    }

    private List<string> ValidarDatos(string nombre, decimal precio, int diasDuracion)
    {
        var errores = new List<string>();
        if (string.IsNullOrWhiteSpace(nombre)) errores.Add("Nombre requerido");
        else if (nombre.Length > MaxNombreLongitud) errores.Add("Nombre muy largo");

        if (precio <= 0) errores.Add("Precio debe ser mayor a 0");
        else if (precio > PrecioMaximo) errores.Add($"Precio excede {PrecioMaximo} Bs");

        if (diasDuracion < 1) errores.Add("Duracion minima 1 dia");
        else if (diasDuracion > DuracionMaximaDias) errores.Add($"Duracion maxima {DuracionMaximaDias} dias");

        return errores;
    }

    private string DeterminarEstado(string categoria)
    {
        // Categorias que requieren revision previa
        var categoriasCriticas = new[] { "electronica", "vehiculos" };
        return Array.Exists(categoriasCriticas, c => c == categoria) ? "revision" : "activa";
    }
}

public class ResultadoSubasta
{
    public bool Exito { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public List<string> Errores { get; set; } = new();
    public string? Nombre { get; set; }
    public decimal Precio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string? Estado { get; set; }

    public static ResultadoSubasta Exitoso(string nombre, decimal precio, DateTime fechaFin, string estado) => new()
    { Exito = true, Mensaje = "Subasta creada", Nombre = nombre, Precio = precio, FechaFin = fechaFin, Estado = estado };

    public static ResultadoSubasta Fallo(List<string> errores) => new() { Exito = false, Errores = errores };
}

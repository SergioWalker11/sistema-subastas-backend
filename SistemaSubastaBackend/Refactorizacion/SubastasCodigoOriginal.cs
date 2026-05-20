using System;
using System.Collections.Generic;

namespace SistemaSubastaBackend.Refactorizacion;

public class SubastasCodigoOriginal
{
    public Dictionary<string, object> CrearSubasta(string nombre, decimal precio, int diasDuracion, string categoria)
    {
        var respuesta = new Dictionary<string, object>();

        // Validacion inline y dispersa
        if (nombre == "" || nombre == null) { respuesta.Add("exito", false); respuesta.Add("error", "Nombre requerido"); return respuesta; }
        if (nombre.Length > 100) { respuesta.Add("exito", false); respuesta.Add("error", "Nombre muy largo"); return respuesta; }
        
        if (precio <= 0) { respuesta.Add("exito", false); respuesta.Add("error", "Precio debe ser mayor a 0"); return respuesta; }
        if (precio > 100000) { respuesta.Add("exito", false); respuesta.Add("error", "Precio excede 100,000 Bs"); return respuesta; }

        if (diasDuracion < 1) { respuesta.Add("exito", false); respuesta.Add("error", "Duracion minima 1 dia"); return respuesta; }
        if (diasDuracion > 30) { respuesta.Add("exito", false); respuesta.Add("error", "Duracion maxima 30 dias"); return respuesta; }

        // Logica de categoria con switch
        string estado = "activa";
        if (categoria == "electronica" || categoria == "vehiculos")
        {
            estado = "revision"; // Requiere revision
        }

        DateTime fechaFin = DateTime.Now.AddDays(diasDuracion);

        respuesta.Add("exito", true);
        respuesta.Add("mensaje", "Subasta creada");
        respuesta.Add("nombre", nombre);
        respuesta.Add("precio", precio);
        respuesta.Add("fechaFin", fechaFin);
        respuesta.Add("estado", estado);

        return respuesta;
    }
}

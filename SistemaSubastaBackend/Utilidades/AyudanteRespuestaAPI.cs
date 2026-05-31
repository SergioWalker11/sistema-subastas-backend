using System.Text.Json;

namespace SistemaSubastaBackend.Utilidades;

public static class AyudanteRespuestaAPI
{
    public static Dictionary<string, object> RespuestaExito(object datos, string mensaje = "Operacion exitosa")
    {
        return new Dictionary<string, object>
        {
            ["exito"] = true,
            ["mensaje"] = mensaje,
            ["datos"] = datos
        };
    }

    public static Dictionary<string, object> RespuestaError(string mensaje, int codigo = 400)
    {
        return new Dictionary<string, object>
        {
            ["exito"] = false,
            ["mensaje"] = mensaje,
            ["codigo"] = codigo
        };
    }

}

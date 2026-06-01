using SistemaSubastaBackend.Datos;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Datos;

public static class SembradorDatos
{
    public static void Sembrar(ContextoSubastas contexto)
    {
        if (contexto.Usuarios.Any()) return;

        var rolAdmin = new Rol { Nombre = "administrador", Descripcion = "Administrador del sistema" };
        var rolVendedor = new Rol { Nombre = "vendedor", Descripcion = "Usuario que publica subastas" };
        var rolComprador = new Rol { Nombre = "comprador", Descripcion = "Usuario que realiza pujas" };
        contexto.Roles.AddRange(rolAdmin, rolVendedor, rolComprador);
        contexto.SaveChanges();

        var hash = BCrypt.Net.BCrypt.HashPassword("123456");

        var admin = new Usuario { NombreCompleto = "Administrador", Correo = "admin@gmail.com", ContrasenaHash = hash, RolId = rolAdmin.Id };
        var vendedor = new Usuario { NombreCompleto = "Vendedor", Correo = "vendedor@gmail.com", ContrasenaHash = hash, RolId = rolVendedor.Id };
        var comprador = new Usuario { NombreCompleto = "Comprador", Correo = "comprador@gmail.com", ContrasenaHash = hash, RolId = rolComprador.Id };
        contexto.Usuarios.AddRange(admin, vendedor, comprador);
        contexto.SaveChanges();
    }
}

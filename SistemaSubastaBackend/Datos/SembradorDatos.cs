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

        var usuario1 = new Usuario { NombreCompleto = "Carlos Mendez", Correo = "carlos@demo.com", ContrasenaHash = hash, RolId = rolComprador.Id };
        var usuario2 = new Usuario { NombreCompleto = "Ana Lopez", Correo = "ana@demo.com", ContrasenaHash = hash, RolId = rolVendedor.Id };
        var usuario3 = new Usuario { NombreCompleto = "Pedro Ramirez", Correo = "pedro@demo.com", ContrasenaHash = hash, RolId = rolComprador.Id };
        var usuario4 = new Usuario { NombreCompleto = "Maria Torres", Correo = "maria@demo.com", ContrasenaHash = hash, RolId = rolComprador.Id };
        var usuario5 = new Usuario { NombreCompleto = "Luis Garcia", Correo = "luis@demo.com", ContrasenaHash = hash, RolId = rolVendedor.Id };
        contexto.Usuarios.AddRange(usuario1, usuario2, usuario3, usuario4, usuario5);
        contexto.SaveChanges();

        var catElectronica = new Categoria { Nombre = "Electrónica", Descripcion = "Dispositivos electrónicos y gadgets" };
        var catVehiculos = new Categoria { Nombre = "Vehículos", Descripcion = "Automóviles, motos y bicicletas" };
        var catFotografia = new Categoria { Nombre = "Fotografía", Descripcion = "Cámaras, lentes y accesorios" };
        var catAudio = new Categoria { Nombre = "Audio", Descripcion = "Equipos de sonido y auriculares" };
        contexto.Categorias.AddRange(catElectronica, catVehiculos, catFotografia, catAudio);
        contexto.SaveChanges();

        var producto1 = new Producto { Nombre = "Laptop Gaming ASUS ROG", Descripcion = "Laptop gaming con procesador Intel i7, 16GB RAM, RTX 3060, pantalla 15.6 pulgadas 144Hz", CategoriaId = catElectronica.Id };
        var producto2 = new Producto { Nombre = "iPhone 15 Pro Max", Descripcion = "Apple iPhone 15 Pro Max 256GB, color titanio natural, nuevo sellado", CategoriaId = catElectronica.Id };
        var producto3 = new Producto { Nombre = "PlayStation 5 Edicion Digital", Descripcion = "Consola PlayStation 5 edition digital, incluye control DualSense", CategoriaId = catElectronica.Id };
        var producto4 = new Producto { Nombre = "Bicicleta Montaña Trek", Descripcion = "Bicicleta de montaña Trek Marlin 7, cuadro aluminio, suspension delantera", CategoriaId = catVehiculos.Id };
        var producto5 = new Producto { Nombre = "Camara Canon EOS R6", Descripcion = "Camara mirrorless Canon EOS R6 Mark II, cuerpo solamente, 24.2MP", CategoriaId = catFotografia.Id };
        var producto6 = new Producto { Nombre = "Monitor Samsung 4K 32\"", Descripcion = "Monitor Samsung UHD 4K de 32 pulgadas, panel IPS, HDR10, USB-C", CategoriaId = catElectronica.Id };
        var producto7 = new Producto { Nombre = "Auriculares Sony WH-1000XM5", Descripcion = "Auriculares inalambricos con cancelacion de ruido, 30 horas de bateria", CategoriaId = catAudio.Id };
        var producto8 = new Producto { Nombre = "Tablet iPad Air M2", Descripcion = "iPad Air con chip M2, 256GB, pantalla 11 pulgadas, color azul", CategoriaId = catElectronica.Id };
        contexto.Productos.AddRange(producto1, producto2, producto3, producto4, producto5, producto6, producto7, producto8);
        contexto.SaveChanges();

        var ahora = DateTime.UtcNow;
        var subasta1 = new Subasta { ProductoId = producto1.Id, VendedorId = usuario2.Id, PrecioInicial = 800.00m, PrecioActual = 950.00m, FechaInicio = ahora.AddDays(-2), FechaFin = ahora.AddDays(5), Estado = "activa" };
        var subasta2 = new Subasta { ProductoId = producto2.Id, VendedorId = usuario2.Id, PrecioInicial = 900.00m, PrecioActual = 1050.00m, FechaInicio = ahora.AddDays(-1), FechaFin = ahora.AddDays(3), Estado = "activa" };
        var subasta3 = new Subasta { ProductoId = producto3.Id, VendedorId = usuario5.Id, PrecioInicial = 350.00m, PrecioActual = 420.00m, FechaInicio = ahora.AddDays(-3), FechaFin = ahora.AddDays(2), Estado = "activa" };
        var subasta4 = new Subasta { ProductoId = producto4.Id, VendedorId = usuario5.Id, PrecioInicial = 500.00m, PrecioActual = 650.00m, FechaInicio = ahora.AddDays(-5), FechaFin = ahora.AddDays(-1), Estado = "finalizada" };
        var subasta5 = new Subasta { ProductoId = producto5.Id, VendedorId = usuario2.Id, PrecioInicial = 1200.00m, PrecioActual = 1350.00m, FechaInicio = ahora.AddDays(-1), FechaFin = ahora.AddDays(7), Estado = "activa" };
        var subasta6 = new Subasta { ProductoId = producto6.Id, VendedorId = usuario5.Id, PrecioInicial = 300.00m, PrecioActual = 380.00m, FechaInicio = ahora, FechaFin = ahora.AddDays(4), Estado = "activa" };
        var subasta7 = new Subasta { ProductoId = producto7.Id, VendedorId = usuario2.Id, PrecioInicial = 200.00m, PrecioActual = 250.00m, FechaInicio = ahora.AddDays(-2), FechaFin = ahora.AddDays(1), Estado = "activa" };
        var subasta8 = new Subasta { ProductoId = producto8.Id, VendedorId = usuario5.Id, PrecioInicial = 500.00m, PrecioActual = 580.00m, FechaInicio = ahora.AddDays(-1), FechaFin = ahora.AddDays(6), Estado = "activa" };
        contexto.Subastas.AddRange(subasta1, subasta2, subasta3, subasta4, subasta5, subasta6, subasta7, subasta8);
        contexto.SaveChanges();

        var puja1 = new Puja { SubastaId = subasta1.Id, UsuarioId = usuario1.Id, Monto = 850.00m, FechaCreacion = ahora.AddDays(-1) };
        var puja2 = new Puja { SubastaId = subasta1.Id, UsuarioId = usuario3.Id, Monto = 900.00m, FechaCreacion = ahora.AddHours(-12) };
        var puja3 = new Puja { SubastaId = subasta1.Id, UsuarioId = usuario4.Id, Monto = 950.00m, FechaCreacion = ahora.AddHours(-2) };
        var puja4 = new Puja { SubastaId = subasta2.Id, UsuarioId = usuario3.Id, Monto = 950.00m, FechaCreacion = ahora.AddHours(-20) };
        var puja5 = new Puja { SubastaId = subasta2.Id, UsuarioId = usuario1.Id, Monto = 1000.00m, FechaCreacion = ahora.AddHours(-10) };
        var puja6 = new Puja { SubastaId = subasta2.Id, UsuarioId = usuario4.Id, Monto = 1050.00m, FechaCreacion = ahora.AddHours(-3) };
        var puja7 = new Puja { SubastaId = subasta3.Id, UsuarioId = usuario1.Id, Monto = 380.00m, FechaCreacion = ahora.AddDays(-2) };
        var puja8 = new Puja { SubastaId = subasta3.Id, UsuarioId = usuario4.Id, Monto = 400.00m, FechaCreacion = ahora.AddDays(-1) };
        var puja9 = new Puja { SubastaId = subasta3.Id, UsuarioId = usuario3.Id, Monto = 420.00m, FechaCreacion = ahora.AddHours(-6) };
        var puja10 = new Puja { SubastaId = subasta5.Id, UsuarioId = usuario3.Id, Monto = 1250.00m, FechaCreacion = ahora.AddHours(-18) };
        var puja11 = new Puja { SubastaId = subasta5.Id, UsuarioId = usuario1.Id, Monto = 1300.00m, FechaCreacion = ahora.AddHours(-8) };
        var puja12 = new Puja { SubastaId = subasta5.Id, UsuarioId = usuario4.Id, Monto = 1350.00m, FechaCreacion = ahora.AddHours(-1) };
        var puja13 = new Puja { SubastaId = subasta6.Id, UsuarioId = usuario4.Id, Monto = 340.00m, FechaCreacion = ahora.AddHours(-22) };
        var puja14 = new Puja { SubastaId = subasta6.Id, UsuarioId = usuario1.Id, Monto = 380.00m, FechaCreacion = ahora.AddHours(-5) };
        var puja15 = new Puja { SubastaId = subasta7.Id, UsuarioId = usuario3.Id, Monto = 220.00m, FechaCreacion = ahora.AddDays(-1) };
        var puja16 = new Puja { SubastaId = subasta7.Id, UsuarioId = usuario1.Id, Monto = 250.00m, FechaCreacion = ahora.AddHours(-4) };
        var puja17 = new Puja { SubastaId = subasta8.Id, UsuarioId = usuario4.Id, Monto = 540.00m, FechaCreacion = ahora.AddHours(-15) };
        var puja18 = new Puja { SubastaId = subasta8.Id, UsuarioId = usuario3.Id, Monto = 580.00m, FechaCreacion = ahora.AddHours(-7) };
        contexto.Pujas.AddRange(puja1, puja2, puja3, puja4, puja5, puja6, puja7, puja8, puja9, puja10, puja11, puja12, puja13, puja14, puja15, puja16, puja17, puja18);
        contexto.SaveChanges();

        var pago1 = new Pago { SubastaId = subasta4.Id, UsuarioId = usuario3.Id, Monto = 650.00m, CodigoTransaccion = "SUBASTA-20260517120000-1234", EstadoPago = "aprobado", FechaPago = ahora.AddDays(-1) };
        contexto.Pagos.Add(pago1);
        contexto.SaveChanges();

        var notificacion1 = new Notificacion { UsuarioId = usuario1.Id, Titulo = "Puja superada", Mensaje = "Alguien ha superado tu puja en Laptop Gaming ASUS ROG", Leida = false, FechaCreacion = ahora.AddHours(-2) };
        var notificacion2 = new Notificacion { UsuarioId = usuario3.Id, Titulo = "Subasta ganada", Mensaje = "Has ganado la subasta de PlayStation 5 Edicion Digital", Leida = false, FechaCreacion = ahora.AddDays(-1) };
        var notificacion3 = new Notificacion { UsuarioId = usuario4.Id, Titulo = "Pago procesado", Mensaje = "Tu pago de $650.00 ha sido procesado exitosamente", Leida = true, FechaCreacion = ahora.AddDays(-1) };
        var notificacion4 = new Notificacion { UsuarioId = usuario1.Id, Titulo = "Nueva puja recibida", Mensaje = "Se ha recibido una puja en Monitor Samsung 4K 32\"", Leida = false, FechaCreacion = ahora.AddHours(-5) };
        var notificacion5 = new Notificacion { UsuarioId = usuario3.Id, Titulo = "Subasta por finalizar", Mensaje = "La subasta de Bicicleta Montaña Trek finaliza pronto", Leida = true, FechaCreacion = ahora.AddDays(-2) };
        contexto.Notificaciones.AddRange(notificacion1, notificacion2, notificacion3, notificacion4, notificacion5);
        contexto.SaveChanges();
    }
}

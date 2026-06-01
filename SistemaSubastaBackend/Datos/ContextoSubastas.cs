using Microsoft.EntityFrameworkCore;
using SistemaSubastaBackend.Modelos;

namespace SistemaSubastaBackend.Datos;

public class ContextoSubastas : DbContext
{
    public ContextoSubastas(DbContextOptions<ContextoSubastas> opciones) : base(opciones)
    {
    }

    public DbSet<Rol> Roles { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<ImagenProducto> ImagenesProducto { get; set; }
    public DbSet<Subasta> Subastas { get; set; }
    public DbSet<Puja> Pujas { get; set; }
    public DbSet<Pago> Pagos { get; set; }
    public DbSet<Notificacion> Notificaciones { get; set; }
    public DbSet<DatosBancarios> DatosBancarios { get; set; }
    public DbSet<Denuncia> Denuncias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("roles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(200);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("usuarios");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Correo).IsUnique();
            entity.Property(e => e.NombreCompleto).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Correo).IsRequired().HasMaxLength(150);
            entity.Property(e => e.ContrasenaHash).IsRequired().HasMaxLength(255);
            entity.Property(e => e.EstaSuspendido).IsRequired();
            entity.HasOne(e => e.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(e => e.RolId);
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("categorias");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(200);
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("productos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.HasOne(e => e.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(e => e.CategoriaId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ImagenProducto>(entity =>
        {
            entity.ToTable("imagenes_producto");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RutaArchivo).IsRequired().HasMaxLength(500);
            entity.HasOne(e => e.Producto)
                .WithMany(p => p.Imagenes)
                .HasForeignKey(e => e.ProductoId);
        });

        modelBuilder.Entity<Subasta>(entity =>
        {
            entity.ToTable("subastas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PrecioInicial).HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.PrecioActual).HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.FechaInicio).IsRequired();
            entity.Property(e => e.FechaFin).IsRequired();
            entity.Property(e => e.FechaLimitePago);
            entity.Property(e => e.Estado).IsRequired().HasMaxLength(20);
            entity.HasOne(e => e.Producto)
                .WithMany(p => p.Subastas)
                .HasForeignKey(e => e.ProductoId);
            entity.HasOne(e => e.Vendedor)
                .WithMany(u => u.Subastas)
                .HasForeignKey(e => e.VendedorId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Ganador)
                .WithMany()
                .HasForeignKey(e => e.GanadorId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Puja>(entity =>
        {
            entity.ToTable("pujas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Monto).HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.FechaCreacion).IsRequired();
            entity.HasOne(e => e.Subasta)
                .WithMany(s => s.Pujas)
                .HasForeignKey(e => e.SubastaId);
            entity.HasOne(e => e.Usuario)
                .WithMany(u => u.Pujas)
                .HasForeignKey(e => e.UsuarioId);
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.ToTable("pagos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Monto).HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.CodigoTransaccion).IsRequired().HasMaxLength(50);
            entity.Property(e => e.EstadoPago).IsRequired().HasMaxLength(20);
            entity.Property(e => e.FechaPago).IsRequired();
            entity.HasOne(e => e.Subasta)
                .WithMany(s => s.Pagos)
                .HasForeignKey(e => e.SubastaId);
            entity.HasOne(e => e.Usuario)
                .WithMany(u => u.Pagos)
                .HasForeignKey(e => e.UsuarioId);
        });

        modelBuilder.Entity<Notificacion>(entity =>
        {
            entity.ToTable("notificaciones");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Titulo).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Mensaje).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Leida).IsRequired();
            entity.Property(e => e.FechaCreacion).IsRequired();
            entity.HasOne(e => e.Usuario)
                .WithMany(u => u.Notificaciones)
                .HasForeignKey(e => e.UsuarioId);
        });

        modelBuilder.Entity<DatosBancarios>(entity =>
        {
            entity.ToTable("datos_bancarios");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UsuarioId).IsUnique();
            entity.Property(e => e.Banco).IsRequired().HasMaxLength(100);
            entity.Property(e => e.TipoCuenta).IsRequired().HasMaxLength(20);
            entity.Property(e => e.NumeroCuenta).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Titular).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.Usuario)
                .WithOne(u => u.DatosBancarios)
                .HasForeignKey<DatosBancarios>(e => e.UsuarioId);
        });

        modelBuilder.Entity<Denuncia>(entity =>
        {
            entity.ToTable("denuncias");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Motivo).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Estado).IsRequired().HasMaxLength(20);
            entity.Property(e => e.FechaCreacion).IsRequired();
            entity.HasOne(e => e.Denunciante)
                .WithMany()
                .HasForeignKey(e => e.DenuncianteId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Denunciado)
                .WithMany()
                .HasForeignKey(e => e.DenunciadoId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

using Microsoft.EntityFrameworkCore;

namespace SistemaABM_Libros_Data.Models;

public partial class BDSistemaLibrosContext : DbContext
{
    public BDSistemaLibrosContext()
    {
    }

    public BDSistemaLibrosContext(DbContextOptions<BDSistemaLibrosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<DetallePedido> DetallePedidos { get; set; }

    public virtual DbSet<Libro> Libros { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Subcategoria> Subcategorias { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId).HasName("PK__Categori__F353C1C581919E38");

            entity.HasIndex(e => e.NombreCategoria, "UQ__Categori__A21FBE9FDB5DF97A").IsUnique();

            entity.Property(e => e.CategoriaId).HasColumnName("CategoriaID");
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.NombreCategoria).HasMaxLength(100);
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.DetallePedidoId).HasName("PK__Detalle___6ED21C01FA42525B");

            entity.ToTable("Detalle_Pedido");

            entity.HasIndex(e => new { e.PedidoId, e.LibroId }, "UQ__Detalle___CAE00AD99DD0A4A5").IsUnique();

            entity.Property(e => e.DetallePedidoId).HasColumnName("DetallePedidoID");
            entity.Property(e => e.LibroId).HasColumnName("LibroID");
            entity.Property(e => e.PedidoId).HasColumnName("PedidoID");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Libro).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.LibroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Detalle_P__Libro__5535A963");

            entity.HasOne(d => d.Pedido).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Detalle_P__Pedid__5441852A");
        });

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.HasKey(e => e.LibroId).HasName("PK__Libros__35A1EC8D5718D6FF");

            entity.HasIndex(e => e.Isbn, "UQ__Libros__447D36EAED35E491").IsUnique();

            entity.Property(e => e.LibroId).HasColumnName("LibroID");
            entity.Property(e => e.Autor).HasMaxLength(255);
            entity.Property(e => e.Editorial).HasMaxLength(100);
            entity.Property(e => e.EstadoLibro).HasMaxLength(50);
            entity.Property(e => e.Idioma).HasMaxLength(50);
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .HasColumnName("ISBN");
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SubcategoriaId).HasColumnName("SubcategoriaID");
            entity.Property(e => e.TipoLibro).HasMaxLength(10);
            entity.Property(e => e.Titulo).HasMaxLength(255);

            entity.HasOne(d => d.Subcategoria).WithMany(p => p.Libros)
                .HasForeignKey(d => d.SubcategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Libros__Subcateg__4316F928");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.PedidoId).HasName("PK__Pedidos__09BA14109566092E");

            entity.Property(e => e.PedidoId).HasColumnName("PedidoID");
            entity.Property(e => e.DireccionEnvio).HasMaxLength(500);
            entity.Property(e => e.EstadoPedido)
                .HasMaxLength(50)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaPedido)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TotalPedido).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pedidos__Usuario__4D94879B");
        });

        modelBuilder.Entity<Subcategoria>(entity =>
        {
            entity.HasKey(e => e.SubcategoriaId).HasName("PK__Subcateg__2FEBBB02560B2A90");

            entity.HasIndex(e => e.NombreSubcategoria, "UQ__Subcateg__955F676A84D21A40").IsUnique();

            entity.Property(e => e.SubcategoriaId).HasColumnName("SubcategoriaID");
            entity.Property(e => e.CategoriaId).HasColumnName("CategoriaID");
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.NombreSubcategoria).HasMaxLength(100);

            entity.HasOne(d => d.Categoria).WithMany(p => p.Subcategoria)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subcatego__Categ__3B75D760");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE7987176CF22");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D1053445877BF7").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Apellido).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.EsCliente).HasDefaultValue(true);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

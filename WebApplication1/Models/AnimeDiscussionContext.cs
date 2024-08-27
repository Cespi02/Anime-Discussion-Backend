using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class AnimeDiscussionContext : DbContext
{
    public AnimeDiscussionContext()
    {
    }

    public AnimeDiscussionContext(DbContextOptions<AnimeDiscussionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Anime> Animes { get; set; }

    public virtual DbSet<CategoriasPrestamo> CategoriasPrestamos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Prestamo> Prestamos { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Server=tcp:animediscussiontest.database.windows.net,1433;Initial Catalog=AnimeDiscussion;User ID=cespi02;Password=1161928662Ll!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CI_AS");

        modelBuilder.Entity<Anime>(entity =>
        {
            entity.HasKey(e => e.IdAnime).HasName("PK__Animes__A4BCC06A3B739BB5");

            entity.Property(e => e.IdAnime).HasColumnName("idAnime");
            entity.Property(e => e.Imagen).HasColumnName("imagen");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Texto)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("texto");
        });

        modelBuilder.Entity<CategoriasPrestamo>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__CD54BC5A270D5262");

            entity.ToTable("Categorias_prestamos");

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Limite).HasColumnName("limite");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Clientes__677F38F5D54ECFA4");

            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Contrasenia)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("contrasenia");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_usuario");
            entity.Property(e => e.NroDoc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nro_doc");
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.IdComentario).HasName("PK__Comentar__1BA6C6F41317B2E0");

            entity.Property(e => e.IdComentario).HasColumnName("id_comentario");
            entity.Property(e => e.Contenido)
                .HasMaxLength(1024)
                .IsUnicode(false)
                .HasColumnName("contenido");
            entity.Property(e => e.Fechayhora)
                .HasColumnType("datetime")
                .HasColumnName("fechayhora");
            entity.Property(e => e.IdAnime).HasColumnName("id_anime");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Imagen).HasColumnName("imagen");

            entity.HasOne(d => d.IdAnimeNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.IdAnime)
                .HasConstraintName("FK_COMEN_ANIME");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_COMEN_CLIE");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK__Empleado__88B5139451F7697B");

            entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");
            entity.Property(e => e.Contrasenia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contrasenia");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.NroDoc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nro_doc");
            entity.Property(e => e.Sueldo).HasColumnName("sueldo");
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.HasKey(e => e.IdPrestamo).HasName("PK__Prestamo__5E87BE274D41F3E7");

            entity.Property(e => e.IdPrestamo).HasColumnName("id_prestamo");
            entity.Property(e => e.Cuotas).HasColumnName("cuotas");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Monto).HasColumnName("monto");
            entity.Property(e => e.PrimerVencimiento).HasColumnName("primer_vencimiento");
            entity.Property(e => e.Punitorios).HasColumnName("punitorios");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("FK_PRES_CATEG");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

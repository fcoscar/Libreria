using Libreria.Applications.Interfaces;
using Libreria.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Libreria.Infrastructure;

public class MainDbContext : DbContext
{


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autores>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Nombre)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(a => a.Nacionalidad)
                .IsRequired()
                .HasMaxLength(100);
        });
        
        modelBuilder.Entity<Libros>(entity =>
        {
            entity.HasKey(l => l.Id);
            
            entity.Property(l => l.Titulo)
                .IsRequired()
                .HasMaxLength(300);
            
            entity.Property(l => l.AnoPublicacion)
                .IsRequired();
            
            entity.Property(l => l.Genero)
                .HasMaxLength(100);

            entity.HasOne(l => l.Autor)
                .WithMany(a => a.Libros)
                .HasForeignKey(l => l.AutorId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<Prestamos>(entity =>
        {
            entity.HasKey(p => p.Id);
            
            entity.Property(p => p.FechaPrestamo)
                .IsRequired();

            entity.HasOne(p => p.Libro)
                .WithMany(l => l.Prestamos)
                .HasForeignKey(p => p.LibroId)
                .OnDelete(DeleteBehavior.Restrict);
            
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(u => u.Id);
            
            entity.Property(u => u.NombreUsuario)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(u => u.Contrasena)
                .IsRequired();
            
            entity.Property(u => u.Rol)
                .IsRequired()
                .HasMaxLength(20);

        });
    }

    public DbSet<Autores> Autores { get; set; }
    public DbSet<Libros> Libros { get; set; }
    public DbSet<Prestamos> Prestamos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
}
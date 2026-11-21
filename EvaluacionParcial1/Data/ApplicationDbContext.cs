using Microsoft.EntityFrameworkCore;
using EvaluacionParcial1.Models;
namespace EvaluacionParcial1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Libro> Libros => Set<Libro>();
        public DbSet<Autor> Autores => Set<Autor>();
        public DbSet<LibroAutor> LibroAutores => Set<LibroAutor>();
        public DbSet<Miembro> Miembros => Set<Miembro>();
        public DbSet<Prestamo> Prestamos => Set<Prestamo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LibroAutor>()
                .HasIndex(la => new { la.LibroId, la.AutorId })
                .IsUnique();
        }
    }
}

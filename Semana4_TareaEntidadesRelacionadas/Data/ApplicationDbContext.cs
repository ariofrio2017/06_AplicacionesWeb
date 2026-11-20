using Microsoft.EntityFrameworkCore;
using Semana4_TareaEntidadesRelacionadas.Models;
namespace Semana4_TareaEntidadesRelacionadas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Facultad> Facultades { get; set; }
        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Solución para la advertencia del decimal
            modelBuilder.Entity<Matricula>()
                .Property(m => m.CostoMatricula)
                .HasPrecision(18, 2);
        }
    }
}

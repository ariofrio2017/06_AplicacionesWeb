using Microsoft.EntityFrameworkCore;
using WebApplicationSemana2.Models;

namespace WebApplicationSemana2.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor normal para Program.cs
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
       
        public ApplicationDbContext()
        {
        }

        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-7M1M5LM;Database=DbClientesSemana2;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
    }
}
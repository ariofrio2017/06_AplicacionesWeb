using Microsoft.EntityFrameworkCore;
using Semana3_CRUD_Clientes.Models;
namespace Semana3_CRUD_Clientes.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
    }
}

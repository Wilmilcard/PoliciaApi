using Microsoft.EntityFrameworkCore;
using PoliciaApi.Models;

namespace PoliciaApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Establece "SYSTEM" como esquema por defecto
            modelBuilder.HasDefaultSchema("SYSTEM");

            // Si necesitas más configuraciones
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("CLIENTE");
                entity.HasKey(e => e.ClienteId);
            });
        }
    }
}

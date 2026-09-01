using Microsoft.EntityFrameworkCore;
using MiniCommercial.Models.Entities;

namespace MiniCommercial.Data
{
    using Microsoft.EntityFrameworkCore;
    using MiniCommercial.Models;
    using MiniCommercial.Models.Entities;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // On définit la précision pour tous les champs décimaux (18 chiffres au total, 2 après la virgule)
            modelBuilder.Entity<Product>().Property(p => p.UnitPriceHT).HasPrecision(18, 2);
            modelBuilder.Entity<Order>().Property(o => o.TotalHT).HasPrecision(18, 2);
            modelBuilder.Entity<Order>().Property(o => o.TotalTTC).HasPrecision(18, 2);
            modelBuilder.Entity<OrderLine>().Property(ol => ol.UnitPrice).HasPrecision(18, 2);
        }
    }
}

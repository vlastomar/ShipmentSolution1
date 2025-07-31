using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShipmentSolution.Data.Models;
using ShipmentSolution.Data.Configurations;

namespace ShipmentSolution.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<ShipmentEntity> Shipments { get; set; } = null!;
        public DbSet<MailCarrier> MailCarriers { get; set; } = null!;
        public DbSet<Route> Routes { get; set; } = null!;
        public DbSet<Delivery> Deliveries { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Identity-related configuration

            // ✅ Global query filters for IsDeleted
            modelBuilder.Entity<ShipmentEntity>()
                .HasQueryFilter(s => !s.IsDeleted);

            modelBuilder.Entity<Customer>()
                .HasQueryFilter(c => !c.IsDeleted);

            modelBuilder.Entity<MailCarrier>()
                .HasQueryFilter(mc => !mc.IsDeleted);

            modelBuilder.Entity<Route>()
                .HasQueryFilter(r => !r.IsDeleted);

            // ✅ Apply a query filter to Deliveries based on their related Shipment's IsDeleted
            modelBuilder.Entity<Delivery>()
                .HasQueryFilter(d => !d.Shipment.IsDeleted);

            // Apply Fluent API configurations
            modelBuilder.ApplyConfiguration(new MailCarrierConfiguration());
            modelBuilder.ApplyConfiguration(new RouteConfiguration());
            modelBuilder.ApplyConfiguration(new DeliveryConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new ShipmentConfiguration());
            
            
            
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipmentSolution.Data.Models;

namespace ShipmentSolution.Data.Configurations
{
    public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.HasKey(d => d.DeliveryId);

            builder.HasOne(d => d.Shipment)
                   .WithMany(s => s.Deliveries)
                   .HasForeignKey(d => d.ShipmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.MailCarrier)
                   .WithMany(m => m.Deliveries)
                   .HasForeignKey(d => d.MailCarrierId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Route)
                   .WithMany(r => r.Deliveries)
                   .HasForeignKey(d => d.RouteId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Delivery { DeliveryId = 1, ShipmentId = 1, MailCarrierId = 1, RouteId = 1, DateDelivered = DateTime.Parse("2023-06-01") },
                new Delivery { DeliveryId = 2, ShipmentId = 2, MailCarrierId = 2, RouteId = 2, DateDelivered = DateTime.Parse("2023-06-02") },
                new Delivery { DeliveryId = 3, ShipmentId = 3, MailCarrierId = 3, RouteId = 3, DateDelivered = DateTime.Parse("2023-06-03") },
                new Delivery { DeliveryId = 4, ShipmentId = 4, MailCarrierId = 3, RouteId = 4, DateDelivered = DateTime.Parse("2023-06-11") },
                new Delivery { DeliveryId = 5, ShipmentId = 5, MailCarrierId = 5, RouteId = 5, DateDelivered = DateTime.Parse("2023-06-05") },
                new Delivery { DeliveryId = 6, ShipmentId = 6, MailCarrierId = 1, RouteId = 6, DateDelivered = DateTime.Parse("2023-06-07") },
                new Delivery { DeliveryId = 7, ShipmentId = 7, MailCarrierId = 2, RouteId = 7, DateDelivered = DateTime.Parse("2023-06-11") },
                new Delivery { DeliveryId = 8, ShipmentId = 8, MailCarrierId = 4, RouteId = 8, DateDelivered = DateTime.Parse("2023-06-03") },
                new Delivery { DeliveryId = 9, ShipmentId = 9, MailCarrierId = 4, RouteId = 9, DateDelivered = DateTime.Parse("2023-06-09") },
                new Delivery { DeliveryId = 10, ShipmentId = 10, MailCarrierId = 5, RouteId = 10, DateDelivered = DateTime.Parse("2023-06-05") }
            );
        }
    }
}
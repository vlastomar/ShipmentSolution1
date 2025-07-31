using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipmentSolution.Data.Models;
using ShipmentSolution.GCommon;

namespace ShipmentSolution.Data.Configurations
{
    public class RouteConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.HasKey(r => r.RouteId);

            builder.Property(r => r.StartLocation)
                   .HasMaxLength(ValidationConstants.Route.LocationMaxLength)
                   .IsRequired();

            builder.Property(r => r.EndLocation)
                   .HasMaxLength(ValidationConstants.Route.LocationMaxLength)
                   .IsRequired();

            builder.HasMany(r => r.Deliveries)
                   .WithOne(d => d.Route)
                   .HasForeignKey(d => d.RouteId);

            builder.HasData(
                new Route { RouteId = 1, MailCarrierId = 1, StartLocation = "New York City", EndLocation = "Albany", Stops = 0, Distance = 150.5f, Priority = 1 },
                new Route { RouteId = 2, MailCarrierId = 2, StartLocation = "Los Angeles", EndLocation = "San Francisco", Stops = 0, Distance = 400.2f, Priority = 1 },
                new Route { RouteId = 3, MailCarrierId = 3, StartLocation = "Chicago", EndLocation = "Detroit", Stops = 0, Distance = 250.8f, Priority = 1 },
                new Route { RouteId = 4, MailCarrierId = 3, StartLocation = "Detroit", EndLocation = "Los Angeles", Stops = 1, Distance = 50f, Priority = 2 },
                new Route { RouteId = 5, MailCarrierId = 5, StartLocation = "Miami", EndLocation = "Orlando", Stops = 0, Distance = 150.7f, Priority = 1 },
                new Route { RouteId = 6, MailCarrierId = 1, StartLocation = "Albany", EndLocation = "Albany", Stops = 0, Distance = 0f, Priority = 2 },
                new Route { RouteId = 7, MailCarrierId = 2, StartLocation = "San Francisco", EndLocation = "Seattle", Stops = 1, Distance = 400.2f, Priority = 2 },
                new Route { RouteId = 8, MailCarrierId = 4, StartLocation = "Denver", EndLocation = "boston", Stops = 0, Distance = 250.8f, Priority = 1 },
                new Route { RouteId = 9, MailCarrierId = 4, StartLocation = "boston", EndLocation = "Dallas", Stops = 1, Distance = 50f, Priority = 2 },
                new Route { RouteId = 10, MailCarrierId = 5, StartLocation = "Orlando", EndLocation = "San Francisco", Stops = 1, Distance = 80f, Priority = 2 }
            );
        }
    }
}
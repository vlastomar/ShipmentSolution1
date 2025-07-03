using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipmentSolution.Data.Models;
using ShipmentSolution.GCommon;
//using static Azure.Core.HttpHeader;

namespace ShipmentSolution.Data.Configurations
{
    public class MailCarrierConfiguration : IEntityTypeConfiguration<MailCarrier>
    {
        public void Configure(EntityTypeBuilder<MailCarrier> builder)
        {
            builder.HasKey(m => m.MailCarrierId);

            builder.Property(m => m.FirstName).HasMaxLength(ValidationConstants.MailCarrier.NameMaxLength);
            builder.Property(m => m.LastName).HasMaxLength(ValidationConstants.MailCarrier.NameMaxLength);
            builder.Property(m => m.Email).HasMaxLength(ValidationConstants.MailCarrier.EmailMaxLength);
            builder.Property(m => m.PhoneNumber).HasMaxLength(ValidationConstants.MailCarrier.PhoneMaxLength);
            builder.Property(m => m.CurrentLocation).HasMaxLength(ValidationConstants.MailCarrier.LocationMaxLength);
            builder.Property(m => m.Status).HasMaxLength(ValidationConstants.MailCarrier.StatusMaxLength);

            // ✅ Only configure relationShipment FROM MailCarrier (not Route -> MailCarrier)
            builder.HasMany(m => m.Deliveries)
                   .WithOne(d => d.MailCarrier)
                   .HasForeignKey(d => d.MailCarrierId);

            builder.HasMany(m => m.Routes)
                   .WithOne(r => r.MailCarrier)
                   .HasForeignKey(r => r.MailCarrierId);

            builder.HasData(
                new MailCarrier { MailCarrierId = 1, FirstName = "David", LastName = "Wilson", Email = "david.wilson@example.com", PhoneNumber = "1112223333", StartDate = DateTime.Parse("2023-01-01"), EndDate = null, RouteId = 1, CurrentLocation = "New York City", Status = "Available" },
                new MailCarrier { MailCarrierId = 2, FirstName = "Sarah", LastName = "Anderson", Email = "sarah.anderson@example.com", PhoneNumber = "4445556666", StartDate = DateTime.Parse("2023-02-01"), EndDate = null, RouteId = 2, CurrentLocation = "Los Angeles", Status = "On Break" },
                new MailCarrier { MailCarrierId = 3, FirstName = "Robert", LastName = "Miller", Email = "robert.miller@example.com", PhoneNumber = "7778889999", StartDate = DateTime.Parse("2023-03-01"), EndDate = null, RouteId = 3, CurrentLocation = "Chicago", Status = "On a Delivery" },
                new MailCarrier { MailCarrierId = 4, FirstName = "Jennifer", LastName = "Thomas", Email = "jennifer.thomas@example.com", PhoneNumber = "1231231234", StartDate = DateTime.Parse("2023-04-01"), EndDate = null, RouteId = 4, CurrentLocation = "Houston", Status = "Available" },
                new MailCarrier { MailCarrierId = 5, FirstName = "Daniel", LastName = "Wilson", Email = "daniel.wilson@example.com", PhoneNumber = "9998887777", StartDate = DateTime.Parse("2023-05-01"), EndDate = null, RouteId = 5, CurrentLocation = "Miami", Status = "On Break" }
            );
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipmentSolution.Data.Models;
using ShipmentSolution.GCommon;

namespace ShipmentSolution.Data.Configurations
{
    public class MailCarrierConfiguration : IEntityTypeConfiguration<MailCarrier>
    {
        public void Configure(EntityTypeBuilder<MailCarrier> builder)
        {
            builder.HasKey(m => m.MailCarrierId);

            builder.Property(m => m.FirstName)
                   .HasMaxLength(ValidationConstants.MailCarrier.NameMaxLength)
                   .IsRequired();

            builder.Property(m => m.LastName)
                   .HasMaxLength(ValidationConstants.MailCarrier.NameMaxLength)
                   .IsRequired();

            builder.Property(m => m.Email)
                   .HasMaxLength(ValidationConstants.MailCarrier.EmailMaxLength)
                   .IsRequired();

            builder.Property(m => m.PhoneNumber)
                   .HasMaxLength(ValidationConstants.MailCarrier.PhoneMaxLength)
                   .IsRequired();

            builder.Property(m => m.CurrentLocation)
                   .HasMaxLength(ValidationConstants.MailCarrier.LocationMaxLength);

            builder.Property(m => m.Status)
                   .HasMaxLength(ValidationConstants.MailCarrier.StatusMaxLength);

            builder.HasMany(m => m.Deliveries)
                   .WithOne(d => d.MailCarrier)
                   .HasForeignKey(d => d.MailCarrierId);

            builder.HasMany(m => m.Routes)
                   .WithOne(r => r.MailCarrier)
                   .HasForeignKey(r => r.MailCarrierId);

            builder.HasOne(m => m.CreatedByUser)
                   .WithMany()
                   .HasForeignKey(m => m.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new MailCarrier { MailCarrierId = 1, FirstName = "David", LastName = "Wilson", Email = "david.wilson@example.com", PhoneNumber = "1112223333", StartDate = DateTime.Parse("2023-01-01"), RouteId = 1, CurrentLocation = "New York City", Status = "Available", CreatedByUserId = null },
                new MailCarrier { MailCarrierId = 2, FirstName = "Sarah", LastName = "Anderson", Email = "sarah.anderson@example.com", PhoneNumber = "4445556666", StartDate = DateTime.Parse("2023-02-01"), RouteId = 2, CurrentLocation = "Los Angeles", Status = "On Break", CreatedByUserId = null },
                new MailCarrier { MailCarrierId = 3, FirstName = "Robert", LastName = "Miller", Email = "robert.miller@example.com", PhoneNumber = "7778889999", StartDate = DateTime.Parse("2023-03-01"), RouteId = 3, CurrentLocation = "Chicago", Status = "On a Delivery", CreatedByUserId = null },
                new MailCarrier { MailCarrierId = 4, FirstName = "Jennifer", LastName = "Thomas", Email = "jennifer.thomas@example.com", PhoneNumber = "1231231234", StartDate = DateTime.Parse("2023-04-01"), RouteId = 4, CurrentLocation = "Houston", Status = "Available", CreatedByUserId = null },
                new MailCarrier { MailCarrierId = 5, FirstName = "Daniel", LastName = "Wilson", Email = "daniel.wilson@example.com", PhoneNumber = "9998887777", StartDate = DateTime.Parse("2023-05-01"), RouteId = 5, CurrentLocation = "Miami", Status = "On Break", CreatedByUserId = null }
            );
        }
    }
}
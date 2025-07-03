using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipmentSolution.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentSolution.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.CustomerId);

            builder.Property(c => c.FirstName).HasMaxLength(GCommon.ValidationConstants.Customer.NameMaxLength);
            builder.Property(c => c.LastName).HasMaxLength(GCommon.ValidationConstants.Customer.NameMaxLength);
            builder.Property(c => c.Email).HasMaxLength(GCommon.ValidationConstants.Customer.EmailMaxLength);
            builder.Property(c => c.PhoneNumber).HasMaxLength(GCommon.ValidationConstants.Customer.PhoneMaxLength);
            builder.Property(c => c.City).HasMaxLength(GCommon.ValidationConstants.Customer.LocationMaxLength);
            builder.Property(c => c.State).HasMaxLength(GCommon.ValidationConstants.Customer.LocationMaxLength);
            builder.Property(c => c.ZipCode).HasMaxLength(GCommon.ValidationConstants.Customer.LocationMaxLength);
            builder.Property(c => c.PreferredShippingMethod).HasMaxLength(GCommon.ValidationConstants.Shipment.ShippingMethodMaxLength);

            builder.HasMany(c => c.Shipments)
                   .WithOne(s => s.Customer)
                   .HasForeignKey(s => s.CustomerId);
            builder.HasData(
    new Customer { CustomerId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890", City = "New York City", State = "NY", ZipCode = "10001", PreferredShippingMethod = "Express", ShippingCostThreshold = 50.0f },
    new Customer { CustomerId = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", PhoneNumber = "9876543210", City = "Los Angeles", State = "CA", ZipCode = "90001", PreferredShippingMethod = "Ground", ShippingCostThreshold = 30.0f },
    new Customer { CustomerId = 3, FirstName = "Michael", LastName = "Johnson", Email = "michael.johnson@example.com", PhoneNumber = "5555555555", City = "Chicago", State = "IL", ZipCode = "60601", PreferredShippingMethod = "Express", ShippingCostThreshold = 75.0f },
    new Customer { CustomerId = 4, FirstName = "Emily", LastName = "Brown", Email = "emily.brown@example.com", PhoneNumber = "1111111111", City = "Houston", State = "TX", ZipCode = "77001", PreferredShippingMethod = "Ground", ShippingCostThreshold = 40.0f },
    new Customer { CustomerId = 5, FirstName = "William", LastName = "Taylor", Email = "william.taylor@example.com", PhoneNumber = "9999999999", City = "Miami", State = "FL", ZipCode = "33101", PreferredShippingMethod = "Express", ShippingCostThreshold = 60.0f }
);
        }

    }
}

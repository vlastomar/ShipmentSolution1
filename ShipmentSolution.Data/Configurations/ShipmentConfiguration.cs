using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipmentSolution.Data.Models;


namespace ShipmentSolution.Data.Configurations
{
    public class ShipmentConfiguration : IEntityTypeConfiguration<ShipmentSolution.Data.Models.ShipmentEntity>
    {
        public void Configure(EntityTypeBuilder<ShipmentSolution.Data.Models.ShipmentEntity> builder)
        {
            builder.HasKey(s => s.ShipmentId);

            builder.Property(s => s.Dimensions).HasMaxLength(GCommon.ValidationConstants.Shipment.DimensionsMaxLength);
            builder.Property(s => s.ShippingMethod).HasMaxLength(GCommon.ValidationConstants.Shipment.ShippingMethodMaxLength);

            builder.HasMany(s => s.Deliveries)
                   .WithOne(d => d.Shipment)
                   .HasForeignKey(d => d.ShipmentId);
            builder.HasData(
    new ShipmentSolution.Data.Models.ShipmentEntity { ShipmentId = 1, CustomerId = 1, Weight = 10.5f, Dimensions = "12x10x8", ShippingMethod = "Ground", ShippingCost = 15.0f, DeliveryDate = DateTime.Parse("2023-06-01") },
    new ShipmentSolution.Data.Models.ShipmentEntity { ShipmentId = 2, CustomerId = 2, Weight = 7.2f, Dimensions = "10x8x6", ShippingMethod = "Express", ShippingCost = 25.0f, DeliveryDate = DateTime.Parse("2023-06-02") },
    new ShipmentSolution.Data.Models.ShipmentEntity { ShipmentId = 3, CustomerId = 3, Weight = 15.3f, Dimensions = "16x12x10", ShippingMethod = "Ground", ShippingCost = 18.5f, DeliveryDate = DateTime.Parse("2023-06-03") },
    new ShipmentSolution.Data.Models.ShipmentEntity { ShipmentId = 4, CustomerId = 4, Weight = 5.9f, Dimensions = "8x6x4", ShippingMethod = "Express", ShippingCost = 30.0f, DeliveryDate = DateTime.Parse("2023-06-04") },
    new ShipmentSolution.Data.Models.ShipmentEntity { ShipmentId = 5, CustomerId = 5, Weight = 12.8f, Dimensions = "14x10x8", ShippingMethod = "Ground", ShippingCost = 20.0f, DeliveryDate = DateTime.Parse("2023-06-05") },
    new ShipmentSolution.Data.Models.ShipmentEntity { ShipmentId = 6, CustomerId = 1, Weight = 9.7f, Dimensions = "10x8x6", ShippingMethod = "Ground", ShippingCost = 14.5f, DeliveryDate = DateTime.Parse("2023-06-06") },
    new ShipmentSolution.Data.Models.ShipmentEntity { ShipmentId = 7, CustomerId = 4, Weight = 6.5f, Dimensions = "8x6x4", ShippingMethod = "Express", ShippingCost = 28.0f, DeliveryDate = DateTime.Parse("2023-06-07") },
    new ShipmentSolution.Data.Models.ShipmentEntity { ShipmentId = 8, CustomerId = 5, Weight = 11.2f, Dimensions = "12x10x8", ShippingMethod = "Ground", ShippingCost = 17.0f, DeliveryDate = DateTime.Parse("2023-06-08") },
    new ShipmentSolution.Data.Models.ShipmentEntity { ShipmentId = 9, CustomerId = 2, Weight = 8.9f, Dimensions = "14x10x8", ShippingMethod = "Express", ShippingCost = 32.5f, DeliveryDate = DateTime.Parse("2023-06-09") },
    new ShipmentSolution.Data.Models.ShipmentEntity { ShipmentId = 10, CustomerId = 3, Weight = 13.7f, Dimensions = "16x12x10", ShippingMethod = "Ground", ShippingCost = 19.5f, DeliveryDate = DateTime.Parse("2023-06-10") }
);
        }

        
    }
}

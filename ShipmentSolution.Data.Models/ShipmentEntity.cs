using ShipmentSolution.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace ShipmentSolution.Data.Models
{
    public class ShipmentEntity
    {
        public int ShipmentId { get; set; }
        public int CustomerId { get; set; }
        public float Weight { get; set; }
        public string Dimensions { get; set; } = null!;
        public string ShippingMethod { get; set; } = null!;
        public float ShippingCost { get; set; }
        public DateTime DeliveryDate { get; set; }


        public bool IsDeleted { get; set; }
        public string? CreatedByUserId { get; set; }
        public IdentityUser? CreatedByUser { get; set; }
        public Customer Customer { get; set; } = null!;
        public ICollection<Delivery> Deliveries { get; set; } = null!;
    }
}

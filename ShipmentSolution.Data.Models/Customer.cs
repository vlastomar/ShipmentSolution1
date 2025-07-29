using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentSolution.Data.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public string PreferredShippingMethod { get; set; } = null!;
        public float ShippingCostThreshold { get; set; }
        public bool IsDeleted { get; set; }
        public string? CreatedByUserId { get; set; }
        public IdentityUser? CreatedByUser { get; set; }

        public ICollection<ShipmentEntity> Shipments { get; set; } = null!;
    }
}

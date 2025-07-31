using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ShipmentSolution.Data.Models
{
    public class Route
    {
        public int RouteId { get; set; }

        public int MailCarrierId { get; set; }

        public string StartLocation { get; set; } = null!;

        public string EndLocation { get; set; } = null!;

        public int Stops { get; set; }

        public float Distance { get; set; }

        public int Priority { get; set; }

        public bool IsDeleted { get; set; }

        
        public string? CreatedByUserId { get; set; }

        public IdentityUser? CreatedByUser { get; set; }

        
        public MailCarrier MailCarrier { get; set; } = null!;

        public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
    }
}

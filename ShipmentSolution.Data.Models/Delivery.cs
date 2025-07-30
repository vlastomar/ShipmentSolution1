using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentSolution.Data.Models
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        public int ShipmentId { get; set; }
        public int MailCarrierId { get; set; }
        public int RouteId { get; set; }
        public DateTime DateDelivered { get; set; }

        public ShipmentEntity Shipment { get; set; } = null!;
        public MailCarrier MailCarrier { get; set; } = null!;
        public Route Route { get; set; } = null!;
        public string? CreatedByUserId { get; set; }
        public IdentityUser? CreatedByUser { get; set; }
        public bool IsDeleted { get; set; }
    }
}

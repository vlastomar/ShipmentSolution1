using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public MailCarrier MailCarrier { get; set; } = null!;
        public ICollection<Delivery> Deliveries { get; set; } = null!;
    }
}

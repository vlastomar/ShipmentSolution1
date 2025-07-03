using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentSolution.Web.ViewModels.ShipmentViewModels
{
    public class ShipmentDeleteViewModel
    {
        public int ShipmentId { get; set; }
        public string? CustomerName { get; set; }
        public string? ShippingMethod { get; set; }
        public decimal ShippingCost { get; set; }
        public DateTime? DeliveryDate { get; set; }
    }
}

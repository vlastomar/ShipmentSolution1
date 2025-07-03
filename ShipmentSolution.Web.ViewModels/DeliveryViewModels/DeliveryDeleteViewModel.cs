using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentSolution.Web.ViewModels.DeliveryViewModels
{
    public class DeliveryDeleteViewModel
    {
        public int DeliveryId { get; set; }
        public string ShipmentInfo { get; set; } = null!;
        public string MailCarrierName { get; set; } = null!;
        public string Route { get; set; } = null!;
        public DateTime DateDelivered { get; set; }
    }

}

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentSolution.Web.ViewModels.DeliveryViewModels
{
    public class DeliveryCreateViewModel
    {
        public int ShipmentId { get; set; }
        public int MailCarrierId { get; set; }
        public int RouteId { get; set; }
        public DateTime DateDelivered { get; set; }

        public IEnumerable<SelectListItem> Shipments { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> MailCarriers { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Routes { get; set; } = new List<SelectListItem>();
    }

}

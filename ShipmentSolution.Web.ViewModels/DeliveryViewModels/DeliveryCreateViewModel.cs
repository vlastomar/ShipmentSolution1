using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ShipmentSolution.Web.ViewModels.DeliveryViewModels
{
    public class DeliveryCreateViewModel
    {
        [Required(ErrorMessage = "Shipment is required.")]
        [Display(Name = "Shipment")]
        public int? ShipmentId { get; set; }

        [Required(ErrorMessage = "Mail Carrier is required.")]
        [Display(Name = "Mail Carrier")]
        public int? MailCarrierId { get; set; }

        [Required(ErrorMessage = "Route is required.")]
        [Display(Name = "Route")]
        public int? RouteId { get; set; }

        [Required(ErrorMessage = "Delivery Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Delivered")]
        public DateTime? DateDelivered { get; set; }

        public IEnumerable<SelectListItem> Shipments { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> MailCarriers { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Routes { get; set; } = new List<SelectListItem>();

    }
}

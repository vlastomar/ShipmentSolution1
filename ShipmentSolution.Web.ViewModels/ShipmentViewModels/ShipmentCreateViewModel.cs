using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShipmentSolution.Web.ViewModels.ShipmentViewModels
{
    public class ShipmentCreateViewModel
    {
        [Required]
        public float Weight { get; set; }

        [Required]
        public string Dimensions { get; set; } = null!;

        [Required]
        public string ShippingMethod { get; set; } = null!;

        [Required]
        public decimal ShippingCost { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int CarrierId { get; set; }

        [Required]
        public int RouteId { get; set; }

        // Dropdown lists
        public IEnumerable<SelectListItem> Customers { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Carriers { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Routes { get; set; } = new List<SelectListItem>();
    }
}

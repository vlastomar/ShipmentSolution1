using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentSolution.Web.ViewModels.ShipmentViewModels
{
    public class ShipmentEditViewModel
    {
        public int ShipmentId { get; set; }

        [Required]
        public float Weight { get; set; }

        [Required]
        public string Dimensions { get; set; } = null!;

        [Required]
        public string ShippingMethod { get; set; } = null!;

        [Required]
        public float ShippingCost { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DeliveryDate { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; } = new List<SelectListItem>();
    }
}

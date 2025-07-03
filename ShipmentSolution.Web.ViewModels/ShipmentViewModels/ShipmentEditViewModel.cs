using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShipmentSolution.Web.ViewModels.ShipmentViewModels
{
    public class ShipmentEditViewModel
    {
        [Required]
        public int ShipmentId { get; set; }

        [Required(ErrorMessage = "Weight is required.")]
        [Range(0.01, 10000, ErrorMessage = "Weight must be between 0.01 and 10,000 kg.")]
        [Display(Name = "Weight (kg)")]
        public float Weight { get; set; }

        [Required(ErrorMessage = "Dimensions are required.")]
        [StringLength(100, ErrorMessage = "Dimensions must be under 100 characters.")]
        public string Dimensions { get; set; } = null!;

        [Required(ErrorMessage = "Shipping method is required.")]
        [StringLength(50, ErrorMessage = "Shipping method must be under 50 characters.")]
        [Display(Name = "Shipping Method")]
        public string ShippingMethod { get; set; } = null!;

        [Required(ErrorMessage = "Shipping cost is required.")]
        [Range(0.01, 100000, ErrorMessage = "Shipping cost must be greater than zero.")]
        [Display(Name = "Shipping Cost ($)")]
        public float ShippingCost { get; set; }

        [Required(ErrorMessage = "Delivery date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Delivery Date")]
        public DateTime DeliveryDate { get; set; }

        [Required(ErrorMessage = "Customer selection is required.")]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; } = new List<SelectListItem>();
    }
}

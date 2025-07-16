using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShipmentSolution.Web.ViewModels.ShipmentViewModels
{
    public class ShipmentCreateViewModel
    {
        [Required(ErrorMessage = "Weight is required.")]
        [Range(0.01, 10000, ErrorMessage = "Weight must be between 0.01 and 10,000 kg.")]
        [Display(Name = "Weight (kg)")]
        public float Weight { get; set; }

        [Required(ErrorMessage = "Dimensions are required.")]
        [StringLength(100, ErrorMessage = "Dimensions must be under 100 characters.")]
        public string Dimensions { get; set; } = null!;

        [Required(ErrorMessage = "Shipping method is required.")]
        [Display(Name = "Shipping Method")]
        public string ShippingMethod { get; set; } = null!;

        [Required(ErrorMessage = "Shipping cost is required.")]
        [Range(0.01, 100000, ErrorMessage = "Shipping cost must be greater than zero.")]
        [Display(Name = "Shipping Cost ($)")]
        public decimal ShippingCost { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Expected Delivery Date")]
        public DateTime? DeliveryDate { get; set; }

        [Required(ErrorMessage = "Customer selection is required.")]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Carrier selection is required.")]
        [Display(Name = "Mail Carrier")]
        public int CarrierId { get; set; }

        [Required(ErrorMessage = "Route selection is required.")]
        [Display(Name = "Route")]
        public int RouteId { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Carriers { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Routes { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ShippingMethods { get; set; } = new List<SelectListItem>();
    }
}

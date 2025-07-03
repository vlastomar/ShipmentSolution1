using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShipmentSolution.Web.ViewModels.RouteViewModels
{
    public class RouteCreateViewModel
    {
        [Required(ErrorMessage = "Start location is required.")]
        [StringLength(100, ErrorMessage = "Start location must be under 100 characters.")]
        [Display(Name = "Start Location")]
        public string StartLocation { get; set; } = null!;

        [Required(ErrorMessage = "End location is required.")]
        [StringLength(100, ErrorMessage = "End location must be under 100 characters.")]
        [Display(Name = "End Location")]
        public string EndLocation { get; set; } = null!;

        [Display(Name = "Stops")]
        [StringLength(200, ErrorMessage = "Stops must be under 200 characters.")]
        public string? Stops { get; set; }

        [Required(ErrorMessage = "Distance is required.")]
        [Range(0.1, 10000, ErrorMessage = "Distance must be between 0.1 and 10,000 km.")]
        [Display(Name = "Distance (km)")]
        public double Distance { get; set; }

        [Required(ErrorMessage = "Priority is required.")]
        [Range(1, 3, ErrorMessage = "Priority must be between 1 (High), 2 (Medium), or 3 (Low).")]
        public int Priority { get; set; }

        [Required(ErrorMessage = "Mail carrier selection is required.")]
        [Display(Name = "Assigned Mail Carrier")]
        public int MailCarrierId { get; set; }

        public IEnumerable<SelectListItem> MailCarriers { get; set; } = new List<SelectListItem>();
    }
}

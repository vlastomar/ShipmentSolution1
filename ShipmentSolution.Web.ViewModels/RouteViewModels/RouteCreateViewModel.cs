using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentSolution.Web.ViewModels.RouteViewModels
{
    public class RouteCreateViewModel
    {
        [Required]
        public string StartLocation { get; set; } = null!;

        [Required]
        public string EndLocation { get; set; } = null!;

        public string? Stops { get; set; }

        public double Distance { get; set; }

        public int Priority { get; set; }

        public int MailCarrierId { get; set; } 
        public IEnumerable<SelectListItem> MailCarriers { get; set; } = new List<SelectListItem>();

    }

}

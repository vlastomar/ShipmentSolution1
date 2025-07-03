using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentSolution.Web.ViewModels.RouteViewModels
{
    public class RouteDeleteViewModel
    {
        public int RouteId { get; set; }

        public string StartLocation { get; set; } = null!;

        public string EndLocation { get; set; } = null!;

        public int Stops { get; set; }

        public float Distance { get; set; }

        public string Priority { get; set; } = null!;
    }
}

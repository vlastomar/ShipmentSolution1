using System;

namespace ShipmentSolution.Web.ViewModels.RouteViewModels
{
    public class RouteDeleteViewModel
    {
        public int RouteId { get; set; }
        public string StartLocation { get; set; } = null!;
        public string EndLocation { get; set; } = null!;
        public int Stops { get; set; }
        public float Distance { get; set; }

        public int Priority { get; set; } // 👈 Store as int

        public string PriorityDisplay => Priority switch
        {
            1 => "Low",
            2 => "Medium",
            3 => "High",
            _ => "Unknown"
        };
    }


}

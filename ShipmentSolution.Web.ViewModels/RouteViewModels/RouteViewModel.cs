using System;

namespace ShipmentSolution.Web.ViewModels.RouteViewModels
{
    public class RouteViewModel
    {
        public int RouteId { get; set; }

        public string StartLocation { get; set; } = null!;

        public string EndLocation { get; set; } = null!;

        public int Stops { get; set; }

        public double Distance { get; set; }

        public int Priority { get; set; }

        public string? CreatedByUserId { get; set; }

        // 🪧 Display-friendly priority
        public string PriorityDisplay => Priority switch
        {
            1 => "Low",
            2 => "Medium",
            3 => "High",
            _ => "Unknown"
        };

        // ✅ Ownership-based access (set in the service)
        public bool IsOwnedByCurrentUser { get; set; }
    }
}

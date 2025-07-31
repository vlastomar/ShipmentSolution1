using System;

namespace ShipmentSolution.Web.ViewModels.MailCarrierViewModels
{
    public class MailCarrierViewModel
    {
        public int MailCarrierId { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string? CurrentLocation { get; set; }

       
        public string CreatedByUserId { get; set; } = null!;
    }
}

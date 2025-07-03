using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentSolution.Web.ViewModels.MailCarrierViewModels
{
    public class MailCarrierCreateViewModel
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? CurrentLocation { get; set; }
    }

}

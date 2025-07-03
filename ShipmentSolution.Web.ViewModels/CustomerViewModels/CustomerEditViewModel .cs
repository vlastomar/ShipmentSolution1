using System.ComponentModel.DataAnnotations;

namespace ShipmentSolution.Web.ViewModels.CustomerViewModels
{
    public class CustomerEditViewModel : CustomerCreateViewModel
    {
        [Required]
        public int CustomerId { get; set; }
    }
}

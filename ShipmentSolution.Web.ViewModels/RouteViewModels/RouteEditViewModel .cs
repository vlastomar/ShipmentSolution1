using System.ComponentModel.DataAnnotations;

namespace ShipmentSolution.Web.ViewModels.RouteViewModels
{
    public class RouteEditViewModel : RouteCreateViewModel
    {
        [Required]
        public int RouteId { get; set; }
    }
}

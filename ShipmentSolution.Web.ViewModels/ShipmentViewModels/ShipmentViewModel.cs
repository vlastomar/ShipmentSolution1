
namespace ShipmentSolution.Web.ViewModels.ShipmentViewModels
{
    public class ShipmentViewModel
    {
        public int ShipmentId { get; set; }
        public string? CustomerName { get; set; }
        public string? ShippingMethod { get; set; }
        public decimal ShippingCost { get; set; }
        public DateTime? DeliveryDate { get; set; }

        public string? CreatedByUserId { get; set; }
    }
}
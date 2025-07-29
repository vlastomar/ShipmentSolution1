namespace ShipmentSolution.Web.ViewModels.CustomerViewModels
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;
        public string? CreatedByUserId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ShipmentSolution.Web.ViewModels.CustomerViewModels
{
    public class CustomerCreateViewModel
    {
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name must be under 50 characters.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name must be under 50 characters.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Enter a valid phone number.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "City is required.")]
        [StringLength(100)]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "State is required.")]
        [StringLength(100)]
        public string State { get; set; } = null!;

        [Required(ErrorMessage = "Zip Code is required.")]
        //[RegularExpression(@"^\d{4,6}$", ErrorMessage = "ZipCode must be 4 to 6 digits.")]
        public string ZipCode { get; set; } = null!;

        [Required(ErrorMessage = "Preferred Shipping Method is required.")]
        [StringLength(50)]
        public string PreferredShippingMethod { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "Shipping Cost Threshold must be a non-negative number.")]
        public float ShippingCostThreshold { get; set; }
    }
}

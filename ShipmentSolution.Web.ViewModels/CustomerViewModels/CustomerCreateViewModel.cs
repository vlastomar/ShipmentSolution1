using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ShipmentSolution.Web.ViewModels.CustomerViewModels
{
    public class CustomerCreateViewModel
    {
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name must be under 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name must be under 50 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Enter a valid phone number.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "City is required.")]
        [StringLength(100)]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "State is required.")]
        [StringLength(100)]
        public string State { get; set; } = null!;

        [Required(ErrorMessage = "Zip Code is required.")]
        //[RegularExpression(@"^\d{4,6}$", ErrorMessage = "ZipCode must be 4 to 6 digits.")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; } = null!;

        [Required(ErrorMessage = "Preferred Shipping Method is required.")]
        [StringLength(50)]
        [Display(Name = "Preferred Shipping Method")]
        public string PreferredShippingMethod { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "Shipping Cost Threshold must be a non-negative number.")]
        [Display(Name = "Shipping Cost Threshold")]
        public float ShippingCostThreshold { get; set; }

        public IEnumerable<SelectListItem> ShippingMethodOptions { get; set; } = new List<SelectListItem>();
    }
}

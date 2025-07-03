using System.ComponentModel.DataAnnotations;

namespace ShipmentSolution.Web.ViewModels.MailCarrierViewModels
{
    public class MailCarrierEditViewModel
    {
        [Required]
        public int MailCarrierId { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name must be between 2 and 100 characters.", MinimumLength = 2)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Enter a valid phone number.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Status is required.")]
        [Display(Name = "Status")]
        public string Status { get; set; } = null!;

        [Display(Name = "Current Location")]
        public string? CurrentLocation { get; set; }
    }
}

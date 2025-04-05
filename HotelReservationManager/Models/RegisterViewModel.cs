using System.ComponentModel.DataAnnotations;

namespace HotelReservationManager.Models
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "UserName")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "FirstName")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Middle name is required")]
        [Display(Name = "MiddleName")]
        public required string MiddleName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "LastName")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Display(Name = "PhoneNumber")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public required string ConfirmPassword { get; set; }
    }
}

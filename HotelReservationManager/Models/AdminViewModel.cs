using System.ComponentModel.DataAnnotations;

namespace HotelReservationManager.Models
{
    public class AdminViewModel
    {
        [Display(Name = "Firs Name")]
        public required string FirsName { get; set; }
        [Display(Name = "Last Name")]
        public required string LastName { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress]
        public required string Email { get; set; }

        [Display(Name = "Phone Number")]
        public required string PhoneNumber { get; set; }

        [Display(Name = "Available Rooms")]
        public int AvailableRooms { get; set; }
    }
}

using HotelReservationManager.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace HotelReservationManager.Models
{
    public class UserRole
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        [StringLength(10)]
        public string Role { get; set; } // "Admin" or "User"
    }
}

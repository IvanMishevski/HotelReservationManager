using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservationManager.Models
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public Room Room { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public List<ReservationClient> ReservationClients { get; set; } = new();

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        [CustomDateRange]
        public DateTime CheckOutDate { get; set; }

        public bool IsBreakfastIncluded { get; set; }

        public bool IsAllInclusive { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")] 
        public decimal AmountDue { get; set; }
    }

    public class CustomDateRangeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime checkOutDate = (DateTime)value;
            return checkOutDate > DateTime.Now; // Ensure checkout date is in the future
        }
    }
}

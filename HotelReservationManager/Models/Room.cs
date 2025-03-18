using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelReservationManager.Models
{
    public class Room
    {
         [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required]
            public int Capacity { get; set; }

            [Required]
            [StringLength(100)]
            public string Type { get; set; }

            [Required]
            public bool IsAvailable { get; set; }

            [Required]
            [Column(TypeName = "decimal(18,2)")]
            public decimal PricePerAdult { get; set; }

            [Required]
            [Column(TypeName = "decimal(18,2)")]
            public decimal PricePerChild { get; set; }

            [Required]
            public int RoomNumber { get; set; }
    }
}

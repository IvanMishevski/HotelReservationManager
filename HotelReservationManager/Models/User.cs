using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservationManager.Models
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }


        [StringLength(10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "EGN must be 10 digits.")]
        [Required]
        public string EGN { get; set; }

        [Required]
        public DateTime HireDate { get; set; } = DateTime.Now;

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime? TerminationDate { get; set; }
       
    }
}

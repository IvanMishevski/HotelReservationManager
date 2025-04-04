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

        [StringLength(50)]
        public string FatherName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "EGN must be 10 digits.")]
        public string EGN { get; set; }

        // PhoneNumber is already in IdentityUser, but we can add the validation attribute
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public override string PhoneNumber { get; set; }

        // Email is already in IdentityUser, but we can add the validation attribute
        [EmailAddress]
        public override string Email { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public DateTime? TerminationDate { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservationManager.Models
{
    public class User : IdentityUser
    {
        public string UserRole { get; set; } = "PublicUser";
    }
}

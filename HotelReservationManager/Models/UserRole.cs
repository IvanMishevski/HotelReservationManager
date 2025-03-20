 public class UserRole
    {
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [StringLength(10)]
        public string Role { get; set; } // "Admin" or "User"
    }
}

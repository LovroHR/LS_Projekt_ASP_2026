using System;
using System.ComponentModel.DataAnnotations;

namespace AudioProductionManagement.Model
{
    public abstract class AppUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = "";

        [Required]
        [MaxLength(100)]
        public string Surname { get; set; } = "";

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [MaxLength(30)]
        public string PhoneNumber { get; set; } = "";

        public DateTime CreatedAt { get; set; }

        public UserRole Role { get; set; }
    }
}

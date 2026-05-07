using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AudioProductionManagement.Model
{
    public class Client : AppUser
    {
        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = "";

        public DateTime DateOfBirth { get; set; }

        [MaxLength(255)]
        public string Address { get; set; } = "";

        [MaxLength(100)]
        public string Country { get; set; } = "";

        [MaxLength(150)]
        public string CompanyName { get; set; } = "";

        [MaxLength(255)]
        public string BillingAddress { get; set; } = "";

        public bool IsPriorityClient { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; } = "";

        [InverseProperty(nameof(Booking.Client))]
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        [InverseProperty(nameof(AudioProject.Client))]
        public virtual ICollection<AudioProject> Projects { get; set; } = new List<AudioProject>();
    }
}

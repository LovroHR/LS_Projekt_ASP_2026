using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AudioProductionManagement.Model
{
    public class Producer : AppUser
    {
        [MaxLength(120)]
        public string Specialization { get; set; } = "";

        [Precision(18, 2)]
        public decimal HourlyRate { get; set; }

        public bool IsExternalCollaborator { get; set; }

        [MaxLength(3000)]
        public string Biography { get; set; } = "";

        [InverseProperty(nameof(Booking.Producer))]
        public virtual ICollection<Booking> AssignedBookings { get; set; } = new List<Booking>();

        [InverseProperty(nameof(AudioProject.Producer))]
        public virtual ICollection<AudioProject> ManagedProjects { get; set; } = new List<AudioProject>();
    }
}

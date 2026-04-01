using System;
using System.Collections.Generic;

namespace AudioProductionManagement.Model
{
    public class Producer : AppUser
    {
        public string Specialization { get; set; } = "";
        public decimal HourlyRate { get; set; }
        public bool IsExternalCollaborator { get; set; }
        public string Biography { get; set; } = "";
        public List<Booking> AssignedBookings { get; set; } = new List<Booking>();
        public List<AudioProject> ManagedProjects { get; set; } = new List<AudioProject>();
    }
}

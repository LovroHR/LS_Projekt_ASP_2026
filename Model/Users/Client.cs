using System;
using System.Collections.Generic;

namespace AudioProductionManagement.Model
{
    public class Client : AppUser
    {
        public string CompanyName { get; set; } = "";
        public string BillingAddress { get; set; } = "";
        public bool IsPriorityClient { get; set; }
        public string Notes { get; set; } = "";
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public List<AudioProject> Projects { get; set; } = new List<AudioProject>();
    }
}

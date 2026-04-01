using System;
using System.Collections.Generic;

namespace AudioProductionManagement.Model
{
    public class StudioRoom
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Location { get; set; } = "";
        public int Capacity { get; set; }
        public bool HasVocalBooth { get; set; }
        public bool HasAnalogGear { get; set; }
        public decimal HourlyPrice { get; set; }
        public string EquipmentSummary { get; set; } = "";
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

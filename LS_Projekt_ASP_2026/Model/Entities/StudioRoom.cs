using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AudioProductionManagement.Model
{
    public class StudioRoom
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string Name { get; set; } = "";

        [Required]
        [MaxLength(120)]
        public string Location { get; set; } = "";

        public int Capacity { get; set; }

        public bool HasVocalBooth { get; set; }

        public bool HasAnalogGear { get; set; }

        [Precision(18, 2)]
        public decimal HourlyPrice { get; set; }

        [MaxLength(2000)]
        public string EquipmentSummary { get; set; } = "";

        [InverseProperty(nameof(Booking.StudioRoom))]
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

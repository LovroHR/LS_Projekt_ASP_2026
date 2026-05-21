using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace AudioProductionManagement.Model
{
    public class Booking
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime CreatedAt { get; set; }

        public BookingStatus Status { get; set; }

        [Required]
        [MaxLength(300)]
        public string Purpose { get; set; } = "";

        [Precision(18, 2)]
        public decimal TotalPrice { get; set; }

        public bool RequiresEngineer { get; set; }

        [MaxLength(2000)]
        public string AdditionalNotes { get; set; } = "";

        public int ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        [InverseProperty(nameof(Client.Bookings))]
        [ValidateNever]
        public Client Client { get; set; } = null!;

        public int ProducerId { get; set; }

        [ForeignKey(nameof(ProducerId))]
        [InverseProperty(nameof(Producer.AssignedBookings))]
        [ValidateNever]
        public Producer Producer { get; set; } = null!;

        public int StudioRoomId { get; set; }

        [ForeignKey(nameof(StudioRoomId))]
        [InverseProperty(nameof(StudioRoom.Bookings))]
        [ValidateNever]
        public StudioRoom StudioRoom { get; set; } = null!;
    }
}

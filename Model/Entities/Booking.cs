using System;

namespace AudioProductionManagement.Model
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public BookingStatus Status { get; set; }
        public string Purpose { get; set; } = "";
        public decimal TotalPrice { get; set; }
        public bool RequiresEngineer { get; set; }
        public string AdditionalNotes { get; set; } = "";
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int ProducerId { get; set; }
        public Producer Producer { get; set; }
        public int StudioRoomId { get; set; }
        public StudioRoom StudioRoom { get; set; }
    }
}

using System;

namespace AudioProductionManagement.Model
{
    public abstract class AppUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserRole Role { get; set; }
    }
}

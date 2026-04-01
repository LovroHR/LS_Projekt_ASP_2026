using System;

namespace AudioProductionManagement.Model
{
    public class TimecodedComment
    {
        public int Id { get; set; }
        public decimal TimestampSeconds { get; set; }
        public string Message { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public bool IsResolved { get; set; }
        public string Category { get; set; } = "";
        public bool IsInternalNote { get; set; }
        public int ProjectVersionId { get; set; }
        public ProjectVersion ProjectVersion { get; set; }
        public int AuthorId { get; set; }
        public AppUser Author { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace AudioProductionManagement.Model
{
    public class ProjectVersion
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public AudioProject Project { get; set; }
        public int VersionNumber { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int DurationSeconds { get; set; }
        public decimal FileSize { get; set; }
        public string FileUrl { get; set; } = "";
        public string Notes { get; set; } = "";
        public bool IsApproved { get; set; }
        public List<TimecodedComment> Comments { get; set; } = new List<TimecodedComment>();
    }
}

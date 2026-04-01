using System;
using System.Collections.Generic;

namespace AudioProductionManagement.Model
{
    public class AudioProject
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public ProjectType Type { get; set; }
        public ProjectStatus Status { get; set; }
        public string Genre { get; set; } = "";
        public int TargetDurationSeconds { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? Deadline { get; set; }
        public decimal Budget { get; set; }
        public bool AllowClientComments { get; set; }
        public string SharedFolderUrl { get; set; } = "";
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int ProducerId { get; set; }
        public Producer Producer { get; set; }
        public List<ProjectVersion> Versions { get; set; } = new List<ProjectVersion>();
    }
}

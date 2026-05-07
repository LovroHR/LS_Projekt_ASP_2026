using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AudioProductionManagement.Model
{
    public class ProjectVersion
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        [InverseProperty(nameof(AudioProject.Versions))]
        public AudioProject Project { get; set; } = null!;

        public int VersionNumber { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = "";

        [MaxLength(2000)]
        public string Description { get; set; } = "";

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int DurationSeconds { get; set; }

        [Precision(18, 2)]
        public decimal FileSize { get; set; }

        [MaxLength(500)]
        public string FileUrl { get; set; } = "";

        [MaxLength(2000)]
        public string Notes { get; set; } = "";

        public bool IsApproved { get; set; }

        [InverseProperty(nameof(TimecodedComment.ProjectVersion))]
        public virtual ICollection<TimecodedComment> Comments { get; set; } = new List<TimecodedComment>();
    }
}

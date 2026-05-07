using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AudioProductionManagement.Model
{
    public class TimecodedComment
    {
        public int Id { get; set; }

        [Precision(10, 3)]
        public decimal TimestampSeconds { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Message { get; set; } = "";

        public DateTime CreatedAt { get; set; }

        public bool IsResolved { get; set; }

        [MaxLength(100)]
        public string Category { get; set; } = "";

        public bool IsInternalNote { get; set; }

        public int ProjectVersionId { get; set; }

        [ForeignKey(nameof(ProjectVersionId))]
        public ProjectVersion ProjectVersion { get; set; } = null!;

        public int AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public AppUser Author { get; set; } = null!;
    }
}

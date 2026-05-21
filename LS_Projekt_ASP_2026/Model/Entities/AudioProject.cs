using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace AudioProductionManagement.Model
{
    public class AudioProject
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = "";

        public ProjectType Type { get; set; }

        public ProjectStatus Status { get; set; }

        [MaxLength(80)]
        public string Genre { get; set; } = "";

        public int TargetDurationSeconds { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? Deadline { get; set; }

        [Precision(18, 2)]
        public decimal Budget { get; set; }

        public bool AllowClientComments { get; set; }

        [MaxLength(500)]
        public string SharedFolderUrl { get; set; } = "";

        public int ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        [InverseProperty(nameof(Client.Projects))]
        [ValidateNever]
        public Client Client { get; set; } = null!;

        public int ProducerId { get; set; }

        [ForeignKey(nameof(ProducerId))]
        [InverseProperty(nameof(Producer.ManagedProjects))]
        [ValidateNever]
        public Producer Producer { get; set; } = null!;

        public int? StudioRoomId { get; set; }

        [ForeignKey(nameof(StudioRoomId))]
        [ValidateNever]
        public StudioRoom? StudioRoom { get; set; }

        [ValidateNever]
        public virtual ICollection<ProjectVersion> Versions { get; set; } = new List<ProjectVersion>();
    }
}

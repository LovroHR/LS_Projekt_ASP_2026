using Microsoft.AspNetCore.Mvc.RazorPages;
using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Data;
using System.Collections.Generic;
using System.Linq;

namespace LS_Projekt_ASP_2026.Pages.Projects
{
    public class IndexModel : PageModel
    {
        private readonly IRepository _repository;

        public List<AudioProject> Projects { get; set; } = new();

        public IndexModel(IRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            // Učitaj sve projekte za trenutnog korisnika
            Projects = _repository.GetAllProjects()
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
        }

        // Helper metoda za status labelu
        public string GetStatusLabel(ProjectStatus status)
        {
            return status switch
            {
                ProjectStatus.Draft => "Draft",
                ProjectStatus.Active => "Active",
                ProjectStatus.WaitingForFeedback => "Čeka feedback",
                ProjectStatus.Revision => "Revizija",
                ProjectStatus.Approved => "Approved",
                ProjectStatus.Archived => "Archived",
                _ => "Unknown"
            };
        }

        // Helper metoda za ikone projekta
        public string GetProjectTypeIcon(ProjectType type)
        {
            return type switch
            {
                ProjectType.Single => "🎵",
                ProjectType.Album => "💿",
                ProjectType.EP => "📀",
                ProjectType.Podcast => "🎙️",
                ProjectType.VoiceOver => "🎤",
                _ => "🎼"
            };
        }

        // Helper metoda za relativno vrijeme
        public string GetTimeAgo(DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan.TotalSeconds < 60)
                return "upravo sada";

            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes}min";

            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours}h";

            if (timeSpan.TotalDays < 7)
                return $"{(int)timeSpan.TotalDays}d";

            if (timeSpan.TotalDays < 30)
                return $"{(int)(timeSpan.TotalDays / 7)}w";

            if (timeSpan.TotalDays < 365)
                return $"{(int)(timeSpan.TotalDays / 30)}m";

            return $"{(int)(timeSpan.TotalDays / 365)}y";
        }
    }
}

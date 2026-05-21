using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Data;
using System.Collections.Generic;
using System.Linq;
using System;

namespace LS_Projekt_ASP_2026.Pages.Projects
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IRepository _repository;

        public List<AudioProject> Projects { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Q { get; set; }

        public IndexModel(IRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            var projects = _repository.GetAllProjects().AsEnumerable();
            if (!string.IsNullOrWhiteSpace(Q))
            {
                var q = Q.Trim();
                projects = projects.Where(p =>
                    (p.Title ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    (p.Genre ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    (p.Client?.Name ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    (p.Producer?.Name ?? "").Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            Projects = projects.OrderByDescending(p => p.CreatedAt).ToList();
        }

        public IActionResult OnGetData(string? q)
        {
            var projects = _repository.GetAllProjects().AsEnumerable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var tq = q.Trim();
                projects = projects.Where(p =>
                    (p.Title ?? "").Contains(tq, StringComparison.OrdinalIgnoreCase) ||
                    (p.Genre ?? "").Contains(tq, StringComparison.OrdinalIgnoreCase) ||
                    (p.Client?.Name ?? "").Contains(tq, StringComparison.OrdinalIgnoreCase) ||
                    (p.Producer?.Name ?? "").Contains(tq, StringComparison.OrdinalIgnoreCase));
            }

            return new JsonResult(projects.Select(p => new
            {
                p.Id,
                p.Title,
                Type = p.Type.ToString(),
                Status = p.Status.ToString(),
                p.Genre,
                p.Budget,
                ClientName = p.Client?.Name + " " + p.Client?.Surname,
                ProducerName = p.Producer?.Name + " " + p.Producer?.Surname
            }).ToList());
        }

        public IActionResult OnPostDelete(int id)
        {
            _repository.DeleteProject(id);
            TempData["Message"] = "Projekt je obrisan.";
            return RedirectToPage();
        }
    }
}

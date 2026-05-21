using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;
using System.Collections.Generic;
using System.Linq;
using System;

namespace LS_Projekt_ASP_2026.Pages.Producers
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IRepository _repository;

        public List<Producer> Producers { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Q { get; set; }

        public IndexModel(IRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            var producers = _repository.GetAllProducers().AsQueryable();
            if (!string.IsNullOrWhiteSpace(Q))
            {
                var q = Q.Trim();
                producers = producers.Where(p =>
                    (p.Name ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    (p.Surname ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    (p.Email ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    (p.Specialization ?? "").Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            Producers = producers.OrderBy(p => p.Name).ThenBy(p => p.Surname).ToList();
        }

        public IActionResult OnGetData(string? q)
        {
            var producers = _repository.GetAllProducers().AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var tq = q.Trim();
                producers = producers.Where(p =>
                    (p.Name ?? "").Contains(tq, StringComparison.OrdinalIgnoreCase) ||
                    (p.Surname ?? "").Contains(tq, StringComparison.OrdinalIgnoreCase) ||
                    (p.Email ?? "").Contains(tq, StringComparison.OrdinalIgnoreCase) ||
                    (p.Specialization ?? "").Contains(tq, StringComparison.OrdinalIgnoreCase));
            }

            return new JsonResult(producers.Select(p => new
            {
                p.Id,
                p.Name,
                p.Surname,
                p.Email,
                p.PhoneNumber,
                p.Specialization,
                p.HourlyRate,
                p.IsExternalCollaborator
            }).ToList());
        }

        public IActionResult OnPostDelete(int id)
        {
            if (_repository.GetAllBookings().Any(b => b.ProducerId == id) || _repository.GetAllProjects().Any(p => p.ProducerId == id))
            {
                TempData["Message"] = "Ne može se obrisati producent koji ima povezane rezervacije ili projekte.";
                return RedirectToPage();
            }

            _repository.DeleteProducer(id);
            TempData["Message"] = "Producent je obrisan.";
            return RedirectToPage();
        }
    }
}

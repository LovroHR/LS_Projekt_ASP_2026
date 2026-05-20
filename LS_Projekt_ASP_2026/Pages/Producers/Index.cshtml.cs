using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;

namespace LS_Projekt_ASP_2026.Pages.Producers
{
    public class IndexModel : PageModel
    {
        private readonly IRepository _repository;

        public List<Producer> Producers { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string Sort { get; set; } = "name";

        [BindProperty(SupportsGet = true)]
        public string Dir { get; set; } = "asc";

        [BindProperty(SupportsGet = true)]
        public string Alpha { get; set; } = "";

        public IndexModel(IRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            var producers = _repository.GetAllProducers().AsQueryable();
            var sortKey = (Sort ?? "name").ToLowerInvariant();
            var isDesc = string.Equals(Dir, "desc", StringComparison.OrdinalIgnoreCase);

            var alphaKey = (Alpha ?? "").ToLowerInvariant();
            if (alphaKey is "asc" or "desc")
            {
                Producers = alphaKey == "desc"
                    ? producers.OrderByDescending(p => p.Name).ThenByDescending(p => p.Surname).ToList()
                    : producers.OrderBy(p => p.Name).ThenBy(p => p.Surname).ToList();
                return;
            }

            producers = sortKey switch
            {
                "email" => isDesc ? producers.OrderByDescending(p => p.Email) : producers.OrderBy(p => p.Email),
                "specialization" => isDesc ? producers.OrderByDescending(p => p.Specialization) : producers.OrderBy(p => p.Specialization),
                "rate" => isDesc ? producers.OrderByDescending(p => p.HourlyRate) : producers.OrderBy(p => p.HourlyRate),
                _ => isDesc
                    ? producers.OrderByDescending(p => p.Name).ThenByDescending(p => p.Surname)
                    : producers.OrderBy(p => p.Name).ThenBy(p => p.Surname)
            };

            Producers = producers.ToList();
        }

        public string NextDir(string column)
        {
            if (string.Equals(Sort, column, StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(Dir, "asc", StringComparison.OrdinalIgnoreCase) ? "desc" : "asc";
            }

            return "asc";
        }
    }
}

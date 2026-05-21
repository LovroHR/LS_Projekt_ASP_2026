using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;
using System.Collections.Generic;
using System.Linq;
using System;

namespace LS_Projekt_ASP_2026.Pages.Clients
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IRepository _repository;

        public List<Client> Clients { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Q { get; set; }

        public IndexModel(IRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            var clients = _repository.GetAllClients().AsQueryable();
            if (!string.IsNullOrWhiteSpace(Q))
            {
                var q = Q.Trim();
                clients = clients.Where(c =>
                    (c.Name ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    (c.Surname ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    (c.Email ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    (c.CompanyName ?? "").Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            Clients = clients.OrderBy(c => c.Name).ThenBy(c => c.Surname).ToList();
        }

        public IActionResult OnGetData(string? q)
        {
            var clients = _repository.GetAllClients().AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var tq = q.Trim();
                clients = clients.Where(c =>
                    (c.Name ?? "").Contains(tq, StringComparison.OrdinalIgnoreCase) ||
                    (c.Surname ?? "").Contains(tq, StringComparison.OrdinalIgnoreCase) ||
                    (c.Email ?? "").Contains(tq, StringComparison.OrdinalIgnoreCase) ||
                    (c.CompanyName ?? "").Contains(tq, StringComparison.OrdinalIgnoreCase));
            }

            return new JsonResult(clients.Select(c => new
            {
                c.Id,
                c.Name,
                c.Surname,
                c.Email,
                c.PhoneNumber,
                c.CompanyName,
                c.IsPriorityClient
            }).ToList());
        }

        public IActionResult OnPostDelete(int id)
        {
            if (_repository.GetBookingsByClient(id).Any() || (_repository.GetClientById(id)?.Projects?.Any() ?? false))
            {
                TempData["Message"] = "Ne može se obrisati klijent koji ima povezane rezervacije ili projekte.";
                return RedirectToPage();
            }

            _repository.DeleteClient(id);
            TempData["Message"] = "Klijent je obrisan.";
            return RedirectToPage();
        }
    }
}

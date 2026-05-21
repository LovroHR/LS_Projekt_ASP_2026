using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;
using System.Collections.Generic;
using System.Linq;
using System;

namespace LS_Projekt_ASP_2026.Pages.StudioRooms
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IRepository _repository;

        public List<StudioRoom> StudioRooms { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Q { get; set; }

        public IndexModel(IRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            var rooms = _repository.GetAllStudioRooms().AsQueryable();

            if (!string.IsNullOrWhiteSpace(Q))
            {
                var q = Q.Trim();
                rooms = rooms.Where(r => (r.Name ?? "").Contains(q, StringComparison.OrdinalIgnoreCase)
                                         || (r.Location ?? "").Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            StudioRooms = rooms.ToList();
        }

        // AJAX handler returning JSON data for search
        public IActionResult OnGetData(string? q)
        {
            var rooms = _repository.GetAllStudioRooms().AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var tq = q.Trim();
                rooms = rooms.Where(r => (r.Name ?? "").Contains(tq, StringComparison.OrdinalIgnoreCase)
                                         || (r.Location ?? "").Contains(tq, StringComparison.OrdinalIgnoreCase));
            }

            var result = rooms.Select(r => new
            {
                r.Id,
                r.Name,
                r.Location,
                r.Capacity,
                HourlyPrice = r.HourlyPrice
            }).ToList();

            return new JsonResult(result);
        }

        public IActionResult OnPostDelete(int id)
        {
            var bookings = _repository.GetBookingsByStudio(id);
            if (bookings != null && bookings.Any())
            {
                TempData["Message"] = "Ne može se obrisati prostor koji ima rezervacije.";
                return RedirectToPage();
            }

            _repository.DeleteStudioRoom(id);
            TempData["Message"] = "Prostor je obrisan.";
            return RedirectToPage();
        }
    }
}

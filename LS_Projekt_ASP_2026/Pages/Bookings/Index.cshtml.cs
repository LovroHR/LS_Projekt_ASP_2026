using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Data;
using LS_Projekt_ASP_2026.Services;
using System.Collections.Generic;
using System.Linq;
using System;

namespace LS_Projekt_ASP_2026.Pages.Bookings
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IRepository _repository;

        public List<Booking> Bookings { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Q { get; set; }

        public IndexModel(IRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            var bookings = FilterForCurrentUser(_repository.GetAllBookings().AsEnumerable());
            if (!string.IsNullOrWhiteSpace(Q))
            {
                var q = Q.Trim();
                bookings = bookings.Where(b =>
                    (b.Purpose ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    (b.Client?.Name ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    (b.Producer?.Name ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    (b.StudioRoom?.Name ?? "").Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            Bookings = bookings.OrderByDescending(b => b.StartTime).ToList();
        }

        public IActionResult OnGetData(string? q)
        {
            var bookings = FilterForCurrentUser(_repository.GetAllBookings().AsEnumerable());
            if (!string.IsNullOrWhiteSpace(q))
            {
                var tq = q.Trim();
                bookings = bookings.Where(b =>
                    (b.Purpose ?? "").Contains(tq, StringComparison.OrdinalIgnoreCase) ||
                    (b.Client?.Name ?? "").Contains(tq, StringComparison.OrdinalIgnoreCase) ||
                    (b.Producer?.Name ?? "").Contains(tq, StringComparison.OrdinalIgnoreCase) ||
                    (b.StudioRoom?.Name ?? "").Contains(tq, StringComparison.OrdinalIgnoreCase));
            }

            return new JsonResult(bookings.Select(b => new
            {
                b.Id,
                b.Purpose,
                Status = b.Status.ToString(),
                b.StartTime,
                b.EndTime,
                ClientName = b.Client?.Name + " " + b.Client?.Surname,
                ProducerName = b.Producer?.Name + " " + b.Producer?.Surname,
                StudioName = b.StudioRoom?.Name,
                b.TotalPrice,
                b.RequiresEngineer
            }).ToList());
        }

        public IActionResult OnPostDelete(int id)
        {
            if (!User.IsInRole("Admin") && !User.IsInRole("Producer"))
            {
                return Forbid();
            }

            _repository.DeleteBooking(id);
            TempData["Message"] = "Rezervacija je obrisana.";
            return RedirectToPage();
        }

        private IEnumerable<Booking> FilterForCurrentUser(IEnumerable<Booking> bookings)
        {
            var userId = User.GetUserId();

            if (User.IsInRole("Admin") || userId is null)
            {
                return bookings;
            }

            if (User.IsInRole("Producer"))
            {
                return bookings.Where(b => b.ProducerId == userId.Value);
            }

            return bookings.Where(b => b.ClientId == userId.Value);
        }
    }
}

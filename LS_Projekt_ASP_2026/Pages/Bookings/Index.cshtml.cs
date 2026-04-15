using Microsoft.AspNetCore.Mvc.RazorPages;
using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Data;
using System.Collections.Generic;

namespace LS_Projekt_ASP_2026.Pages.Bookings
{
    public class IndexModel : PageModel
    {
        private readonly IRepository _repository;

        public List<Booking> Bookings { get; set; } = new();

        public IndexModel(IRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            // Dohvati sve bookinge iz repository-ja
            Bookings = new List<Booking>(_repository.GetAllBookings());
        }
    }
}

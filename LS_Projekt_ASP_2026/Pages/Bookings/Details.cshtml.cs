using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;

namespace LS_Projekt_ASP_2026.Pages.Bookings
{
    public class DetailsModel : PageModel
    {
        private readonly IRepository _repository;

        public Booking? Booking { get; set; }

        public DetailsModel(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult OnGet(int id)
        {
            Booking = _repository.GetBookingById(id);
            if (Booking == null) return RedirectToPage("Index");
            return Page();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;

namespace LS_Projekt_ASP_2026.Pages.StudioRooms
{
    public class DeleteModel : PageModel
    {
        private readonly IRepository _repository;

        public DeleteModel(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult OnPost(int id)
        {
            var bookings = _repository.GetBookingsByStudio(id);
            if (bookings != null && bookings.Any())
            {
                TempData["Message"] = "Ne može se obrisati prostor koji ima rezervacije.";
                return RedirectToPage("Index");
            }

            _repository.DeleteStudioRoom(id);
            TempData["Message"] = "Prostor je obrisan.";
            return RedirectToPage("Index");
        }
    }
}

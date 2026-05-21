using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;

namespace LS_Projekt_ASP_2026.Pages.Bookings
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
            _repository.DeleteBooking(id);
            TempData["Message"] = "Rezervacija je obrisana.";
            return RedirectToPage("Index");
        }
    }
}

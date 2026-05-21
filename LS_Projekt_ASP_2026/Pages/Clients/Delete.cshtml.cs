using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;
using System.Linq;

namespace LS_Projekt_ASP_2026.Pages.Clients
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
            if (_repository.GetBookingsByClient(id).Any() || (_repository.GetClientById(id)?.Projects?.Any() ?? false))
            {
                TempData["Message"] = "Ne može se obrisati klijent koji ima povezane rezervacije ili projekte.";
                return RedirectToPage("Index");
            }

            _repository.DeleteClient(id);
            TempData["Message"] = "Klijent je obrisan.";
            return RedirectToPage("Index");
        }
    }
}

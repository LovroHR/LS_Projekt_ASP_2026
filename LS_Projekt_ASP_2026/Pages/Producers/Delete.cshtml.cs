using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;
using System.Linq;

namespace LS_Projekt_ASP_2026.Pages.Producers
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
            if (_repository.GetAllBookings().Any(b => b.ProducerId == id) || _repository.GetAllProjects().Any(p => p.ProducerId == id))
            {
                TempData["Message"] = "Ne može se obrisati producent koji ima povezane rezervacije ili projekte.";
                return RedirectToPage("Index");
            }

            _repository.DeleteProducer(id);
            TempData["Message"] = "Producent je obrisan.";
            return RedirectToPage("Index");
        }
    }
}

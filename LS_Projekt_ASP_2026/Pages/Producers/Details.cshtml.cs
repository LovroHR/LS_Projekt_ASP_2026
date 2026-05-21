using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;

namespace LS_Projekt_ASP_2026.Pages.Producers
{
    public class DetailsModel : PageModel
    {
        private readonly IRepository _repository;

        public Producer? Producer { get; set; }

        public DetailsModel(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult OnGet(int id)
        {
            Producer = _repository.GetProducerById(id);
            if (Producer == null) return RedirectToPage("Index");
            return Page();
        }
    }
}

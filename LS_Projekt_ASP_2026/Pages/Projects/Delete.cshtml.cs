using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;

namespace LS_Projekt_ASP_2026.Pages.Projects
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
            _repository.DeleteProject(id);
            TempData["Message"] = "Projekt je obrisan.";
            return RedirectToPage("Index");
        }
    }
}

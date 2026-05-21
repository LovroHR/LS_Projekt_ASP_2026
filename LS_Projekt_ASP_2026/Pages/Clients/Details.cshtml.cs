using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;

namespace LS_Projekt_ASP_2026.Pages.Clients
{
    public class DetailsModel : PageModel
    {
        private readonly IRepository _repository;

        public Client? Client { get; set; }

        public DetailsModel(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult OnGet(int id)
        {
            Client = _repository.GetClientById(id);
            if (Client == null)
            {
                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}

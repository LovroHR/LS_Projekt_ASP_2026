using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;

namespace LS_Projekt_ASP_2026.Pages.Clients
{
    public class CreateModel : PageModel
    {
        private readonly IRepository _repository;

        [BindProperty]
        public Client Input { get; set; } = new();

        public CreateModel(IRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            AutocompleteValidation.ValidateCountrySelection(ModelState, Input.Country);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Input.Role = UserRole.Client;
            Input.CreatedAt = DateTime.Now;
            _repository.AddClient(Input);
            TempData["Message"] = "Klijent je kreiran.";
            return RedirectToPage("Index");
        }
    }
}

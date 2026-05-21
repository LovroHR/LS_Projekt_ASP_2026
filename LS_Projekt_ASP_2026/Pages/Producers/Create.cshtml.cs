using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;
using System.Linq;

namespace LS_Projekt_ASP_2026.Pages.Producers
{
    public class CreateModel : PageModel
    {
        private readonly IRepository _repository;

        [BindProperty]
        public Producer Input { get; set; } = new();

        public CreateModel(IRepository repository)
        {
            _repository = repository;
        }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var emailExists = _repository.GetAllClients().Any(x => x.Email == Input.Email)
                || _repository.GetAllProducers().Any(x => x.Email == Input.Email);

            if (emailExists)
            {
                ModelState.AddModelError("Input.Email", "Korisnik s tim emailom već postoji.");
                return Page();
            }

            Input.Role = UserRole.Producer;
            Input.CreatedAt = DateTime.Now;
            _repository.AddProducer(Input);
            TempData["Message"] = "Producent je kreiran.";
            return RedirectToPage("Index");
        }
    }
}

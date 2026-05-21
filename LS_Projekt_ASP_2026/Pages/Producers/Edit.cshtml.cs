using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;
using System.Linq;

namespace LS_Projekt_ASP_2026.Pages.Producers
{
    public class EditModel : PageModel
    {
        private readonly IRepository _repository;

        [BindProperty]
        public Producer Input { get; set; } = new();

        public EditModel(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult OnGet(int id)
        {
            var producer = _repository.GetProducerById(id);
            if (producer == null) return RedirectToPage("Index");
            Input = producer;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var existing = _repository.GetProducerById(Input.Id);
            if (existing == null) return RedirectToPage("Index");

            var emailExists = _repository.GetAllClients().Any(x => x.Email == Input.Email)
                || _repository.GetAllProducers().Any(x => x.Email == Input.Email && x.Id != Input.Id);

            if (emailExists)
            {
                ModelState.AddModelError("Input.Email", "Korisnik s tim emailom već postoji.");
                return Page();
            }

            existing.Name = Input.Name;
            existing.Surname = Input.Surname;
            existing.Email = Input.Email;
            existing.PhoneNumber = Input.PhoneNumber;
            existing.Specialization = Input.Specialization;
            existing.HourlyRate = Input.HourlyRate;
            existing.IsExternalCollaborator = Input.IsExternalCollaborator;
            existing.Biography = Input.Biography;

            _repository.UpdateProducer(existing);
            TempData["Message"] = "Producent je ažuriran.";
            return RedirectToPage("Index");
        }
    }
}

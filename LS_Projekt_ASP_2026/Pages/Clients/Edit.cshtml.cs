using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;

namespace LS_Projekt_ASP_2026.Pages.Clients
{
    public class EditModel : PageModel
    {
        private readonly IRepository _repository;

        [BindProperty]
        public Client Input { get; set; } = new();

        public EditModel(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult OnGet(int id)
        {
            var client = _repository.GetClientById(id);
            if (client == null)
            {
                return RedirectToPage("Index");
            }

            Input = client;
            return Page();
        }

        public IActionResult OnPost()
        {
            AutocompleteValidation.ValidateCountrySelection(ModelState, Input.Country);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existing = _repository.GetClientById(Input.Id);
            if (existing == null)
            {
                return RedirectToPage("Index");
            }

            existing.Name = Input.Name;
            existing.Surname = Input.Surname;
            existing.Email = Input.Email;
            existing.PhoneNumber = Input.PhoneNumber;
            existing.Password = Input.Password;
            existing.DateOfBirth = Input.DateOfBirth;
            existing.Address = Input.Address;
            existing.Country = Input.Country;
            existing.CompanyName = Input.CompanyName;
            existing.BillingAddress = Input.BillingAddress;
            existing.IsPriorityClient = Input.IsPriorityClient;
            existing.Notes = Input.Notes;

            _repository.UpdateClient(existing);
            TempData["Message"] = "Klijent je ažuriran.";
            return RedirectToPage("Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Data;

namespace LS_Projekt_ASP_2026.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IRepository _repository;

        [BindProperty]
        public string Email { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";

        public string ErrorMessage { get; set; } = "";

        public LoginModel(IRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Mail i lozinka su obavezni";
                return Page();
            }

            // Pronađi klijenta sa datim mailom i lozinkom
            var client = _repository.GetAllClients()
                .FirstOrDefault(c => c.Email == Email && c.Password == Password);

            if (client == null)
            {
                ErrorMessage = "Neispravni mail ili lozinka";
                return Page();
            }

            // Postavi session ili cookie za logiranog korisnika
            HttpContext.Session.SetString("UserId", client.Id.ToString());
            HttpContext.Session.SetString("UserEmail", client.Email);
            HttpContext.Session.SetString("UserName", $"{client.Name} {client.Surname}");

            return RedirectToPage("/Index");
        }
    }
}

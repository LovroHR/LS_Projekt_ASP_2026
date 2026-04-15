using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Data;

namespace LS_Projekt_ASP_2026.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly IRepository _repository;

        [BindProperty]
        public string Name { get; set; } = "";

        [BindProperty]
        public string Surname { get; set; } = "";

        [BindProperty]
        public string Email { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";

        [BindProperty]
        public string ConfirmPassword { get; set; } = "";

        [BindProperty]
        public DateTime DateOfBirth { get; set; }

        [BindProperty]
        public string PhoneNumber { get; set; } = "";

        [BindProperty]
        public string Address { get; set; } = "";

        [BindProperty]
        public string Country { get; set; } = "";

        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        public RegisterModel(IRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Validacija
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Surname))
            {
                ErrorMessage = "Ime i prezime su obavezni";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Email je obavezan";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
            {
                ErrorMessage = "Lozinka mora sadržavati najmanje 6 znakova";
                return Page();
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Lozinke se ne poklapaju";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                ErrorMessage = "Broj telefona je obavezan";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Address))
            {
                ErrorMessage = "Adresa je obavezna";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Country))
            {
                ErrorMessage = "Država je obavezna";
                return Page();
            }

            if (DateOfBirth == DateTime.MinValue)
            {
                ErrorMessage = "Datum rođenja je obavezan";
                return Page();
            }

            // Provjeri da li email već postoji
            var existingClient = _repository.GetAllClients()
                .FirstOrDefault(c => c.Email == Email);

            if (existingClient != null)
            {
                ErrorMessage = "Korisnik sa tim emailom već postoji";
                return Page();
            }

            // Kreiraj novog klijenta
            var newClient = new Client
            {
                Name = Name,
                Surname = Surname,
                Email = Email,
                Password = Password, // NAPOMENA: U produkciji trebao bi hash i salt
                PhoneNumber = PhoneNumber,
                Address = Address,
                Country = Country,
                DateOfBirth = DateOfBirth,
                Role = UserRole.Client,
                CreatedAt = DateTime.Now
            };

            // Spremi novog klijenta
            _repository.AddClient(newClient);

            SuccessMessage = "Registracija je uspješna! Sada se možeš prijaviti.";
            return RedirectToPage("/Auth/Login");
        }
    }
}

using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Data;
using LS_Projekt_ASP_2026.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LS_Projekt_ASP_2026.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityAppUser> _userManager;
        private readonly SignInManager<IdentityAppUser> _signInManager;

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

        [BindProperty]
        public string OIB { get; set; } = "";

        [BindProperty]
        public string JMBG { get; set; } = "";

        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";
        public IList<AuthenticationScheme> ExternalLogins { get; set; } = new List<AuthenticationScheme>();

        public RegisterModel(
            AppDbContext context,
            UserManager<IdentityAppUser> userManager,
            SignInManager<IdentityAppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task OnGetAsync()
        {
            if (DateOfBirth == DateTime.MinValue)
            {
                DateOfBirth = DateTime.Now.AddYears(-20);
            }

            await LoadExternalLogins();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadExternalLogins();

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
                ErrorMessage = "Lozinka mora sadrzavati najmanje 6 znakova";
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
                ErrorMessage = "Drzava je obavezna";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(OIB) || OIB.Length != 11 || !OIB.All(char.IsDigit))
            {
                ErrorMessage = "OIB mora imati tocno 11 znamenki";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(JMBG) || JMBG.Length != 13 || !JMBG.All(char.IsDigit))
            {
                ErrorMessage = "JMBG mora imati tocno 13 znamenki";
                return Page();
            }

            if (DateOfBirth == DateTime.MinValue)
            {
                ErrorMessage = "Datum rodenja je obavezan";
                return Page();
            }

            var email = Email.Trim();
            var existingBusinessUser = await _context.BusinessUsers.AnyAsync(c => c.Email == email);
            var existingIdentityUser = await _userManager.FindByEmailAsync(email);
            if (existingBusinessUser || existingIdentityUser != null)
            {
                ErrorMessage = "Korisnik s tim emailom vec postoji";
                return Page();
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            var newClient = new Client
            {
                Name = Name.Trim(),
                Surname = Surname.Trim(),
                Email = email,
                Password = null,
                PhoneNumber = PhoneNumber.Trim(),
                Address = Address.Trim(),
                Country = Country.Trim(),
                OIB = OIB.Trim(),
                JMBG = JMBG.Trim(),
                DateOfBirth = DateOfBirth,
                Role = UserRole.Client,
                CreatedAt = DateTime.Now
            };

            _context.Clients.Add(newClient);
            await _context.SaveChangesAsync();

            var identityUser = new IdentityAppUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                PhoneNumber = PhoneNumber.Trim(),
                Name = Name.Trim(),
                Surname = Surname.Trim(),
                OIB = OIB.Trim(),
                JMBG = JMBG.Trim(),
                DateOfBirth = DateOfBirth,
                Address = Address.Trim(),
                Country = Country.Trim(),
                CreatedAt = DateTime.Now,
                BusinessUserId = newClient.Id
            };

            var createResult = await _userManager.CreateAsync(identityUser, Password);
            if (!createResult.Succeeded)
            {
                await transaction.RollbackAsync();
                ErrorMessage = string.Join(" ", createResult.Errors.Select(e => e.Description));
                return Page();
            }

            var roleResult = await _userManager.AddToRoleAsync(identityUser, UserRole.Client.ToString());
            if (!roleResult.Succeeded)
            {
                await transaction.RollbackAsync();
                ErrorMessage = string.Join(" ", roleResult.Errors.Select(e => e.Description));
                return Page();
            }

            await transaction.CommitAsync();
            await _signInManager.SignInAsync(identityUser, isPersistent: true);
            SuccessMessage = "Registracija je uspjesna.";
            return RedirectToPage("/Index");
        }

        private async Task LoadExternalLogins()
        {
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }
    }
}

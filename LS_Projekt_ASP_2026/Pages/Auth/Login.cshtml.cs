using System.Security.Claims;
using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Data;
using LS_Projekt_ASP_2026.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LS_Projekt_ASP_2026.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<IdentityAppUser> _signInManager;
        private readonly UserManager<IdentityAppUser> _userManager;

        [BindProperty]
        public string Email { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";

        public string ErrorMessage { get; set; } = "";
        public IList<AuthenticationScheme> ExternalLogins { get; set; } = new List<AuthenticationScheme>();

        public LoginModel(
            AppDbContext context,
            SignInManager<IdentityAppUser> signInManager,
            UserManager<IdentityAppUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            await LoadExternalLogins();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Mail i lozinka su obavezni";
                await LoadExternalLogins();
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Email.Trim());
            if (user == null)
            {
                ErrorMessage = "Neispravni mail ili lozinka";
                await LoadExternalLogins();
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(user, Password, isPersistent: true, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                ErrorMessage = "Neispravni mail ili lozinka";
                await LoadExternalLogins();
                return Page();
            }

            return LocalRedirect(GetSafeReturnUrl(returnUrl));
        }

        public IActionResult OnPostExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Page("/Auth/Login", "ExternalLoginCallback", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> OnGetExternalLoginCallbackAsync(string? returnUrl = null, string? remoteError = null)
        {
            if (!string.IsNullOrWhiteSpace(remoteError))
            {
                ErrorMessage = $"Google prijava nije uspjela: {remoteError}";
                await LoadExternalLogins();
                return Page();
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Nije moguce ucitati podatke Google prijave.";
                await LoadExternalLogins();
                return Page();
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: true,
                bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(GetSafeReturnUrl(returnUrl));
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
            {
                ErrorMessage = "Google racun nije vratio email adresu.";
                await LoadExternalLogins();
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = await CreateLocalUserFromGoogle(info, email);
            }

            var loginResult = await _userManager.AddLoginAsync(user, info);
            if (!loginResult.Succeeded && loginResult.Errors.All(e => e.Code != "LoginAlreadyAssociated"))
            {
                ErrorMessage = string.Join(" ", loginResult.Errors.Select(e => e.Description));
                await LoadExternalLogins();
                return Page();
            }

            await _signInManager.SignInAsync(user, isPersistent: true, info.LoginProvider);
            return LocalRedirect(GetSafeReturnUrl(returnUrl));
        }

        private async Task<IdentityAppUser> CreateLocalUserFromGoogle(ExternalLoginInfo info, string email)
        {
            var givenName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
            var surname = info.Principal.FindFirstValue(ClaimTypes.Surname);
            var fullName = info.Principal.FindFirstValue(ClaimTypes.Name);
            var nameParts = (fullName ?? email.Split('@')[0]).Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var name = !string.IsNullOrWhiteSpace(givenName)
                ? givenName
                : nameParts.FirstOrDefault() ?? "Google";
            var lastName = !string.IsNullOrWhiteSpace(surname)
                ? surname
                : nameParts.Skip(1).FirstOrDefault() ?? "User";

            await using var transaction = await _context.Database.BeginTransactionAsync();
            var client = new Client
            {
                Name = name,
                Surname = lastName,
                Email = email,
                PhoneNumber = "",
                Password = null,
                CreatedAt = DateTime.Now,
                Role = UserRole.Client,
                DateOfBirth = DateTime.Now.AddYears(-18),
                Address = "",
                Country = "",
                OIB = "00000000000",
                JMBG = "0000000000000",
                Notes = "Korisnik kreiran preko Google prijave."
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            var user = new IdentityAppUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                Name = name,
                Surname = lastName,
                OIB = "00000000000",
                JMBG = "0000000000000",
                CreatedAt = DateTime.Now,
                BusinessUserId = client.Id
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(string.Join(" ", createResult.Errors.Select(e => e.Description)));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, UserRole.Client.ToString());
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(string.Join(" ", roleResult.Errors.Select(e => e.Description)));
            }

            await transaction.CommitAsync();
            return user;
        }

        private async Task LoadExternalLogins()
        {
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        private string GetSafeReturnUrl(string? returnUrl)
        {
            return !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
                ? returnUrl
                : Url.Content("~/");
        }
    }
}

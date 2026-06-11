using System.ComponentModel.DataAnnotations;
using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Data;
using LS_Projekt_ASP_2026.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LS_Projekt_ASP_2026.Pages.Profile;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityAppUser> _userManager;
    private readonly SignInManager<IdentityAppUser> _signInManager;

    public IndexModel(
        AppDbContext context,
        UserManager<IdentityAppUser> userManager,
        SignInManager<IdentityAppUser> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [BindProperty]
    public ProfileInput Input { get; set; } = new();

    public string Role { get; set; } = "";

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }

        await PopulateInput(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }

        Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "";
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var userName = Input.UserName.Trim();
        var email = Input.Email.Trim();

        var userWithSameName = await _userManager.FindByNameAsync(userName);
        if (userWithSameName != null && userWithSameName.Id != user.Id)
        {
            ModelState.AddModelError(nameof(Input.UserName), "Username je vec zauzet.");
            return Page();
        }

        var userWithSameEmail = await _userManager.FindByEmailAsync(email);
        if (userWithSameEmail != null && userWithSameEmail.Id != user.Id)
        {
            ModelState.AddModelError(nameof(Input.Email), "Email je vec zauzet.");
            return Page();
        }

        user.UserName = userName;
        user.Email = email;
        user.EmailConfirmed = true;
        user.Name = Input.Name.Trim();
        user.Surname = Input.Surname.Trim();
        user.PhoneNumber = Input.PhoneNumber.Trim();
        user.DateOfBirth = Input.DateOfBirth;
        user.Address = Input.Address.Trim();
        user.Country = Input.Country.Trim();

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        await SyncBusinessUser(user);
        await _signInManager.RefreshSignInAsync(user);

        TempData["Message"] = "Profil je azuriran.";
        return RedirectToPage();
    }

    private async Task PopulateInput(IdentityAppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        Role = roles.FirstOrDefault() ?? "";

        if (user.BusinessUserId.HasValue)
        {
            var businessUser = await _context.BusinessUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == user.BusinessUserId.Value);

            if (businessUser is Client client)
            {
                user.DateOfBirth ??= client.DateOfBirth;
                user.Address ??= client.Address;
                user.Country ??= client.Country;
            }
        }

        Input = new ProfileInput
        {
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            Name = user.Name,
            Surname = user.Surname,
            PhoneNumber = user.PhoneNumber ?? "",
            DateOfBirth = user.DateOfBirth,
            Address = user.Address ?? "",
            Country = user.Country ?? ""
        };
    }

    private async Task SyncBusinessUser(IdentityAppUser identityUser)
    {
        if (!identityUser.BusinessUserId.HasValue)
        {
            return;
        }

        var businessUser = await _context.BusinessUsers.FirstOrDefaultAsync(x => x.Id == identityUser.BusinessUserId.Value);
        if (businessUser == null)
        {
            return;
        }

        businessUser.Name = identityUser.Name;
        businessUser.Surname = identityUser.Surname;
        businessUser.Email = identityUser.Email ?? businessUser.Email;
        businessUser.PhoneNumber = identityUser.PhoneNumber ?? businessUser.PhoneNumber;

        if (businessUser is Client client)
        {
            client.DateOfBirth = identityUser.DateOfBirth ?? client.DateOfBirth;
            client.Address = identityUser.Address ?? "";
            client.Country = identityUser.Country ?? "";
        }

        await _context.SaveChangesAsync();
    }

    public class ProfileInput
    {
        [Required(ErrorMessage = "Username je obavezan.")]
        [MaxLength(256)]
        public string UserName { get; set; } = "";

        [Required(ErrorMessage = "Email je obavezan.")]
        [EmailAddress(ErrorMessage = "Email nije valjan.")]
        [MaxLength(256)]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Ime je obavezno.")]
        [MaxLength(100)]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Prezime je obavezno.")]
        [MaxLength(100)]
        public string Surname { get; set; } = "";

        [MaxLength(30)]
        public string PhoneNumber { get; set; } = "";

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(255)]
        public string Address { get; set; } = "";

        [MaxLength(100)]
        public string Country { get; set; } = "";
    }
}

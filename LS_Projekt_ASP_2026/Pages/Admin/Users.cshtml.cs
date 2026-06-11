using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Data;
using LS_Projekt_ASP_2026.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LS_Projekt_ASP_2026.Pages.Admin;

public class UsersModel : PageModel
{
    private static readonly string[] Roles = { "Admin", "Producer", "Client" };
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityAppUser> _userManager;

    public UsersModel(AppDbContext context, UserManager<IdentityAppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty(SupportsGet = true)]
    public string? Q { get; set; }

    [BindProperty]
    public UserEditInput Input { get; set; } = new();

    public List<UserRow> Users { get; set; } = new();
    public IReadOnlyList<string> AvailableRoles => Roles;

    public async Task OnGetAsync()
    {
        await LoadUsers();
    }

    public async Task<IActionResult> OnPostUpdateAsync()
    {
        if (!Roles.Contains(Input.Role))
        {
            TempData["Message"] = "Odabrana rola nije valjana.";
            return RedirectToPage(new { Q });
        }

        var user = await _userManager.FindByIdAsync(Input.IdentityUserId.ToString());
        if (user == null)
        {
            TempData["Message"] = "Korisnik nije pronaden.";
            return RedirectToPage(new { Q });
        }

        var currentUserId = _userManager.GetUserId(User);
        if (currentUserId == user.Id.ToString() && Input.Role != "Admin")
        {
            TempData["Message"] = "Ne mozes sam sebi maknuti Admin rolu.";
            return RedirectToPage(new { Q });
        }

        var email = Input.Email.Trim();
        var existing = await _userManager.FindByEmailAsync(email);
        if (existing != null && existing.Id != user.Id)
        {
            TempData["Message"] = "Email je vec zauzet.";
            return RedirectToPage(new { Q });
        }

        user.Name = Input.Name.Trim();
        user.Surname = Input.Surname.Trim();
        user.Email = email;
        user.UserName = email;
        user.PhoneNumber = Input.PhoneNumber.Trim();

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            TempData["Message"] = string.Join(" ", updateResult.Errors.Select(e => e.Description));
            return RedirectToPage(new { Q });
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeResult.Succeeded)
        {
            TempData["Message"] = string.Join(" ", removeResult.Errors.Select(e => e.Description));
            return RedirectToPage(new { Q });
        }

        var addResult = await _userManager.AddToRoleAsync(user, Input.Role);
        if (!addResult.Succeeded)
        {
            TempData["Message"] = string.Join(" ", addResult.Errors.Select(e => e.Description));
            return RedirectToPage(new { Q });
        }

        await UpdateBusinessUser(user, Input.Role);
        TempData["Message"] = "Korisnik je azuriran.";
        return RedirectToPage(new { Q });
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            TempData["Message"] = "Korisnik nije pronaden.";
            return RedirectToPage(new { Q });
        }

        if (_userManager.GetUserId(User) == user.Id.ToString())
        {
            TempData["Message"] = "Ne mozes obrisati sam sebe.";
            return RedirectToPage(new { Q });
        }

        if (user.BusinessUserId.HasValue && await HasBusinessDependencies(user.BusinessUserId.Value))
        {
            TempData["Message"] = "Korisnik ima povezane rezervacije, projekte ili komentare pa ga nije moguce obrisati.";
            return RedirectToPage(new { Q });
        }

        if (user.BusinessUserId.HasValue)
        {
            var businessUser = await _context.BusinessUsers.FirstOrDefaultAsync(x => x.Id == user.BusinessUserId.Value);
            if (businessUser != null)
            {
                _context.BusinessUsers.Remove(businessUser);
            }
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            TempData["Message"] = string.Join(" ", result.Errors.Select(e => e.Description));
            return RedirectToPage(new { Q });
        }

        await _context.SaveChangesAsync();
        TempData["Message"] = "Korisnik je obrisan.";
        return RedirectToPage(new { Q });
    }

    private async Task LoadUsers()
    {
        var query = _userManager.Users.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(Q))
        {
            var q = Q.Trim();
            query = query.Where(u => u.Name.Contains(q) || u.Surname.Contains(q) || (u.Email != null && u.Email.Contains(q)));
        }

        var users = await query.OrderBy(u => u.Name).ThenBy(u => u.Surname).ToListAsync();
        var currentUserId = _userManager.GetUserId(User);

        Users = new List<UserRow>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            Users.Add(new UserRow
            {
                IdentityUserId = user.Id,
                BusinessUserId = user.BusinessUserId,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email ?? "",
                PhoneNumber = user.PhoneNumber ?? "",
                Role = roles.FirstOrDefault() ?? "",
                IsCurrentUser = currentUserId == user.Id.ToString()
            });
        }
    }

    private async Task UpdateBusinessUser(IdentityAppUser identityUser, string role)
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
        await _context.SaveChangesAsync();

        var roleValue = (int)Enum.Parse<UserRole>(role);
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE Users SET Role = {roleValue} WHERE Id = {identityUser.BusinessUserId.Value}");
    }

    private async Task<bool> HasBusinessDependencies(int businessUserId)
    {
        return await _context.Bookings.AnyAsync(b => b.ClientId == businessUserId || b.ProducerId == businessUserId)
            || await _context.AudioProjects.AnyAsync(p => p.ClientId == businessUserId || p.ProducerId == businessUserId)
            || await _context.TimecodedComments.AnyAsync(c => c.AuthorId == businessUserId);
    }

    public class UserEditInput
    {
        public int IdentityUserId { get; set; }
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Role { get; set; } = "";
    }

    public class UserRow : UserEditInput
    {
        public int? BusinessUserId { get; set; }
        public bool IsCurrentUser { get; set; }
    }
}

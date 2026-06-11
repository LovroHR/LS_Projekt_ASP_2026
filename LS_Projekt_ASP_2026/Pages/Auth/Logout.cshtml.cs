using LS_Projekt_ASP_2026.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LS_Projekt_ASP_2026.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityAppUser> _signInManager;

        public LogoutModel(SignInManager<IdentityAppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
    }
}

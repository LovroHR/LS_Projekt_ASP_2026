using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;

namespace LS_Projekt_ASP_2026.Pages.StudioRooms
{
    public class EditModel : PageModel
    {
        private readonly IRepository _repository;

        [BindProperty]
        public StudioRoom Room { get; set; } = new();

        public EditModel(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult OnGet(int id)
        {
            var r = _repository.GetStudioRoomById(id);
            if (r == null) return RedirectToPage("Index");
            Room = r;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            _repository.UpdateStudioRoom(Room);
            TempData["Message"] = "Promjene su spremljene.";
            return RedirectToPage("Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;

namespace LS_Projekt_ASP_2026.Pages.StudioRooms
{
    public class CreateModel : PageModel
    {
        private readonly IRepository _repository;

        [BindProperty]
        public StudioRoom NewRoom { get; set; } = new();

        public CreateModel(IRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            NewRoom = NewRoom ?? new StudioRoom();
            _repository.AddStudioRoom(NewRoom);
            TempData["Message"] = "Prostor je dodan.";
            return RedirectToPage("Index");
        }
    }
}

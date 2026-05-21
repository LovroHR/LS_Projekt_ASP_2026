using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;

namespace LS_Projekt_ASP_2026.Pages.StudioRooms
{
    public class DetailsModel : PageModel
    {
        private readonly IRepository _repository;

        public StudioRoom? Room { get; set; }

        public DetailsModel(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult OnGet(int id)
        {
            Room = _repository.GetStudioRoomById(id);
            if (Room == null) return RedirectToPage("Index");
            return Page();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Data;
using System.Collections.Generic;
using System.Linq;

namespace LS_Projekt_ASP_2026.Pages.Bookings
{
    public class CreateModel : PageModel
    {
        private readonly IRepository _repository;

        [BindProperty]
        public Booking NewBooking { get; set; } = new();

        [BindProperty]
        public string ProjectId { get; set; } = "";

        [BindProperty]
        public string ProjectType { get; set; } = "";

        [BindProperty]
        public bool IncludeCatering { get; set; }

        [BindProperty]
        public bool IncludeTechAssistant { get; set; }

        [BindProperty]
        public string AdditionalNotes { get; set; } = "";

        // Dostupni izbori za forme
        public List<StudioRoom> AvailableStudios { get; set; } = new();
        public List<Producer> AvailableProducers { get; set; } = new();
        public List<AudioProject> AvailableProjects { get; set; } = new();
        public List<Client> AvailableClients { get; set; } = new();

        public CreateModel(IRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            // Učitaj sve dostupne izbore za forme
            AvailableStudios = new List<StudioRoom>(_repository.GetAllStudioRooms());
            AvailableProducers = new List<Producer>(_repository.GetAllProducers());
            AvailableProjects = new List<AudioProject>(_repository.GetAllProjects());
            AvailableClients = new List<Client>(_repository.GetAllClients());
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                OnGet(); // Reload paddown liste
                return Page();
            }

            // Dodaj novu rezervaciju
            _repository.AddBooking(NewBooking);

            // Preusmjeri na Index sa porukom
            TempData["Message"] = "Rezervacija je uspješno kreirana!";
            return RedirectToPage("Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Data;
using System.Collections.Generic;

namespace LS_Projekt_ASP_2026.Pages.Bookings
{
    public class CreateModel : PageModel
    {
        private readonly IRepository _repository;

        [BindProperty]
        public Booking Input { get; set; } = new();

        public string SelectedClientLabel { get; set; } = string.Empty;
        public string SelectedProducerLabel { get; set; } = string.Empty;

        public List<StudioRoom> AvailableStudios { get; set; } = new();
        public List<Producer> AvailableProducers { get; set; } = new();
        public List<Client> AvailableClients { get; set; } = new();

        public CreateModel(IRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            // Postaviti default vremenske vrijednosti
            if (Input.StartTime == DateTime.MinValue)
            {
                Input.StartTime = DateTime.Now;
            }
            if (Input.EndTime == DateTime.MinValue)
            {
                Input.EndTime = DateTime.Now.AddHours(1);
            }
            LoadLookups();
            LoadSelectedLabels();
        }

        public IActionResult OnPost()
        {
            AutocompleteValidation.ValidateClientSelection(ModelState, _repository, Input.ClientId);
            AutocompleteValidation.ValidateProducerSelection(ModelState, _repository, Input.ProducerId);
            AutocompleteValidation.ValidateStudioSelection(ModelState, _repository, Input.StudioRoomId);

            if (!ModelState.IsValid)
            {
                LoadLookups();
                LoadSelectedLabels();
                return Page();
            }

            Input.CreatedAt = DateTime.Now;
            _repository.AddBooking(Input);
            TempData["Message"] = "Rezervacija je kreirana.";
            return RedirectToPage("Index");
        }

        private void LoadLookups()
        {
            AvailableStudios = new List<StudioRoom>(_repository.GetAllStudioRooms());
            AvailableProducers = new List<Producer>(_repository.GetAllProducers());
            AvailableClients = new List<Client>(_repository.GetAllClients());
        }

        private void LoadSelectedLabels()
        {
            SelectedClientLabel = Input.ClientId > 0
                ? $"{_repository.GetClientById(Input.ClientId)?.Name} {_repository.GetClientById(Input.ClientId)?.Surname}".Trim()
                : string.Empty;

            SelectedProducerLabel = Input.ProducerId > 0
                ? $"{_repository.GetProducerById(Input.ProducerId)?.Name} {_repository.GetProducerById(Input.ProducerId)?.Surname}".Trim()
                : string.Empty;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Data;
using System.Collections.Generic;

namespace LS_Projekt_ASP_2026.Pages.Bookings
{
    public class EditModel : PageModel
    {
        private readonly IRepository _repository;

        [BindProperty]
        public Booking Input { get; set; } = new();

        public string SelectedClientLabel { get; set; } = string.Empty;
        public string SelectedProducerLabel { get; set; } = string.Empty;

        public List<StudioRoom> AvailableStudios { get; set; } = new();
        public List<Producer> AvailableProducers { get; set; } = new();
        public List<Client> AvailableClients { get; set; } = new();

        public EditModel(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult OnGet(int id)
        {
            var booking = _repository.GetBookingById(id);
            if (booking == null) return RedirectToPage("Index");
            Input = booking;
            LoadLookups();
            LoadSelectedLabels();
            return Page();
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

            var existing = _repository.GetBookingById(Input.Id);
            if (existing == null) return RedirectToPage("Index");

            existing.StartTime = Input.StartTime;
            existing.EndTime = Input.EndTime;
            existing.Status = Input.Status;
            existing.Purpose = Input.Purpose;
            existing.TotalPrice = Input.TotalPrice;
            existing.RequiresEngineer = Input.RequiresEngineer;
            existing.AdditionalNotes = Input.AdditionalNotes;
            existing.ClientId = Input.ClientId;
            existing.ProducerId = Input.ProducerId;
            existing.StudioRoomId = Input.StudioRoomId;

            _repository.UpdateBooking(existing);
            TempData["Message"] = "Rezervacija je ažurirana.";
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

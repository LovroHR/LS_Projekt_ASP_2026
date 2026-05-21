using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;
using System.Collections.Generic;
using System.Linq;

namespace LS_Projekt_ASP_2026.Pages.Projects
{
    public class EditModel : PageModel
    {
        private readonly IRepository _repository;

        [BindProperty]
        public AudioProject Input { get; set; } = new();

        public string SelectedClientLabel { get; set; } = string.Empty;
        public string SelectedProducerLabel { get; set; } = string.Empty;

        public List<Client> AvailableClients { get; set; } = new();
        public List<Producer> AvailableProducers { get; set; } = new();
        public List<SelectListItem> AvailableStudios { get; set; } = new();

        public EditModel(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult OnGet(int id)
        {
            var project = _repository.GetProjectById(id);
            if (project == null) return RedirectToPage("Index");
            Input = project;
            LoadLookups();
            LoadSelectedLabels();
            return Page();
        }

        public IActionResult OnPost()
        {
            AutocompleteValidation.ValidateClientSelection(ModelState, _repository, Input.ClientId);
            AutocompleteValidation.ValidateProducerSelection(ModelState, _repository, Input.ProducerId);
            AutocompleteValidation.ValidateOptionalStudioSelection(ModelState, _repository, Input.StudioRoomId);

            if (!ModelState.IsValid)
            {
                LoadLookups();
                LoadSelectedLabels();
                return Page();
            }

            var existing = _repository.GetProjectById(Input.Id);
            if (existing == null) return RedirectToPage("Index");

            existing.Title = Input.Title;
            existing.Type = Input.Type;
            existing.Status = Input.Status;
            existing.Genre = Input.Genre;
            existing.TargetDurationSeconds = Input.TargetDurationSeconds;
            existing.Deadline = Input.Deadline;
            existing.Budget = Input.Budget;
            existing.AllowClientComments = Input.AllowClientComments;
            existing.SharedFolderUrl = Input.SharedFolderUrl;
            existing.ClientId = Input.ClientId;
            existing.ProducerId = Input.ProducerId;
            existing.StudioRoomId = Input.StudioRoomId;

            _repository.UpdateProject(existing);
            TempData["Message"] = "Projekt je ažuriran.";
            return RedirectToPage("Index");
        }

        private void LoadLookups()
        {
            AvailableClients = new List<Client>(_repository.GetAllClients());
            AvailableProducers = new List<Producer>(_repository.GetAllProducers());
            AvailableStudios = _repository.GetAllStudioRooms()
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                .ToList();
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

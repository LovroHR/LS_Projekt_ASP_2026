using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;
using System.Collections.Generic;

namespace LS_Projekt_ASP_2026.Pages.Projects
{
    public class CreateModel : PageModel
    {
        private readonly IRepository _repository;

        [BindProperty]
        public AudioProject Input { get; set; } = new();

        public string SelectedClientLabel { get; set; } = string.Empty;
        public string SelectedProducerLabel { get; set; } = string.Empty;

        public List<SelectListItem> AvailableClients { get; set; } = new();
        public List<SelectListItem> AvailableProducers { get; set; } = new();
        public List<SelectListItem> AvailableStudios { get; set; } = new();

        public CreateModel(IRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            // Postaviti default deadline (30 dana od sada)
            if (Input.Deadline == DateTime.MinValue)
            {
                Input.Deadline = DateTime.Now.AddDays(30);
            }
            AvailableClients = _repository.GetAllClients()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Name} {c.Surname}"
                })
                .ToList();

            AvailableProducers = _repository.GetAllProducers()
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.Name} {p.Surname}"
                })
                .ToList();

            AvailableStudios = _repository.GetAllStudioRooms()
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                })
                .ToList();

            LoadSelectedLabels();
        }

        public IActionResult OnPost()
        {
            AutocompleteValidation.ValidateClientSelection(ModelState, _repository, Input.ClientId);
            AutocompleteValidation.ValidateProducerSelection(ModelState, _repository, Input.ProducerId);
            AutocompleteValidation.ValidateOptionalStudioSelection(ModelState, _repository, Input.StudioRoomId);

            if (!ModelState.IsValid)
            {
                OnGet();
                return Page();
            }

            Input.CreatedAt = DateTime.Now;
            try
            {
                _repository.AddProject(Input);
                TempData["Message"] = "Projekt je kreiran.";
                return RedirectToPage("Index");
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Greška prilikom kreiranja projekta: " + ex.Message);
                OnGet();
                return Page();
            }
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

using AudioProductionManagement.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LS_Projekt_ASP_2026.Data;

public static class AutocompleteValidation
{
    public static readonly string[] Countries =
    {
        "Hrvatska",
        "Srbija",
        "Bosna i Hercegovina",
        "Slovenija",
        "Italija",
        "Njemačka",
        "Francuska",
        "Austrija",
        "Švicarska",
        "Mađarska",
        "Češka",
        "Ostalo"
    };

    public static bool IsValidCountry(string? country)
    {
        return !string.IsNullOrWhiteSpace(country) && Countries.Contains(country);
    }

    public static bool TryGetClientLabel(IRepository repository, int clientId, out string label)
    {
        var client = repository.GetClientById(clientId);
        if (client is null)
        {
            label = string.Empty;
            return false;
        }

        label = $"{client.Name} {client.Surname}".Trim();
        return true;
    }

    public static bool TryGetProducerLabel(IRepository repository, int producerId, out string label)
    {
        var producer = repository.GetProducerById(producerId);
        if (producer is null)
        {
            label = string.Empty;
            return false;
        }

        label = $"{producer.Name} {producer.Surname}".Trim();
        return true;
    }

    public static void ValidateClientSelection(ModelStateDictionary modelState, IRepository repository, int clientId, string key = "Input.ClientId")
    {
        if (clientId <= 0 || repository.GetClientById(clientId) is null)
        {
            modelState.AddModelError(key, "Odaberite klijenta iz liste.");
        }
    }

    public static void ValidateProducerSelection(ModelStateDictionary modelState, IRepository repository, int producerId, string key = "Input.ProducerId")
    {
        if (producerId <= 0 || repository.GetProducerById(producerId) is null)
        {
            modelState.AddModelError(key, "Odaberite producenta iz liste.");
        }
    }

    public static void ValidateStudioSelection(ModelStateDictionary modelState, IRepository repository, int studioRoomId, string key = "Input.StudioRoomId")
    {
        if (studioRoomId <= 0 || repository.GetStudioRoomById(studioRoomId) is null)
        {
            modelState.AddModelError(key, "Odaberite studio iz liste.");
        }
    }

    public static void ValidateOptionalStudioSelection(ModelStateDictionary modelState, IRepository repository, int? studioRoomId, string key = "Input.StudioRoomId")
    {
        if (studioRoomId.HasValue && studioRoomId.Value > 0 && repository.GetStudioRoomById(studioRoomId.Value) is null)
        {
            modelState.AddModelError(key, "Odabrani studio ne postoji.");
        }
    }

    public static void ValidateCountrySelection(ModelStateDictionary modelState, string? country, string key = "Input.Country")
    {
        if (!IsValidCountry(country))
        {
            modelState.AddModelError(key, "Odaberite državu iz liste.");
        }
    }
}
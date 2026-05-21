using LS_Projekt_ASP_2026.Data;
using Microsoft.AspNetCore.Mvc;

namespace LS_Projekt_ASP_2026.Controllers;

[ApiController]
[Route("api/lookups")]
public class LookupController : ControllerBase
{

    private readonly IRepository _repository;

    public LookupController(IRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("clients")]
    public IActionResult SearchClients([FromQuery] string? q)
    {
        var query = q?.Trim();

        var results = _repository.GetAllClients()
            .Where(client => string.IsNullOrWhiteSpace(query)
                || (client.Name ?? string.Empty).Contains(query, StringComparison.OrdinalIgnoreCase)
                || (client.Surname ?? string.Empty).Contains(query, StringComparison.OrdinalIgnoreCase)
                || (client.Email ?? string.Empty).Contains(query, StringComparison.OrdinalIgnoreCase)
                || (client.CompanyName ?? string.Empty).Contains(query, StringComparison.OrdinalIgnoreCase))
            .OrderBy(client => client.Name)
            .ThenBy(client => client.Surname)
            .Take(10)
            .Select(client => new
            {
                id = client.Id,
                label = $"{client.Name} {client.Surname}".Trim(),
                secondary = client.Email
            })
            .ToList();

        return Ok(results);
    }

    [HttpGet("producers")]
    public IActionResult SearchProducers([FromQuery] string? q)
    {
        var query = q?.Trim();

        var results = _repository.GetAllProducers()
            .Where(producer => string.IsNullOrWhiteSpace(query)
                || (producer.Name ?? string.Empty).Contains(query, StringComparison.OrdinalIgnoreCase)
                || (producer.Surname ?? string.Empty).Contains(query, StringComparison.OrdinalIgnoreCase)
                || (producer.Email ?? string.Empty).Contains(query, StringComparison.OrdinalIgnoreCase)
                || (producer.Specialization ?? string.Empty).Contains(query, StringComparison.OrdinalIgnoreCase))
            .OrderBy(producer => producer.Name)
            .ThenBy(producer => producer.Surname)
            .Take(10)
            .Select(producer => new
            {
                id = producer.Id,
                label = $"{producer.Name} {producer.Surname}".Trim(),
                secondary = producer.Email
            })
            .ToList();

        return Ok(results);
    }

    [HttpGet("countries")]
    public IActionResult SearchCountries([FromQuery] string? q)
    {
        var query = q?.Trim();

        var results = AutocompleteValidation.Countries
            .Where(country => string.IsNullOrWhiteSpace(query)
                || country.Contains(query, StringComparison.OrdinalIgnoreCase))
            .Take(10)
            .Select(country => new
            {
                value = country,
                label = country
            })
            .ToList();

        return Ok(results);
    }
}
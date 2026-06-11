using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Api;
using LS_Projekt_ASP_2026.Data;
using LS_Projekt_ASP_2026.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LS_Projekt_ASP_2026.Controllers;

[ApiController]
[Route("api/v1/clients")]
[Authorize(Roles = "Producer,Admin")]
public class ApiClientsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ApiClientsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiListResponse<ClientDto>>> GetAll([FromQuery] string? q, [FromQuery] string? country, [FromQuery] bool? priority)
    {
        var query = _context.Clients.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(c =>
                c.Name.Contains(term) ||
                c.Surname.Contains(term) ||
                c.Email.Contains(term) ||
                c.CompanyName.Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(country))
        {
            query = query.Where(c => c.Country == country.Trim());
        }

        if (priority.HasValue)
        {
            query = query.Where(c => c.IsPriorityClient == priority.Value);
        }

        var clients = await query.OrderBy(c => c.Name).ThenBy(c => c.Surname).ToListAsync();
        var data = clients.Select(c => c.ToDto()).ToList();
        return Ok(new ApiListResponse<ClientDto>(data.Count, data));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClientDto>> GetById(int id)
    {
        var client = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        return client == null ? NotFound() : Ok(client.ToDto());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ClientDto>> Create(ClientCreateDto dto)
    {
        if (await EmailExists(dto.Email))
        {
            return Conflict(new { message = "Email je vec zauzet." });
        }

        var client = new Client
        {
            Name = dto.Name.Trim(),
            Surname = dto.Surname.Trim(),
            Email = dto.Email.Trim(),
            PhoneNumber = dto.PhoneNumber.Trim(),
            Password = PasswordHasher.Hash(string.IsNullOrWhiteSpace(dto.Password) ? "password123" : dto.Password),
            CreatedAt = DateTime.Now,
            Role = UserRole.Client,
            DateOfBirth = dto.DateOfBirth,
            Address = dto.Address.Trim(),
            Country = dto.Country.Trim(),
            CompanyName = dto.CompanyName.Trim(),
            BillingAddress = dto.BillingAddress.Trim(),
            IsPriorityClient = dto.IsPriorityClient,
            Notes = dto.Notes.Trim()
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = client.Id }, client.ToDto());
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ClientDto>> Update(int id, ClientUpdateDto dto)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
        if (client == null)
        {
            return NotFound();
        }

        if (await EmailExists(dto.Email, id))
        {
            return Conflict(new { message = "Email je vec zauzet." });
        }

        client.Name = dto.Name.Trim();
        client.Surname = dto.Surname.Trim();
        client.Email = dto.Email.Trim();
        client.PhoneNumber = dto.PhoneNumber.Trim();
        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            client.Password = PasswordHasher.Hash(dto.Password);
        }
        client.DateOfBirth = dto.DateOfBirth;
        client.Address = dto.Address.Trim();
        client.Country = dto.Country.Trim();
        client.CompanyName = dto.CompanyName.Trim();
        client.BillingAddress = dto.BillingAddress.Trim();
        client.IsPriorityClient = dto.IsPriorityClient;
        client.Notes = dto.Notes.Trim();

        await _context.SaveChangesAsync();
        return Ok(client.ToDto());
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
        if (client == null)
        {
            return NotFound();
        }

        var hasDependencies = await _context.Bookings.AnyAsync(b => b.ClientId == id)
            || await _context.AudioProjects.AnyAsync(p => p.ClientId == id);
        if (hasDependencies)
        {
            return Conflict(new { message = "Klijent ima povezane rezervacije ili projekte." });
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private Task<bool> EmailExists(string email, int? exceptUserId = null)
    {
        var normalized = email.Trim().ToLower();
        return _context.BusinessUsers.AnyAsync(u => u.Email.ToLower() == normalized && (!exceptUserId.HasValue || u.Id != exceptUserId.Value));
    }
}

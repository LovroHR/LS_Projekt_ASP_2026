using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Api;
using LS_Projekt_ASP_2026.Data;
using LS_Projekt_ASP_2026.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LS_Projekt_ASP_2026.Controllers;

[ApiController]
[Route("api/v1/producers")]
[Authorize(Roles = "Admin")]
public class ApiProducersController : ControllerBase
{
    private readonly AppDbContext _context;

    public ApiProducersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiListResponse<ProducerDto>>> GetAll([FromQuery] string? q, [FromQuery] string? specialization, [FromQuery] bool? external)
    {
        var query = _context.Producers.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(p => p.Name.Contains(term) || p.Surname.Contains(term) || p.Email.Contains(term) || p.Specialization.Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(specialization))
        {
            var spec = specialization.Trim();
            query = query.Where(p => p.Specialization.Contains(spec));
        }

        if (external.HasValue)
        {
            query = query.Where(p => p.IsExternalCollaborator == external.Value);
        }

        var producers = await query.OrderBy(p => p.Name).ThenBy(p => p.Surname).ToListAsync();
        var data = producers.Select(p => p.ToDto()).ToList();
        return Ok(new ApiListResponse<ProducerDto>(data.Count, data));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProducerDto>> GetById(int id)
    {
        var producer = await _context.Producers.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        return producer == null ? NotFound() : Ok(producer.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<ProducerDto>> Create(ProducerCreateDto dto)
    {
        if (await EmailExists(dto.Email))
        {
            return Conflict(new { message = "Email je vec zauzet." });
        }

        var producer = new Producer
        {
            Name = dto.Name.Trim(),
            Surname = dto.Surname.Trim(),
            Email = dto.Email.Trim(),
            PhoneNumber = dto.PhoneNumber.Trim(),
            Password = PasswordHasher.Hash(string.IsNullOrWhiteSpace(dto.Password) ? "password123" : dto.Password),
            CreatedAt = DateTime.Now,
            Role = UserRole.Producer,
            Specialization = dto.Specialization.Trim(),
            HourlyRate = dto.HourlyRate,
            IsExternalCollaborator = dto.IsExternalCollaborator,
            Biography = dto.Biography.Trim()
        };

        _context.Producers.Add(producer);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = producer.Id }, producer.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProducerDto>> Update(int id, ProducerUpdateDto dto)
    {
        var producer = await _context.Producers.FirstOrDefaultAsync(p => p.Id == id);
        if (producer == null)
        {
            return NotFound();
        }

        if (await EmailExists(dto.Email, id))
        {
            return Conflict(new { message = "Email je vec zauzet." });
        }

        producer.Name = dto.Name.Trim();
        producer.Surname = dto.Surname.Trim();
        producer.Email = dto.Email.Trim();
        producer.PhoneNumber = dto.PhoneNumber.Trim();
        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            producer.Password = PasswordHasher.Hash(dto.Password);
        }
        producer.Specialization = dto.Specialization.Trim();
        producer.HourlyRate = dto.HourlyRate;
        producer.IsExternalCollaborator = dto.IsExternalCollaborator;
        producer.Biography = dto.Biography.Trim();

        await _context.SaveChangesAsync();
        return Ok(producer.ToDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var producer = await _context.Producers.FirstOrDefaultAsync(p => p.Id == id);
        if (producer == null)
        {
            return NotFound();
        }

        var hasDependencies = await _context.Bookings.AnyAsync(b => b.ProducerId == id)
            || await _context.AudioProjects.AnyAsync(p => p.ProducerId == id);
        if (hasDependencies)
        {
            return Conflict(new { message = "Producent ima povezane rezervacije ili projekte." });
        }

        _context.Producers.Remove(producer);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private Task<bool> EmailExists(string email, int? exceptUserId = null)
    {
        var normalized = email.Trim().ToLower();
        return _context.BusinessUsers.AnyAsync(u => u.Email.ToLower() == normalized && (!exceptUserId.HasValue || u.Id != exceptUserId.Value));
    }
}

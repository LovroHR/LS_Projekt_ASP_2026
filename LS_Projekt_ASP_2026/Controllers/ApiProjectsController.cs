using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Api;
using LS_Projekt_ASP_2026.Data;
using LS_Projekt_ASP_2026.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LS_Projekt_ASP_2026.Controllers;

[ApiController]
[Route("api/v1/projects")]
[Authorize(Roles = "Client,Producer,Admin")]
public class ApiProjectsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ApiProjectsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiListResponse<AudioProjectDto>>> GetAll(
        [FromQuery] string? q,
        [FromQuery] ProjectStatus? status,
        [FromQuery] ProjectType? type,
        [FromQuery] string? genre,
        [FromQuery] int? clientId,
        [FromQuery] int? producerId,
        [FromQuery] decimal? minBudget,
        [FromQuery] decimal? maxBudget)
    {
        var query = ApplyUserScope(IncludeAll(_context.AudioProjects.AsNoTracking()));

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(p =>
                p.Title.Contains(term) ||
                p.Genre.Contains(term) ||
                p.Client.Name.Contains(term) ||
                p.Client.Surname.Contains(term) ||
                p.Producer.Name.Contains(term) ||
                p.Producer.Surname.Contains(term));
        }

        if (status.HasValue) query = query.Where(p => p.Status == status.Value);
        if (type.HasValue) query = query.Where(p => p.Type == type.Value);
        if (!string.IsNullOrWhiteSpace(genre)) query = query.Where(p => p.Genre.Contains(genre.Trim()));
        if (clientId.HasValue) query = query.Where(p => p.ClientId == clientId.Value);
        if (producerId.HasValue) query = query.Where(p => p.ProducerId == producerId.Value);
        if (minBudget.HasValue) query = query.Where(p => p.Budget >= minBudget.Value);
        if (maxBudget.HasValue) query = query.Where(p => p.Budget <= maxBudget.Value);

        var projects = await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        var data = projects.Select(p => p.ToDto()).ToList();
        return Ok(new ApiListResponse<AudioProjectDto>(data.Count, data));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AudioProjectDto>> GetById(int id)
    {
        var project = await ApplyUserScope(IncludeAll(_context.AudioProjects.AsNoTracking()))
            .FirstOrDefaultAsync(p => p.Id == id);
        return project == null ? NotFound() : Ok(project.ToDto());
    }

    [HttpPost]
    [Authorize(Roles = "Producer,Admin")]
    public async Task<ActionResult<AudioProjectDto>> Create(AudioProjectCreateDto dto)
    {
        var validation = await ValidateReferences(dto.ClientId, dto.ProducerId, dto.StudioRoomId);
        if (validation != null) return validation;

        var project = new AudioProject
        {
            Title = dto.Title.Trim(),
            Type = dto.Type,
            Status = dto.Status,
            Genre = dto.Genre.Trim(),
            TargetDurationSeconds = dto.TargetDurationSeconds,
            CreatedAt = DateTime.Now,
            Deadline = dto.Deadline,
            Budget = dto.Budget,
            AllowClientComments = dto.AllowClientComments,
            SharedFolderUrl = dto.SharedFolderUrl.Trim(),
            ClientId = dto.ClientId,
            ProducerId = dto.ProducerId,
            StudioRoomId = dto.StudioRoomId
        };

        _context.AudioProjects.Add(project);
        await _context.SaveChangesAsync();

        var created = await IncludeAll(_context.AudioProjects.AsNoTracking()).FirstAsync(p => p.Id == project.Id);
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, created.ToDto());
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Producer,Admin")]
    public async Task<ActionResult<AudioProjectDto>> Update(int id, AudioProjectUpdateDto dto)
    {
        var project = await _context.AudioProjects.FirstOrDefaultAsync(p => p.Id == id);
        if (project == null)
        {
            return NotFound();
        }

        var validation = await ValidateReferences(dto.ClientId, dto.ProducerId, dto.StudioRoomId);
        if (validation != null) return validation;

        project.Title = dto.Title.Trim();
        project.Type = dto.Type;
        project.Status = dto.Status;
        project.Genre = dto.Genre.Trim();
        project.TargetDurationSeconds = dto.TargetDurationSeconds;
        project.Deadline = dto.Deadline;
        project.Budget = dto.Budget;
        project.AllowClientComments = dto.AllowClientComments;
        project.SharedFolderUrl = dto.SharedFolderUrl.Trim();
        project.ClientId = dto.ClientId;
        project.ProducerId = dto.ProducerId;
        project.StudioRoomId = dto.StudioRoomId;

        await _context.SaveChangesAsync();
        var updated = await IncludeAll(_context.AudioProjects.AsNoTracking()).FirstAsync(p => p.Id == id);
        return Ok(updated.ToDto());
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Producer,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _context.AudioProjects.FirstOrDefaultAsync(p => p.Id == id);
        if (project == null)
        {
            return NotFound();
        }

        _context.AudioProjects.Remove(project);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private IQueryable<AudioProject> IncludeAll(IQueryable<AudioProject> query)
    {
        return query
            .Include(p => p.Client)
            .Include(p => p.Producer)
            .Include(p => p.StudioRoom)
            .Include(p => p.Versions)
                .ThenInclude(v => v.Comments)
                    .ThenInclude(c => c.Author);
    }

    private IQueryable<AudioProject> ApplyUserScope(IQueryable<AudioProject> query)
    {
        var userId = User.GetUserId();
        if (User.IsInRole("Admin") || userId is null)
        {
            return query;
        }

        return User.IsInRole("Producer")
            ? query.Where(p => p.ProducerId == userId.Value)
            : query.Where(p => p.ClientId == userId.Value);
    }

    private async Task<ActionResult?> ValidateReferences(int clientId, int producerId, int? studioRoomId)
    {
        if (!await _context.Clients.AnyAsync(c => c.Id == clientId)) return BadRequest(new { message = "Klijent ne postoji." });
        if (!await _context.Producers.AnyAsync(p => p.Id == producerId)) return BadRequest(new { message = "Producent ne postoji." });
        if (studioRoomId.HasValue && !await _context.StudioRooms.AnyAsync(s => s.Id == studioRoomId.Value)) return BadRequest(new { message = "Studio ne postoji." });
        return null;
    }
}

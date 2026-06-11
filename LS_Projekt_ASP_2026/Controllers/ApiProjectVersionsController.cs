using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Api;
using LS_Projekt_ASP_2026.Data;
using LS_Projekt_ASP_2026.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LS_Projekt_ASP_2026.Controllers;

[ApiController]
[Route("api/v1/project-versions")]
[Authorize(Roles = "Client,Producer,Admin")]
public class ApiProjectVersionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ApiProjectVersionsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiListResponse<ProjectVersionDto>>> GetAll([FromQuery] string? q, [FromQuery] int? projectId, [FromQuery] bool? approved)
    {
        var query = ApplyUserScope(IncludeAll(_context.ProjectVersions.AsNoTracking()));

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(v => v.Name.Contains(term) || v.Description.Contains(term) || v.Notes.Contains(term));
        }

        if (projectId.HasValue)
        {
            query = query.Where(v => v.ProjectId == projectId.Value);
        }

        if (approved.HasValue)
        {
            query = query.Where(v => v.IsApproved == approved.Value);
        }

        var versions = await query.OrderByDescending(v => v.CreatedAt).ToListAsync();
        var data = versions.Select(v => v.ToDto()).ToList();
        return Ok(new ApiListResponse<ProjectVersionDto>(data.Count, data));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectVersionDto>> GetById(int id)
    {
        var version = await ApplyUserScope(IncludeAll(_context.ProjectVersions.AsNoTracking())).FirstOrDefaultAsync(v => v.Id == id);
        return version == null ? NotFound() : Ok(version.ToDto());
    }

    [HttpPost]
    [Authorize(Roles = "Producer,Admin")]
    public async Task<ActionResult<ProjectVersionDto>> Create(ProjectVersionCreateDto dto)
    {
        var project = await _context.AudioProjects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == dto.ProjectId);
        if (project == null)
        {
            return BadRequest(new { message = "Projekt ne postoji." });
        }

        if (!CanManageProject(project))
        {
            return Forbid();
        }

        var nextVersionNumber = await _context.ProjectVersions
            .Where(v => v.ProjectId == dto.ProjectId)
            .Select(v => (int?)v.VersionNumber)
            .MaxAsync() ?? 0;

        var version = new ProjectVersion
        {
            ProjectId = dto.ProjectId,
            VersionNumber = nextVersionNumber + 1,
            Name = dto.Name.Trim(),
            Description = dto.Description.Trim(),
            CreatedAt = DateTime.Now,
            DurationSeconds = dto.DurationSeconds,
            FileSize = dto.FileSize,
            FileUrl = dto.FileUrl.Trim(),
            Notes = dto.Notes.Trim(),
            IsApproved = dto.IsApproved
        };

        _context.ProjectVersions.Add(version);
        await _context.SaveChangesAsync();

        var created = await IncludeAll(_context.ProjectVersions.AsNoTracking()).FirstAsync(v => v.Id == version.Id);
        return CreatedAtAction(nameof(GetById), new { id = version.Id }, created.ToDto());
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Producer,Admin")]
    public async Task<ActionResult<ProjectVersionDto>> Update(int id, ProjectVersionUpdateDto dto)
    {
        var version = await _context.ProjectVersions
            .Include(v => v.Project)
            .FirstOrDefaultAsync(v => v.Id == id);
        if (version == null)
        {
            return NotFound();
        }

        if (!CanManageProject(version.Project))
        {
            return Forbid();
        }

        version.Name = dto.Name.Trim();
        version.Description = dto.Description.Trim();
        version.UpdatedAt = DateTime.Now;
        version.DurationSeconds = dto.DurationSeconds;
        version.FileSize = dto.FileSize;
        version.FileUrl = dto.FileUrl.Trim();
        version.Notes = dto.Notes.Trim();
        version.IsApproved = dto.IsApproved;

        await _context.SaveChangesAsync();
        var updated = await IncludeAll(_context.ProjectVersions.AsNoTracking()).FirstAsync(v => v.Id == id);
        return Ok(updated.ToDto());
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Producer,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var version = await _context.ProjectVersions
            .Include(v => v.Project)
            .FirstOrDefaultAsync(v => v.Id == id);
        if (version == null)
        {
            return NotFound();
        }

        if (!CanManageProject(version.Project))
        {
            return Forbid();
        }

        _context.ProjectVersions.Remove(version);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private IQueryable<ProjectVersion> IncludeAll(IQueryable<ProjectVersion> query)
    {
        return query
            .Include(v => v.Project)
            .Include(v => v.Comments)
                .ThenInclude(c => c.Author);
    }

    private IQueryable<ProjectVersion> ApplyUserScope(IQueryable<ProjectVersion> query)
    {
        var userId = User.GetUserId();
        if (User.IsInRole("Admin") || userId is null)
        {
            return query;
        }

        return User.IsInRole("Producer")
            ? query.Where(v => v.Project.ProducerId == userId.Value)
            : query.Where(v => v.Project.ClientId == userId.Value);
    }

    private bool CanManageProject(AudioProject project)
    {
        var userId = User.GetUserId();
        return User.IsInRole("Admin") ||
            (User.IsInRole("Producer") && userId == project.ProducerId);
    }
}

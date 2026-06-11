using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Api;
using LS_Projekt_ASP_2026.Data;
using LS_Projekt_ASP_2026.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LS_Projekt_ASP_2026.Controllers;

[ApiController]
[Route("api/v1/comments")]
[Authorize(Roles = "Client,Producer,Admin")]
public class ApiCommentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ApiCommentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiListResponse<TimecodedCommentDto>>> GetAll(
        [FromQuery] string? q,
        [FromQuery] int? projectVersionId,
        [FromQuery] int? authorId,
        [FromQuery] bool? resolved,
        [FromQuery] bool? internalNote)
    {
        var query = ApplyUserScope(IncludeAll(_context.TimecodedComments.AsNoTracking()));

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(c => c.Message.Contains(term) || c.Category.Contains(term) || c.Author.Name.Contains(term) || c.Author.Surname.Contains(term));
        }

        if (projectVersionId.HasValue) query = query.Where(c => c.ProjectVersionId == projectVersionId.Value);
        if (authorId.HasValue) query = query.Where(c => c.AuthorId == authorId.Value);
        if (resolved.HasValue) query = query.Where(c => c.IsResolved == resolved.Value);
        if (internalNote.HasValue) query = query.Where(c => c.IsInternalNote == internalNote.Value);

        var comments = await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
        var data = comments.Select(c => c.ToDto()).ToList();
        return Ok(new ApiListResponse<TimecodedCommentDto>(data.Count, data));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TimecodedCommentDto>> GetById(int id)
    {
        var comment = await ApplyUserScope(IncludeAll(_context.TimecodedComments.AsNoTracking())).FirstOrDefaultAsync(c => c.Id == id);
        return comment == null ? NotFound() : Ok(comment.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<TimecodedCommentDto>> Create(TimecodedCommentCreateDto dto)
    {
        var authorId = User.IsInRole("Admin") ? dto.AuthorId : User.GetUserId();
        if (authorId is null)
        {
            return Forbid();
        }

        var validation = await ValidateReferences(dto.ProjectVersionId, authorId.Value);
        if (validation != null) return validation;

        var version = await _context.ProjectVersions
            .Include(v => v.Project)
            .FirstAsync(v => v.Id == dto.ProjectVersionId);
        if (!CanAccessProject(version.Project))
        {
            return Forbid();
        }

        var comment = new TimecodedComment
        {
            TimestampSeconds = dto.TimestampSeconds,
            Message = dto.Message.Trim(),
            CreatedAt = DateTime.Now,
            IsResolved = dto.IsResolved,
            Category = dto.Category.Trim(),
            IsInternalNote = dto.IsInternalNote,
            ProjectVersionId = dto.ProjectVersionId,
            AuthorId = authorId.Value
        };

        _context.TimecodedComments.Add(comment);
        await _context.SaveChangesAsync();

        var created = await IncludeAll(_context.TimecodedComments.AsNoTracking()).FirstAsync(c => c.Id == comment.Id);
        return CreatedAtAction(nameof(GetById), new { id = comment.Id }, created.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TimecodedCommentDto>> Update(int id, TimecodedCommentUpdateDto dto)
    {
        var comment = await _context.TimecodedComments
            .Include(c => c.ProjectVersion)
                .ThenInclude(v => v.Project)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (comment == null)
        {
            return NotFound();
        }

        if (!CanEditComment(comment))
        {
            return Forbid();
        }

        var authorId = User.IsInRole("Admin") ? dto.AuthorId : comment.AuthorId;
        var validation = await ValidateReferences(dto.ProjectVersionId, authorId);
        if (validation != null) return validation;

        var targetVersion = await _context.ProjectVersions
            .Include(v => v.Project)
            .FirstAsync(v => v.Id == dto.ProjectVersionId);
        if (!CanAccessProject(targetVersion.Project))
        {
            return Forbid();
        }

        comment.TimestampSeconds = dto.TimestampSeconds;
        comment.Message = dto.Message.Trim();
        comment.IsResolved = dto.IsResolved;
        comment.Category = dto.Category.Trim();
        comment.IsInternalNote = dto.IsInternalNote;
        comment.ProjectVersionId = dto.ProjectVersionId;
        comment.AuthorId = authorId;

        await _context.SaveChangesAsync();
        var updated = await IncludeAll(_context.TimecodedComments.AsNoTracking()).FirstAsync(c => c.Id == id);
        return Ok(updated.ToDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var comment = await _context.TimecodedComments
            .Include(c => c.ProjectVersion)
                .ThenInclude(v => v.Project)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (comment == null)
        {
            return NotFound();
        }

        if (!CanEditComment(comment))
        {
            return Forbid();
        }

        _context.TimecodedComments.Remove(comment);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private IQueryable<TimecodedComment> IncludeAll(IQueryable<TimecodedComment> query)
    {
        return query
            .Include(c => c.Author)
            .Include(c => c.ProjectVersion)
                .ThenInclude(v => v.Project);
    }

    private async Task<ActionResult?> ValidateReferences(int projectVersionId, int authorId)
    {
        if (!await _context.ProjectVersions.AnyAsync(v => v.Id == projectVersionId)) return BadRequest(new { message = "Verzija projekta ne postoji." });
        if (!await _context.BusinessUsers.AnyAsync(u => u.Id == authorId)) return BadRequest(new { message = "Autor ne postoji." });
        return null;
    }

    private IQueryable<TimecodedComment> ApplyUserScope(IQueryable<TimecodedComment> query)
    {
        var userId = User.GetUserId();
        if (User.IsInRole("Admin") || userId is null)
        {
            return query;
        }

        if (User.IsInRole("Producer"))
        {
            return query.Where(c => c.ProjectVersion.Project.ProducerId == userId.Value);
        }

        return query.Where(c => c.ProjectVersion.Project.ClientId == userId.Value && !c.IsInternalNote);
    }

    private bool CanAccessProject(AudioProject project)
    {
        var userId = User.GetUserId();
        return User.IsInRole("Admin") ||
            (User.IsInRole("Producer") && userId == project.ProducerId) ||
            (User.IsInRole("Client") && userId == project.ClientId);
    }

    private bool CanEditComment(TimecodedComment comment)
    {
        var userId = User.GetUserId();
        return User.IsInRole("Admin") ||
            (User.IsInRole("Producer") && userId == comment.ProjectVersion.Project.ProducerId) ||
            userId == comment.AuthorId;
    }
}

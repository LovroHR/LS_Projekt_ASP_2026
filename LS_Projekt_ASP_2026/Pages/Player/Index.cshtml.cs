using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LS_Projekt_ASP_2026.Pages.Player
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public ProjectVersion? Version { get; set; }

        public async Task<IActionResult> OnGetAsync(int? versionId)
        {
            if (versionId == null)
            {
                return Page();
            }

            Version = await _context.ProjectVersions
                .AsNoTracking()
                .Include(x => x.Project)
                .Include(x => x.Comments)
                    .ThenInclude(x => x.Author)
                .FirstOrDefaultAsync(x => x.Id == versionId.Value);

            if (Version == null)
            {
                return RedirectToPage("/Projects/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCommentAsync([FromBody] AddCommentRequest request)
        {
            if (request.ProjectVersionId <= 0 ||
                string.IsNullOrWhiteSpace(request.Message) ||
                request.TimestampSeconds < 0)
            {
                return BadRequest();
            }

            var versionExists = await _context.ProjectVersions.AnyAsync(x => x.Id == request.ProjectVersionId);
            if (!versionExists)
            {
                return NotFound();
            }

            var authorId = GetCurrentUserId();
            var comment = new TimecodedComment
            {
                ProjectVersionId = request.ProjectVersionId,
                TimestampSeconds = request.TimestampSeconds,
                Message = request.Message.Trim(),
                CreatedAt = DateTime.Now,
                AuthorId = authorId,
                Category = "",
                IsInternalNote = false,
                IsResolved = false
            };

            _context.TimecodedComments.Add(comment);
            await _context.SaveChangesAsync();

            var author = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == authorId);

            return new JsonResult(new CommentResponse
            {
                Id = comment.Id,
                Time = (double)comment.TimestampSeconds,
                Text = comment.Message,
                Author = author == null ? "Korisnik" : $"{author.Name} {author.Surname}",
                Timestamp = comment.CreatedAt.ToString("dd.MM.yyyy. HH:mm")
            });
        }

        private int GetCurrentUserId()
        {
            var sessionValue = HttpContext.Session.GetString("UserId");
            if (int.TryParse(sessionValue, out var userId) && _context.Users.Any(x => x.Id == userId))
            {
                return userId;
            }

            return _context.Users.OrderBy(x => x.Id).Select(x => x.Id).First();
        }

        public class AddCommentRequest
        {
            public int ProjectVersionId { get; set; }
            public decimal TimestampSeconds { get; set; }
            public string Message { get; set; } = "";
        }

        public class CommentResponse
        {
            public int Id { get; set; }
            public double Time { get; set; }
            public string Text { get; set; } = "";
            public string Author { get; set; } = "";
            public string Timestamp { get; set; } = "";
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace LS_Projekt_ASP_2026.Pages.Projects
{
    public class DetailsModel : PageModel
    {
        private readonly IRepository _repository;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AudioProject? Project { get; set; }

        [BindProperty]
        public IFormFile? AudioFile { get; set; }

        [BindProperty]
        public string VersionName { get; set; } = "";

        [TempData]
        public string? StatusMessage { get; set; }

        public DetailsModel(IRepository repository, AppDbContext context, IWebHostEnvironment environment)
        {
            _repository = repository;
            _context = context;
            _environment = environment;
        }

        public IActionResult OnGet(int id)
        {
            Project = _repository.GetProjectById(id);
            if (Project == null) return RedirectToPage("Index");
            return Page();
        }

        public async Task<IActionResult> OnPostUploadVersionAsync(int id)
        {
            Project = await _context.AudioProjects
                .Include(x => x.Versions)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (Project == null) return RedirectToPage("Index");

            if (AudioFile == null || AudioFile.Length == 0)
            {
                ModelState.AddModelError(nameof(AudioFile), "Odaberi audio datoteku za upload.");
                return Page();
            }

            if (!AudioFile.ContentType.StartsWith("audio/", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(nameof(AudioFile), "Datoteka mora biti audio format.");
                return Page();
            }

            var uploadsRoot = Path.Combine(_environment.WebRootPath, "uploads", "project-versions", id.ToString());
            Directory.CreateDirectory(uploadsRoot);

            var extension = Path.GetExtension(AudioFile.FileName);
            var safeBaseName = Path.GetFileNameWithoutExtension(AudioFile.FileName);
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                safeBaseName = safeBaseName.Replace(invalidChar, '-');
            }

            var storedFileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}{extension}";
            var absolutePath = Path.Combine(uploadsRoot, storedFileName);

            await using (var stream = System.IO.File.Create(absolutePath))
            {
                await AudioFile.CopyToAsync(stream);
            }

            var nextVersionNumber = Project.Versions.Any()
                ? Project.Versions.Max(x => x.VersionNumber) + 1
                : 1;

            var version = new ProjectVersion
            {
                ProjectId = Project.Id,
                VersionNumber = nextVersionNumber,
                Name = string.IsNullOrWhiteSpace(VersionName) ? safeBaseName : VersionName.Trim(),
                Description = "",
                CreatedAt = DateTime.Now,
                DurationSeconds = 0,
                FileSize = Math.Round((decimal)AudioFile.Length / 1024 / 1024, 2),
                FileUrl = $"/uploads/project-versions/{id}/{storedFileName}",
                Notes = "",
                IsApproved = false
            };

            _context.ProjectVersions.Add(version);
            await _context.SaveChangesAsync();

            StatusMessage = "Audio verzija je uspjesno uploadana.";
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostDeleteVersionAsync(int id, int versionId)
        {
            var version = await _context.ProjectVersions
                .FirstOrDefaultAsync(x => x.Id == versionId && x.ProjectId == id);

            if (version == null)
            {
                StatusMessage = "Audio verzija nije pronadena.";
                return RedirectToPage(new { id });
            }

            DeleteUploadedFileIfLocal(version.FileUrl);

            _context.ProjectVersions.Remove(version);
            await _context.SaveChangesAsync();

            StatusMessage = "Audio verzija je obrisana.";
            return RedirectToPage(new { id });
        }

        private void DeleteUploadedFileIfLocal(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl) ||
                !fileUrl.StartsWith("/uploads/project-versions/", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var relativePath = fileUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var absolutePath = Path.GetFullPath(Path.Combine(_environment.WebRootPath, relativePath));
            var uploadsRoot = Path.GetFullPath(Path.Combine(_environment.WebRootPath, "uploads", "project-versions"));

            if (!absolutePath.StartsWith(uploadsRoot, StringComparison.OrdinalIgnoreCase) ||
                !System.IO.File.Exists(absolutePath))
            {
                return;
            }

            System.IO.File.Delete(absolutePath);
        }
    }
}

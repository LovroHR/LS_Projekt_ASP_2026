using Microsoft.AspNetCore.Mvc;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;

namespace LS_Projekt_ASP_2026.Controllers
{
    /// <summary>
    /// Kontroler za upravljanje audio projektima sa custom routingom
    /// Demonstrira HTTP metode i kompleksnije routing scenarije
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(IRepository repository, ILogger<ProjectsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// AKCIJA 1: Route s verzijom API-ja - [Route("api/[controller]")] + GET
        /// Vraća sve projekte
        /// </summary>
        [HttpGet]
        public IActionResult GetAllProjects()
        {
            _logger.LogInformation("🎵 GetAllProjects - Dohvaća sve projekte");
            try
            {
                var projects = _repository.GetAllProjects();
                return Ok(new
                {
                    success = true,
                    message = "Svi projekti su dohvaćeni",
                    count = projects.Count(),
                    data = projects
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Greška pri dohvaćanju svih projekata");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// AKCIJA 2: Route s ID-om - GET api/projects/{id}
        /// Vraća specifičan projekt
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetProjectById(int id)
        {
            _logger.LogInformation($"🎵 GetProjectById - Dohvaća projekt ID: {id}");
            try
            {
                var project = _repository.GetProjectById(id);
                if (project == null)
                {
                    return NotFound(new { success = false, message = $"Projekt s ID {id} nije pronađen" });
                }

                return Ok(new
                {
                    success = true,
                    message = "Projekt je dohvaćen",
                    data = project
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Greška pri dohvaćanju projekta ID {id}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// AKCIJA 3: Custom route - GET projects/by-type/{type}
        /// Vraća projekte po tipu
        /// </summary>
        [HttpGet("by-type/{type}")]
        public IActionResult GetProjectsByType(string type)
        {
            _logger.LogInformation($"🎵 GetProjectsByType - Dohvaća projekte tipa: {type}");
            try
            {
                if (!Enum.TryParse<ProjectType>(type, ignoreCase: true, out var projectType))
                {
                    return BadRequest(new { success = false, message = $"Neispravan tip projekta: {type}" });
                }

                var projects = _repository.GetAllProjects()
                    .Where(p => p.Type == projectType)
                    .ToList();

                return Ok(new
                {
                    success = true,
                    message = $"Projekti tipa '{type}' su dohvaćeni",
                    count = projects.Count,
                    type = type,
                    data = projects
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Greška pri dohvaćanju projekata tipa {type}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// AKCIJA 4: Custom route s prefiksima - GET admin/projects/report
        /// Vraća detaljnu analizu projekata
        /// </summary>
        [HttpGet("admin/report")]
        public IActionResult GetProjectReport()
        {
            _logger.LogInformation("📊 GetProjectReport - Dohvaća izvještaj o projektima");
            try
            {
                var projects = _repository.GetAllProjects().ToList();

                var report = new
                {
                    success = true,
                    message = "Izvještaj o projektima je dohvaćen",
                    generatedAt = DateTime.Now,
                    summary = new
                    {
                        totalProjects = projects.Count,
                        totalBudget = projects.Sum(p => p.Budget),
                        averageBudget = projects.Count > 0 ? projects.Average(p => p.Budget) : 0,
                        byType = new
                        {
                            singles = projects.Count(p => p.Type == ProjectType.Single),
                            albums = projects.Count(p => p.Type == ProjectType.Album),
                            eps = projects.Count(p => p.Type == ProjectType.EP),
                            podcasts = projects.Count(p => p.Type == ProjectType.Podcast)
                        },
                        byStatus = new
                        {
                            draft = projects.Count(p => p.Status == ProjectStatus.Draft),
                            active = projects.Count(p => p.Status == ProjectStatus.Active),
                            waitingForFeedback = projects.Count(p => p.Status == ProjectStatus.WaitingForFeedback),
                            revision = projects.Count(p => p.Status == ProjectStatus.Revision),
                            approved = projects.Count(p => p.Status == ProjectStatus.Approved),
                            archived = projects.Count(p => p.Status == ProjectStatus.Archived)
                        }
                    },
                    details = projects
                };

                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Greška pri generiranju izvještaja");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// AKCIJA 5: Custom route s query parametrima - GET projects/advanced-search
        /// Traži projekte po kriterijima
        /// </summary>
        [HttpGet("advanced-search")]
        public IActionResult AdvancedSearch([FromQuery] string? genre, [FromQuery] string? status, [FromQuery] decimal? minBudget, [FromQuery] decimal? maxBudget)
        {
            _logger.LogInformation($"🔎 AdvancedSearch - Traži projekte: genre={genre}, status={status}, budget={minBudget}-{maxBudget}");
            try
            {
                var projects = _repository.GetAllProjects().AsEnumerable();

                if (!string.IsNullOrEmpty(genre))
                {
                    projects = projects.Where(p => p.Genre != null && p.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(status) && Enum.TryParse<ProjectStatus>(status, ignoreCase: true, out var parsedStatus))
                {
                    projects = projects.Where(p => p.Status == parsedStatus);
                }

                if (minBudget.HasValue)
                {
                    projects = projects.Where(p => p.Budget >= minBudget.Value);
                }

                if (maxBudget.HasValue)
                {
                    projects = projects.Where(p => p.Budget <= maxBudget.Value);
                }

                var results = projects.ToList();

                return Ok(new
                {
                    success = true,
                    message = "Pretraživanje je izvršeno",
                    count = results.Count,
                    filters = new { genre, status, minBudget, maxBudget },
                    data = results
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Greška pri naprednoj pretrazi");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// AKCIJA 6: Custom route - GET projects/client/{clientId}
        /// Vraća projekte određenog klijenta
        /// </summary>
        [HttpGet("client/{clientId}")]
        public IActionResult GetProjectsByClient(int clientId)
        {
            _logger.LogInformation($"👤 GetProjectsByClient - Dohvaća projekte klijenta ID: {clientId}");
            try
            {
                var projects = _repository.GetAllProjects()
                    .Where(p => p.ClientId == clientId)
                    .ToList();

                if (projects.Count == 0)
                {
                    return NotFound(new { success = false, message = $"Nema projekata za klijenta ID {clientId}" });
                }

                return Ok(new
                {
                    success = true,
                    message = $"Projekti klijenta ID {clientId} su dohvaćeni",
                    count = projects.Count,
                    clientId = clientId,
                    data = projects
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Greška pri dohvaćanju projekata klijenta ID {clientId}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// AKCIJA 7: Create project - POST api/projects
        /// </summary>
        [HttpPost]
        public IActionResult CreateProject([FromBody] AudioProject project)
        {
            _logger.LogInformation("🎵 CreateProject - Kreira novi projekt");
            try
            {
                if (project == null) return BadRequest(new { success = false, message = "Neispravan payload" });

                project.CreatedAt = DateTime.Now;
                _repository.AddProject(project);

                return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, new { success = true, message = "Projekt je kreiran", data = project });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Greška pri kreiranju projekta");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// AKCIJA 8: Update project - PUT api/projects/{id}
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult UpdateProject(int id, [FromBody] AudioProject project)
        {
            _logger.LogInformation($"🎵 UpdateProject - Ažurira projekt ID: {id}");
            try
            {
                if (project == null || id != project.Id) return BadRequest(new { success = false, message = "Neispravan payload ili ID" });

                var existing = _repository.GetProjectById(id);
                if (existing == null) return NotFound(new { success = false, message = $"Projekt ID {id} nije pronađen" });

                existing.Title = project.Title;
                existing.Type = project.Type;
                existing.Status = project.Status;
                existing.Genre = project.Genre;
                existing.TargetDurationSeconds = project.TargetDurationSeconds;
                existing.Deadline = project.Deadline;
                existing.Budget = project.Budget;
                existing.AllowClientComments = project.AllowClientComments;
                existing.SharedFolderUrl = project.SharedFolderUrl;
                existing.ClientId = project.ClientId;
                existing.ProducerId = project.ProducerId;

                _repository.UpdateProject(existing);

                return Ok(new { success = true, message = "Projekt je ažuriran", data = existing });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Greška pri ažuriranju projekta ID {id}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// AKCIJA 9: Delete project - DELETE api/projects/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteProject(int id)
        {
            _logger.LogInformation($"🎵 DeleteProject - Briše projekt ID: {id}");
            try
            {
                var existing = _repository.GetProjectById(id);
                if (existing == null) return NotFound(new { success = false, message = $"Projekt ID {id} nije pronađen" });

                _repository.DeleteProject(id);
                return Ok(new { success = true, message = "Projekt je obrisan" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Greška pri brisanju projekta ID {id}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}

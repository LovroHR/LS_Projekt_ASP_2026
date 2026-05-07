using Microsoft.AspNetCore.Mvc;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;

namespace LS_Projekt_ASP_2026.Controllers
{
    /// <summary>
    /// Kontroler za upravljanje rezervacijama sa custom routingom
    /// Demonstrira različite načine custom usmjeravanja u ASP.NET Core
    /// </summary>
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(IRepository repository, ILogger<BookingsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// AKCIJA 1: Custom route - /api/bookings/all
        /// Vraća sve rezervacije
        /// </summary>
        [HttpGet("api/bookings/all")]
        public IActionResult GetAllBookings()
        {
            _logger.LogInformation("🔍 GetAllBookings - Dohvata sve rezervacije");
            try
            {
                var bookings = _repository.GetAllBookings();
                return Ok(new
                {
                    success = true,
                    message = "Sve rezervacije su dohvaćene",
                    count = bookings.Count(),
                    data = bookings
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Greška pri dohvaćanju svih rezervacija");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// AKCIJA 2: Custom route s parametrom - /api/bookings/details/{id}
        /// Vraća detaljne informacije o specifičnoj rezervaciji
        /// </summary>
        [HttpGet("api/bookings/details/{id}")]
        public IActionResult GetBookingDetails(int id)
        {
            _logger.LogInformation($"🔍 GetBookingDetails - Dohvaća detalje rezervacije ID: {id}");
            try
            {
                var booking = _repository.GetBookingById(id);
                if (booking == null)
                {
                    _logger.LogWarning($"⚠️ Rezervacija s ID {id} nije pronađena");
                    return NotFound(new { success = false, message = $"Rezervacija s ID {id} nije pronađena" });
                }

                return Ok(new
                {
                    success = true,
                    message = "Detalji rezervacije su dohvaćeni",
                    data = booking
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Greška pri dohvaćanju detalja rezervacije ID {id}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// AKCIJA 3: Custom route s prefixom - /bookings-report/active
        /// Vraća samo aktivne i pending rezervacije
        /// </summary>
        [HttpGet("bookings-report/active")]
        public IActionResult GetActiveBookings()
        {
            _logger.LogInformation("📊 GetActiveBookings - Dohvaća aktivne rezervacije");
            try
            {
                var bookings = _repository.GetAllBookings()
                    .Where(b => b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.Pending || b.Status == BookingStatus.InProgress)
                    .ToList();

                return Ok(new
                {
                    success = true,
                    message = "Aktivne rezervacije su dohvaćene",
                    count = bookings.Count,
                    data = bookings
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Greška pri dohvaćanju aktivnih rezervacija");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// AKCIJA 4: Custom route s verzijom API-ja - /api/v2/bookings/statistics
        /// Vraća statistiku rezervacija
        /// </summary>
        [HttpGet("api/v2/bookings/statistics")]
        public IActionResult GetBookingStatistics()
        {
            _logger.LogInformation("📈 GetBookingStatistics - Dohvaća statistiku rezervacija");
            try
            {
                var allBookings = _repository.GetAllBookings().ToList();

                var stats = new
                {
                    success = true,
                    message = "Statistika rezervacija je dohvaćena",
                    totalBookings = allBookings.Count,
                    byStatus = new
                    {
                        confirmed = allBookings.Count(b => b.Status == BookingStatus.Confirmed),
                        pending = allBookings.Count(b => b.Status == BookingStatus.Pending),
                        inProgress = allBookings.Count(b => b.Status == BookingStatus.InProgress),
                        completed = allBookings.Count(b => b.Status == BookingStatus.Completed),
                        cancelled = allBookings.Count(b => b.Status == BookingStatus.Cancelled)
                    },
                    totalRevenue = allBookings.Sum(b => b.TotalPrice),
                    averagePrice = allBookings.Count > 0 ? allBookings.Average(b => b.TotalPrice) : 0
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Greška pri dohvaćanju statistike rezervacija");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// AKCIJA 5: Custom route s datumom - /bookings-by-date/{date:datetime}
        /// Vraća rezervacije za specifičan datum
        /// </summary>
        [HttpGet("bookings-by-date/{date:datetime}")]
        public IActionResult GetBookingsByDate(DateTime date)
        {
            _logger.LogInformation($"📅 GetBookingsByDate - Dohvaća rezervacije za {date:yyyy-MM-dd}");
            try
            {
                var bookings = _repository.GetAllBookings()
                    .Where(b => b.StartTime.Date == date.Date)
                    .ToList();

                return Ok(new
                {
                    success = true,
                    message = $"Rezervacije za {date:yyyy-MM-dd} su dohvaćene",
                    count = bookings.Count,
                    date = date.ToShortDateString(),
                    data = bookings
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Greška pri dohvaćanju rezervacija za {date:yyyy-MM-dd}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// AKCIJA 6: Custom route s query string - /search-bookings
        /// Traži rezervacije po statusu
        /// </summary>
        [HttpGet("search-bookings")]
        public IActionResult SearchBookings([FromQuery] string? status)
        {
            _logger.LogInformation($"🔎 SearchBookings - Traži rezervacije po statusu: {status}");
            try
            {
                var bookings = _repository.GetAllBookings().ToList();

                if (!string.IsNullOrEmpty(status))
                {
                    if (Enum.TryParse<BookingStatus>(status, ignoreCase: true, out var parsedStatus))
                    {
                        bookings = bookings.Where(b => b.Status == parsedStatus).ToList();
                    }
                    else
                    {
                        return BadRequest(new { success = false, message = $"Neispravan status: {status}" });
                    }
                }

                return Ok(new
                {
                    success = true,
                    message = "Pretraga rezervacija je izvršena",
                    count = bookings.Count,
                    filter = status ?? "sve",
                    data = bookings
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Greška pri pretrazi rezervacija");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}

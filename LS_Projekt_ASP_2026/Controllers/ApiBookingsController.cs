using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Api;
using LS_Projekt_ASP_2026.Data;
using LS_Projekt_ASP_2026.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LS_Projekt_ASP_2026.Controllers;

[ApiController]
[Route("api/v1/bookings")]
[Authorize(Roles = "Client,Producer,Admin")]
public class ApiBookingsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ApiBookingsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiListResponse<BookingDto>>> GetAll(
        [FromQuery] string? q,
        [FromQuery] BookingStatus? status,
        [FromQuery] int? clientId,
        [FromQuery] int? producerId,
        [FromQuery] int? studioRoomId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        var query = IncludeAll(_context.Bookings.AsNoTracking());
        query = ApplyUserScope(query);

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(b =>
                b.Purpose.Contains(term) ||
                b.Client.Name.Contains(term) ||
                b.Client.Surname.Contains(term) ||
                b.Producer.Name.Contains(term) ||
                b.Producer.Surname.Contains(term) ||
                b.StudioRoom.Name.Contains(term));
        }

        if (status.HasValue) query = query.Where(b => b.Status == status.Value);
        if (clientId.HasValue) query = query.Where(b => b.ClientId == clientId.Value);
        if (producerId.HasValue) query = query.Where(b => b.ProducerId == producerId.Value);
        if (studioRoomId.HasValue) query = query.Where(b => b.StudioRoomId == studioRoomId.Value);
        if (from.HasValue) query = query.Where(b => b.StartTime >= from.Value);
        if (to.HasValue) query = query.Where(b => b.EndTime <= to.Value);

        var bookings = await query.OrderByDescending(b => b.StartTime).ToListAsync();
        var data = bookings.Select(b => b.ToDto()).ToList();
        return Ok(new ApiListResponse<BookingDto>(data.Count, data));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookingDto>> GetById(int id)
    {
        var booking = await ApplyUserScope(IncludeAll(_context.Bookings.AsNoTracking()))
            .FirstOrDefaultAsync(b => b.Id == id);
        return booking == null ? NotFound() : Ok(booking.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<BookingDto>> Create(BookingCreateDto dto)
    {
        if (User.IsInRole("Client") && User.GetUserId() != dto.ClientId)
        {
            return Forbid();
        }

        var validation = await ValidateReferences(dto.ClientId, dto.ProducerId, dto.StudioRoomId);
        if (validation != null) return validation;

        if (dto.EndTime <= dto.StartTime)
        {
            return BadRequest(new { message = "Vrijeme zavrsetka mora biti nakon vremena pocetka." });
        }

        var booking = new Booking
        {
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            CreatedAt = DateTime.Now,
            Status = dto.Status,
            Purpose = dto.Purpose.Trim(),
            TotalPrice = dto.TotalPrice,
            RequiresEngineer = dto.RequiresEngineer,
            AdditionalNotes = dto.AdditionalNotes.Trim(),
            ClientId = dto.ClientId,
            ProducerId = dto.ProducerId,
            StudioRoomId = dto.StudioRoomId
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        var created = await IncludeAll(_context.Bookings.AsNoTracking()).FirstAsync(b => b.Id == booking.Id);
        return CreatedAtAction(nameof(GetById), new { id = booking.Id }, created.ToDto());
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Producer,Admin")]
    public async Task<ActionResult<BookingDto>> Update(int id, BookingUpdateDto dto)
    {
        var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
        if (booking == null)
        {
            return NotFound();
        }

        var validation = await ValidateReferences(dto.ClientId, dto.ProducerId, dto.StudioRoomId);
        if (validation != null) return validation;

        if (dto.EndTime <= dto.StartTime)
        {
            return BadRequest(new { message = "Vrijeme zavrsetka mora biti nakon vremena pocetka." });
        }

        booking.StartTime = dto.StartTime;
        booking.EndTime = dto.EndTime;
        booking.Status = dto.Status;
        booking.Purpose = dto.Purpose.Trim();
        booking.TotalPrice = dto.TotalPrice;
        booking.RequiresEngineer = dto.RequiresEngineer;
        booking.AdditionalNotes = dto.AdditionalNotes.Trim();
        booking.ClientId = dto.ClientId;
        booking.ProducerId = dto.ProducerId;
        booking.StudioRoomId = dto.StudioRoomId;

        await _context.SaveChangesAsync();
        var updated = await IncludeAll(_context.Bookings.AsNoTracking()).FirstAsync(b => b.Id == id);
        return Ok(updated.ToDto());
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Producer,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
        if (booking == null)
        {
            return NotFound();
        }

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private IQueryable<Booking> IncludeAll(IQueryable<Booking> query)
    {
        return query.Include(b => b.Client).Include(b => b.Producer).Include(b => b.StudioRoom);
    }

    private IQueryable<Booking> ApplyUserScope(IQueryable<Booking> query)
    {
        var userId = User.GetUserId();
        if (User.IsInRole("Admin") || userId is null)
        {
            return query;
        }

        return User.IsInRole("Producer")
            ? query.Where(b => b.ProducerId == userId.Value)
            : query.Where(b => b.ClientId == userId.Value);
    }

    private async Task<ActionResult?> ValidateReferences(int clientId, int producerId, int studioRoomId)
    {
        if (!await _context.Clients.AnyAsync(c => c.Id == clientId)) return BadRequest(new { message = "Klijent ne postoji." });
        if (!await _context.Producers.AnyAsync(p => p.Id == producerId)) return BadRequest(new { message = "Producent ne postoji." });
        if (!await _context.StudioRooms.AnyAsync(s => s.Id == studioRoomId)) return BadRequest(new { message = "Studio ne postoji." });
        return null;
    }
}

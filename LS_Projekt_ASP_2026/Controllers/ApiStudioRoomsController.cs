using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Api;
using LS_Projekt_ASP_2026.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LS_Projekt_ASP_2026.Controllers;

[ApiController]
[Route("api/v1/studio-rooms")]
[Authorize(Roles = "Producer,Admin")]
public class ApiStudioRoomsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ApiStudioRoomsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiListResponse<StudioRoomDto>>> GetAll([FromQuery] string? q, [FromQuery] int? minCapacity, [FromQuery] bool? hasVocalBooth)
    {
        var query = _context.StudioRooms.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(r => r.Name.Contains(term) || r.Location.Contains(term) || r.EquipmentSummary.Contains(term));
        }

        if (minCapacity.HasValue)
        {
            query = query.Where(r => r.Capacity >= minCapacity.Value);
        }

        if (hasVocalBooth.HasValue)
        {
            query = query.Where(r => r.HasVocalBooth == hasVocalBooth.Value);
        }

        var rooms = await query.OrderBy(r => r.Name).ToListAsync();
        var data = rooms.Select(r => r.ToDto()).ToList();
        return Ok(new ApiListResponse<StudioRoomDto>(data.Count, data));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudioRoomDto>> GetById(int id)
    {
        var room = await _context.StudioRooms.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        return room == null ? NotFound() : Ok(room.ToDto());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<StudioRoomDto>> Create(StudioRoomCreateDto dto)
    {
        var room = new StudioRoom
        {
            Name = dto.Name.Trim(),
            Location = dto.Location.Trim(),
            Capacity = dto.Capacity,
            HasVocalBooth = dto.HasVocalBooth,
            HasAnalogGear = dto.HasAnalogGear,
            HourlyPrice = dto.HourlyPrice,
            EquipmentSummary = dto.EquipmentSummary.Trim()
        };

        _context.StudioRooms.Add(room);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room.ToDto());
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<StudioRoomDto>> Update(int id, StudioRoomUpdateDto dto)
    {
        var room = await _context.StudioRooms.FirstOrDefaultAsync(r => r.Id == id);
        if (room == null)
        {
            return NotFound();
        }

        room.Name = dto.Name.Trim();
        room.Location = dto.Location.Trim();
        room.Capacity = dto.Capacity;
        room.HasVocalBooth = dto.HasVocalBooth;
        room.HasAnalogGear = dto.HasAnalogGear;
        room.HourlyPrice = dto.HourlyPrice;
        room.EquipmentSummary = dto.EquipmentSummary.Trim();

        await _context.SaveChangesAsync();
        return Ok(room.ToDto());
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var room = await _context.StudioRooms.FirstOrDefaultAsync(r => r.Id == id);
        if (room == null)
        {
            return NotFound();
        }

        if (await _context.Bookings.AnyAsync(b => b.StudioRoomId == id))
        {
            return Conflict(new { message = "Studio ima povezane rezervacije." });
        }

        _context.StudioRooms.Remove(room);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

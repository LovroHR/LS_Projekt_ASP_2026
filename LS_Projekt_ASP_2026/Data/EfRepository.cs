using AudioProductionManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace LS_Projekt_ASP_2026.Data;

public class EfRepository : IRepository
{
    private readonly AppDbContext _context;

    public EfRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Booking> GetAllBookings()
    {
        return _context.Bookings
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.Producer)
            .Include(x => x.StudioRoom)
            .OrderByDescending(x => x.StartTime)
            .ToList();
    }

    public Booking? GetBookingById(int id)
    {
        return _context.Bookings
            .Include(x => x.Client)
            .Include(x => x.Producer)
            .Include(x => x.StudioRoom)
            .FirstOrDefault(x => x.Id == id);
    }

    public void AddBooking(Booking booking)
    {
        _context.Bookings.Add(booking);
        _context.SaveChanges();
    }

    public void UpdateBooking(Booking booking)
    {
        var existing = _context.Bookings.FirstOrDefault(x => x.Id == booking.Id);
        if (existing is null)
        {
            return;
        }

        existing.StartTime = booking.StartTime;
        existing.EndTime = booking.EndTime;
        existing.CreatedAt = booking.CreatedAt;
        existing.Status = booking.Status;
        existing.Purpose = booking.Purpose;
        existing.TotalPrice = booking.TotalPrice;
        existing.RequiresEngineer = booking.RequiresEngineer;
        existing.AdditionalNotes = booking.AdditionalNotes;
        existing.ClientId = booking.ClientId;
        existing.ProducerId = booking.ProducerId;
        existing.StudioRoomId = booking.StudioRoomId;

        _context.SaveChanges();
    }

    public void DeleteBooking(int id)
    {
        var booking = _context.Bookings.Find(id);
        if (booking is null)
        {
            return;
        }

        _context.Bookings.Remove(booking);
        _context.SaveChanges();
    }

    public IEnumerable<Client> GetAllClients()
    {
        return _context.Clients
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Surname)
            .ToList();
    }

    public Client? GetClientById(int id)
    {
        return _context.Clients
            .Include(x => x.Bookings)
            .Include(x => x.Projects)
            .FirstOrDefault(x => x.Id == id);
    }

    public void AddClient(Client client)
    {
        _context.Clients.Add(client);
        _context.SaveChanges();
    }

    public void UpdateClient(Client client)
    {
        _context.Clients.Update(client);
        _context.SaveChanges();
    }

    public void DeleteClient(int id)
    {
        var client = _context.Clients.Find(id);
        if (client is null)
        {
            return;
        }

        _context.Clients.Remove(client);
        _context.SaveChanges();
    }

    public IEnumerable<Producer> GetAllProducers()
    {
        return _context.Producers
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Surname)
            .ToList();
    }

    public Producer? GetProducerById(int id)
    {
        return _context.Producers
            .Include(x => x.AssignedBookings)
            .Include(x => x.ManagedProjects)
            .FirstOrDefault(x => x.Id == id);
    }

    public void AddProducer(Producer producer)
    {
        _context.Producers.Add(producer);
        _context.SaveChanges();
    }

    public void UpdateProducer(Producer producer)
    {
        _context.Producers.Update(producer);
        _context.SaveChanges();
    }

    public void DeleteProducer(int id)
    {
        var producer = _context.Producers.Find(id);
        if (producer is null)
        {
            return;
        }

        _context.Producers.Remove(producer);
        _context.SaveChanges();
    }

    public IEnumerable<StudioRoom> GetAllStudioRooms()
    {
        return _context.StudioRooms
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToList();
    }

    public StudioRoom? GetStudioRoomById(int id)
    {
        return _context.StudioRooms
            .Include(x => x.Bookings)
            .FirstOrDefault(x => x.Id == id);
    }

    public IEnumerable<Booking> GetBookingsByClient(int clientId)
    {
        return _context.Bookings
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.Producer)
            .Include(x => x.StudioRoom)
            .Where(b => b.ClientId == clientId)
            .OrderByDescending(b => b.StartTime)
            .ToList();
    }

    public IEnumerable<Booking> GetBookingsByStudio(int studioId)
    {
        return _context.Bookings
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.Producer)
            .Include(x => x.StudioRoom)
            .Where(b => b.StudioRoomId == studioId)
            .OrderByDescending(b => b.StartTime)
            .ToList();
    }

    public IEnumerable<Booking> GetAvailableBookingSlots(int studioId, DateTime startDate, DateTime endDate)
    {
        return _context.Bookings
            .AsNoTracking()
            .Where(b => b.StudioRoomId == studioId && b.StartTime >= startDate && b.EndTime <= endDate)
            .OrderBy(b => b.StartTime)
            .ToList();
    }

    public void AddStudioRoom(StudioRoom studioRoom)
    {
        _context.StudioRooms.Add(studioRoom);
        _context.SaveChanges();
    }

    public void UpdateStudioRoom(StudioRoom studioRoom)
    {
        _context.StudioRooms.Update(studioRoom);
        _context.SaveChanges();
    }

    public void DeleteStudioRoom(int id)
    {
        var sr = _context.StudioRooms.Find(id);
        if (sr is null) return;
        _context.StudioRooms.Remove(sr);
        _context.SaveChanges();
    }

    public IEnumerable<AudioProject> GetAllProjects()
    {
        return _context.AudioProjects
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.Producer)
            .OrderByDescending(x => x.CreatedAt)
            .ToList();
    }

    public AudioProject? GetProjectById(int id)
    {
        return _context.AudioProjects
            .Include(x => x.Client)
            .Include(x => x.Producer)
            .Include(x => x.Versions)
                .ThenInclude(v => v.Comments)
            .FirstOrDefault(x => x.Id == id);
    }

    public void AddProject(AudioProject project)
    {
        _context.AudioProjects.Add(project);
        _context.SaveChanges();
    }

    public void UpdateProject(AudioProject project)
    {
        _context.AudioProjects.Update(project);
        _context.SaveChanges();
    }

    public void DeleteProject(int id)
    {
        var project = _context.AudioProjects.Find(id);
        if (project is null)
        {
            return;
        }

        _context.AudioProjects.Remove(project);
        _context.SaveChanges();
    }
}

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
        _context.Bookings.Update(booking);
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
}

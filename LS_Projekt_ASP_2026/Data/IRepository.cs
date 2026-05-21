using System.Collections.Generic;
using AudioProductionManagement.Model;

namespace LS_Projekt_ASP_2026.Data
{
    public interface IRepository
    {
        IEnumerable<Booking> GetAllBookings();
        Booking? GetBookingById(int id);
        void AddBooking(Booking booking);
        void UpdateBooking(Booking booking);
        void DeleteBooking(int id);

        IEnumerable<Client> GetAllClients();
        Client? GetClientById(int id);
        void AddClient(Client client);
        void UpdateClient(Client client);
        void DeleteClient(int id);

        IEnumerable<Producer> GetAllProducers();
        Producer? GetProducerById(int id);
        void AddProducer(Producer producer);
        void UpdateProducer(Producer producer);
        void DeleteProducer(int id);

        IEnumerable<StudioRoom> GetAllStudioRooms();
        StudioRoom? GetStudioRoomById(int id);
        void AddStudioRoom(StudioRoom studioRoom);
        void UpdateStudioRoom(StudioRoom studioRoom);
        void DeleteStudioRoom(int id);

        IEnumerable<Booking> GetBookingsByClient(int clientId);
        IEnumerable<Booking> GetBookingsByStudio(int studioId);
        IEnumerable<Booking> GetAvailableBookingSlots(int studioId, System.DateTime startDate, System.DateTime endDate);

        IEnumerable<AudioProject> GetAllProjects();
        AudioProject? GetProjectById(int id);
        void AddProject(AudioProject project);
        void UpdateProject(AudioProject project);
        void DeleteProject(int id);
    }
}

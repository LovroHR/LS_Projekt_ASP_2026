using System.Collections.Generic;
using AudioProductionManagement.Model;

namespace LS_Projekt_ASP_2026.Data
{
    public interface IRepository
    {
        IEnumerable<Booking> GetAllBookings();
        Booking GetBookingById(int id);
        void AddBooking(Booking booking);
        void UpdateBooking(Booking booking);
        void DeleteBooking(int id);

        IEnumerable<Client> GetAllClients();
        Client GetClientById(int id);
        void AddClient(Client client);

        IEnumerable<Producer> GetAllProducers();
        Producer GetProducerById(int id);

        IEnumerable<StudioRoom> GetAllStudioRooms();
        StudioRoom GetStudioRoomById(int id);

        IEnumerable<AudioProject> GetAllProjects();
        AudioProject GetProjectById(int id);
    }
}

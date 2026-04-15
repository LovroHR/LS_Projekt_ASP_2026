using System;
using System.Collections.Generic;
using System.Linq;
using AudioProductionManagement.Model;

namespace LS_Projekt_ASP_2026.Data
{
    /// <summary>
    /// Mock Repository sa statičkim podacima iz Lab 1 Osnove C# / LINQ
    /// Koristi memorijsku kolekciju umjesto baze podataka
    /// </summary>
    public class MockRepository : IRepository
    {
        // Statičke kolekcije
        private readonly List<Client> _clients;
        private readonly List<Producer> _producers;
        private readonly List<StudioRoom> _studioRooms;
        private readonly List<Booking> _bookings;
        private readonly List<AudioProject> _audioProjects;
        private readonly List<ProjectVersion> _projectVersions;
        private readonly List<TimecodedComment> _timecodedComments;

        public MockRepository()
        {
            // Inicijalizacija kolekcija
            _clients = new List<Client>();
            _producers = new List<Producer>();
            _studioRooms = new List<StudioRoom>();
            _bookings = new List<Booking>();
            _audioProjects = new List<AudioProject>();
            _projectVersions = new List<ProjectVersion>();
            _timecodedComments = new List<TimecodedComment>();

            // Učitaj demo podatke
            InitializeDemoData();
        }

        /// <summary>
        /// Inicijalizira sve demo podatke - Lab 1 Osnove C# / LINQ
        /// </summary>
        private void InitializeDemoData()
        {
            // ============== KLIJENTI ==============
            var client1 = new Client
            {
                Id = 1,
                Name = "Marina",
                Surname = "Horvat",
                Email = "marina.horvat@musicstudio.com",
                Password = "password123",
                PhoneNumber = "+385 1 234 5678",
                DateOfBirth = new DateTime(1985, 3, 15),
                Address = "Ul. Strahovska 2",
                Country = "Hrvatska",
                CreatedAt = new DateTime(2025, 6, 15),
                Role = UserRole.Client,
                CompanyName = "Marina Productions",
                BillingAddress = "Ul. Strahovska 2, Zagreb",
                IsPriorityClient = true,
                Notes = "VIP klijent - često snima podkaste",
                Bookings = new List<Booking>(),
                Projects = new List<AudioProject>()
            };

            var client2 = new Client
            {
                Id = 2,
                Name = "Andrej",
                Surname = "Novaković",
                Email = "andrej.novakovic@indie.band",
                Password = "password123",
                PhoneNumber = "+385 1 555 9876",
                DateOfBirth = new DateTime(1990, 7, 22),
                Address = "Tehnička 12",
                Country = "Hrvatska",
                CreatedAt = new DateTime(2025, 7, 20),
                Role = UserRole.Client,
                CompanyName = "Indie Records",
                BillingAddress = "Tehnička 12, Split",
                IsPriorityClient = false,
                Notes = "Producira indie glazbu i albume",
                Bookings = new List<Booking>(),
                Projects = new List<AudioProject>()
            };

            var client3 = new Client
            {
                Id = 3,
                Name = "Petra",
                Surname = "Nikolić",
                Email = "petra.nikolic@podcast.agency",
                Password = "password123",
                PhoneNumber = "+385 1 777 1234",
                DateOfBirth = new DateTime(1988, 11, 8),
                Address = "Tkalčićeva 5",
                Country = "Hrvatska",
                CreatedAt = new DateTime(2025, 8, 10),
                Role = UserRole.Client,
                CompanyName = "Podcast Agency",
                BillingAddress = "Tkalčićeva 5, Zagreb",
                IsPriorityClient = true,
                Notes = "Specijalizirana za produkciju podkasta",
                Bookings = new List<Booking>(),
                Projects = new List<AudioProject>()
            };

            _clients.AddRange(new[] { client1, client2, client3 });

            // ============== PRODUCENTI ==============
            var producer1 = new Producer
            {
                Id = 1,
                Name = "Filip",
                Surname = "Marković",
                Email = "filip.markovic@studio.hr",
                PhoneNumber = "+385 1 234 0000",
                CreatedAt = new DateTime(2023, 1, 5),
                Role = UserRole.Producer,
                Specialization = "Pop i Electronic",
                HourlyRate = 150.00m,
                IsExternalCollaborator = false,
                Biography = "10+ godina iskustva u produkciji pop i elektroničke glazbe",
                AssignedBookings = new List<Booking>(),
                ManagedProjects = new List<AudioProject>()
            };

            var producer2 = new Producer
            {
                Id = 2,
                Name = "Juraj",
                Surname = "Horvat",
                Email = "juraj.horvat@studio.hr",
                PhoneNumber = "+385 1 234 1111",
                CreatedAt = new DateTime(2022, 3, 12),
                Role = UserRole.Producer,
                Specialization = "Rock i Metal",
                HourlyRate = 175.00m,
                IsExternalCollaborator = false,
                Biography = "Producent specijaliziran za rock i metal produkciju",
                AssignedBookings = new List<Booking>(),
                ManagedProjects = new List<AudioProject>()
            };

            var producer3 = new Producer
            {
                Id = 3,
                Name = "Søren",
                Surname = "Jensen",
                Email = "soren.jensen@independent.producer",
                PhoneNumber = "+45 40 12 3456",
                CreatedAt = new DateTime(2024, 5, 8),
                Role = UserRole.Producer,
                Specialization = "Classical i Orchestral",
                HourlyRate = 200.00m,
                IsExternalCollaborator = true,
                Biography = "Vanjski suradnik - specijalist za klasičnu glazbu",
                AssignedBookings = new List<Booking>(),
                ManagedProjects = new List<AudioProject>()
            };

            _producers.AddRange(new[] { producer1, producer2, producer3 });

            // ============== STUDIO PROSTORIJE ==============
            var studio1 = new StudioRoom
            {
                Id = 1,
                Name = "Studio A - Pro",
                Location = "1. kat",
                Capacity = 6,
                HasVocalBooth = true,
                HasAnalogGear = true,
                HourlyPrice = 120.00m,
                EquipmentSummary = "Neve 8088 console, Neumann U87 mic, Neve 1073 preamp",
                Bookings = new List<Booking>()
            };

            var studio2 = new StudioRoom
            {
                Id = 2,
                Name = "Studio B - Digital",
                Location = "2. kat",
                Capacity = 4,
                HasVocalBooth = false,
                HasAnalogGear = false,
                HourlyPrice = 80.00m,
                EquipmentSummary = "ProTools, SSL Nucleus, Genelec 8050B monitoring",
                Bookings = new List<Booking>()
            };

            var studio3 = new StudioRoom
            {
                Id = 3,
                Name = "Live Room",
                Location = "Prizemlje",
                Capacity = 12,
                HasVocalBooth = true,
                HasAnalogGear = true,
                HourlyPrice = 150.00m,
                EquipmentSummary = "Vintage Studer A820, Neve 1073 preamps, Live recording setup",
                Bookings = new List<Booking>()
            };

            _studioRooms.AddRange(new[] { studio1, studio2, studio3 });

            // ============== REZERVACIJE ==============
            var booking1 = new Booking
            {
                Id = 1,
                StartTime = new DateTime(2026, 4, 3, 10, 0, 0),
                EndTime = new DateTime(2026, 4, 3, 13, 0, 0),
                CreatedAt = new DateTime(2026, 3, 25),
                Status = BookingStatus.Confirmed,
                Purpose = "Vokal session za novi single",
                TotalPrice = 360.00m,
                RequiresEngineer = true,
                AdditionalNotes = "Potreban inženjer sa iskustvom u vokalnim sesijama",
                ClientId = 1,
                Client = client1,
                ProducerId = 1,
                Producer = producer1,
                StudioRoomId = 1,
                StudioRoom = studio1
            };

            var booking2 = new Booking
            {
                Id = 2,
                StartTime = new DateTime(2026, 4, 4, 14, 0, 0),
                EndTime = new DateTime(2026, 4, 4, 17, 0, 0),
                CreatedAt = new DateTime(2026, 3, 26),
                Status = BookingStatus.Confirmed,
                Purpose = "Recording sesija za EP",
                TotalPrice = 420.00m,
                RequiresEngineer = true,
                AdditionalNotes = "Banda od 5 članova",
                ClientId = 2,
                Client = client2,
                ProducerId = 2,
                Producer = producer2,
                StudioRoomId = 3,
                StudioRoom = studio3
            };

            var booking3 = new Booking
            {
                Id = 3,
                StartTime = new DateTime(2026, 4, 5, 9, 0, 0),
                EndTime = new DateTime(2026, 4, 5, 12, 0, 0),
                CreatedAt = new DateTime(2026, 3, 27),
                Status = BookingStatus.Confirmed,
                Purpose = "Mastering sesija - Podcast",
                TotalPrice = 240.00m,
                RequiresEngineer = false,
                AdditionalNotes = "Mastering dva epizode",
                ClientId = 3,
                Client = client3,
                ProducerId = 1,
                Producer = producer1,
                StudioRoomId = 2,
                StudioRoom = studio2
            };

            var booking4 = new Booking
            {
                Id = 4,
                StartTime = new DateTime(2026, 4, 6, 11, 0, 0),
                EndTime = new DateTime(2026, 4, 6, 15, 0, 0),
                CreatedAt = new DateTime(2026, 3, 28),
                Status = BookingStatus.Pending,
                Purpose = "Orkestralna snemanja",
                TotalPrice = 600.00m,
                RequiresEngineer = true,
                AdditionalNotes = "Klasična glazba, potreban iskusan sound engineer",
                ClientId = 1,
                Client = client1,
                ProducerId = 3,
                Producer = producer3,
                StudioRoomId = 3,
                StudioRoom = studio3
            };

            var booking5 = new Booking
            {
                Id = 5,
                StartTime = new DateTime(2026, 4, 7, 15, 0, 0),
                EndTime = new DateTime(2026, 4, 7, 18, 0, 0),
                CreatedAt = new DateTime(2026, 3, 29),
                Status = BookingStatus.InProgress,
                Purpose = "Mixdown - Prvi album",
                TotalPrice = 360.00m,
                RequiresEngineer = true,
                AdditionalNotes = "Heavy rock album, 12 pjesama",
                ClientId = 2,
                Client = client2,
                ProducerId = 2,
                Producer = producer2,
                StudioRoomId = 1,
                StudioRoom = studio1
            };

            var booking6 = new Booking
            {
                Id = 6,
                StartTime = new DateTime(2026, 4, 8, 10, 0, 0),
                EndTime = new DateTime(2026, 4, 8, 13, 0, 0),
                CreatedAt = new DateTime(2026, 3, 30),
                Status = BookingStatus.Completed,
                Purpose = "Post-production podcast serije",
                TotalPrice = 240.00m,
                RequiresEngineer = false,
                AdditionalNotes = "Audio editiranje i mastering",
                ClientId = 3,
                Client = client3,
                ProducerId = 1,
                Producer = producer1,
                StudioRoomId = 2,
                StudioRoom = studio2
            };

            _bookings.AddRange(new[] { booking1, booking2, booking3, booking4, booking5, booking6 });

            // Povezivanje booking-a sa klijentima i producentima
            client1.Bookings.AddRange(new[] { booking1, booking4 });
            client2.Bookings.AddRange(new[] { booking2, booking5 });
            client3.Bookings.AddRange(new[] { booking3, booking6 });

            producer1.AssignedBookings.AddRange(new[] { booking1, booking3, booking6 });
            producer2.AssignedBookings.AddRange(new[] { booking2, booking5 });
            producer3.AssignedBookings.Add(booking4);

            studio1.Bookings.AddRange(new[] { booking1, booking5 });
            studio2.Bookings.AddRange(new[] { booking3, booking6 });
            studio3.Bookings.AddRange(new[] { booking2, booking4 });

            // ============== AUDIO PROJEKTI ==============
            var project1 = new AudioProject
            {
                Id = 1,
                Title = "Marina's Digital Dreams",
                Type = ProjectType.Single,
                Status = ProjectStatus.Active,
                Genre = "Electronic Pop",
                TargetDurationSeconds = 240,
                CreatedAt = new DateTime(2026, 3, 1),
                Deadline = new DateTime(2026, 4, 30),
                Budget = 2500.00m,
                AllowClientComments = true,
                SharedFolderUrl = "https://drive.google.com/marina-dreams",
                ClientId = 1,
                Client = client1,
                ProducerId = 1,
                Producer = producer1,
                Versions = new List<ProjectVersion>()
            };

            var project2 = new AudioProject
            {
                Id = 2,
                Title = "Heavy Thunder - Full Album",
                Type = ProjectType.Album,
                Status = ProjectStatus.WaitingForFeedback,
                Genre = "Heavy Metal",
                TargetDurationSeconds = 3600,
                CreatedAt = new DateTime(2026, 2, 15),
                Deadline = new DateTime(2026, 5, 15),
                Budget = 8000.00m,
                AllowClientComments = true,
                SharedFolderUrl = "https://drive.google.com/heavy-thunder",
                ClientId = 2,
                Client = client2,
                ProducerId = 2,
                Producer = producer2,
                Versions = new List<ProjectVersion>()
            };

            var project3 = new AudioProject
            {
                Id = 3,
                Title = "Business Talk - Podcast Season 1",
                Type = ProjectType.Podcast,
                Status = ProjectStatus.Revision,
                Genre = "Business / Educational",
                TargetDurationSeconds = 1800,
                CreatedAt = new DateTime(2026, 2, 1),
                Deadline = new DateTime(2026, 4, 15),
                Budget = 1500.00m,
                AllowClientComments = true,
                SharedFolderUrl = "https://drive.google.com/business-talk",
                ClientId = 3,
                Client = client3,
                ProducerId = 1,
                Producer = producer1,
                Versions = new List<ProjectVersion>()
            };

            _audioProjects.AddRange(new[] { project1, project2, project3 });

            client1.Projects.Add(project1);
            client2.Projects.Add(project2);
            client3.Projects.Add(project3);

            producer1.ManagedProjects.AddRange(new[] { project1, project3 });
            producer2.ManagedProjects.Add(project2);

            // ============== VERZIJE PROJEKATA ==============
            var project1_v1 = new ProjectVersion
            {
                Id = 1,
                ProjectId = 1,
                Project = project1,
                VersionNumber = 1,
                Name = "First Draft",
                Description = "Prva verzija - raw recordings",
                CreatedAt = new DateTime(2026, 3, 10),
                DurationSeconds = 238,
                FileSize = 150.5m,
                FileUrl = "https://storage.com/marina-dreams-v1.wav",
                Notes = "Potrebna mastering sesija",
                IsApproved = false,
                Comments = new List<TimecodedComment>()
            };

            var project1_v2 = new ProjectVersion
            {
                Id = 2,
                ProjectId = 1,
                Project = project1,
                VersionNumber = 2,
                Name = "Mixed Version",
                Description = "Verzija nakon miksinga",
                CreatedAt = new DateTime(2026, 3, 20),
                DurationSeconds = 240,
                FileSize = 175.2m,
                FileUrl = "https://storage.com/marina-dreams-v2.wav",
                Notes = "Mixdown gotov, čeka mastering",
                IsApproved = false,
                Comments = new List<TimecodedComment>()
            };

            project1.Versions.AddRange(new[] { project1_v1, project1_v2 });
            _projectVersions.AddRange(new[] { project1_v1, project1_v2 });

            var project2_v1 = new ProjectVersion
            {
                Id = 3,
                ProjectId = 2,
                Project = project2,
                VersionNumber = 1,
                Name = "Raw Recordings",
                Description = "Sirove snimke sa sesija",
                CreatedAt = new DateTime(2026, 2, 25),
                DurationSeconds = 3580,
                FileSize = 2200.0m,
                FileUrl = "https://storage.com/heavy-thunder-v1.wav",
                Notes = "Sve 12 pjesama snimljeno",
                IsApproved = false,
                Comments = new List<TimecodedComment>()
            };

            var project2_v2 = new ProjectVersion
            {
                Id = 4,
                ProjectId = 2,
                Project = project2,
                VersionNumber = 2,
                Name = "Mixed - First Pass",
                Description = "Prvi mixdown - svi kanali",
                CreatedAt = new DateTime(2026, 3, 15),
                DurationSeconds = 3600,
                FileSize = 2250.5m,
                FileUrl = "https://storage.com/heavy-thunder-v2.wav",
                Notes = "Čeka povratne informacije klijenta",
                IsApproved = false,
                Comments = new List<TimecodedComment>()
            };

            project2.Versions.AddRange(new[] { project2_v1, project2_v2 });
            _projectVersions.AddRange(new[] { project2_v1, project2_v2 });

            var project3_v1 = new ProjectVersion
            {
                Id = 5,
                ProjectId = 3,
                Project = project3,
                VersionNumber = 1,
                Name = "Raw Podcast Feed",
                Description = "Sirove snimke - 6 epizoda",
                CreatedAt = new DateTime(2026, 2, 10),
                DurationSeconds = 1780,
                FileSize = 800.0m,
                FileUrl = "https://storage.com/business-talk-v1.wav",
                Notes = "Trebalo editiranje i normalizacija",
                IsApproved = false,
                Comments = new List<TimecodedComment>()
            };

            var project3_v2 = new ProjectVersion
            {
                Id = 6,
                ProjectId = 3,
                Project = project3,
                VersionNumber = 2,
                Name = "Edited & Normalized",
                Description = "Nakon editiranja i normalizacije",
                CreatedAt = new DateTime(2026, 3, 5),
                DurationSeconds = 1800,
                FileSize = 820.0m,
                FileUrl = "https://storage.com/business-talk-v2.wav",
                Notes = "Revizije requested - trebaju izmjene na epizodi 3",
                IsApproved = false,
                Comments = new List<TimecodedComment>()
            };

            project3.Versions.AddRange(new[] { project3_v1, project3_v2 });
            _projectVersions.AddRange(new[] { project3_v1, project3_v2 });

            // ============== KOMENTARI ==============
            var comment1_1 = new TimecodedComment
            {
                Id = 1,
                TimestampSeconds = 45.5m,
                Message = "Previše reverba na vokalima - trebalo bi malo suhije",
                CreatedAt = new DateTime(2026, 3, 12),
                IsResolved = true,
                Category = "Vocals",
                IsInternalNote = false,
                ProjectVersionId = 1,
                ProjectVersion = project1_v1,
                AuthorId = 1,
                Author = producer1
            };

            var comment1_2 = new TimecodedComment
            {
                Id = 2,
                TimestampSeconds = 120.0m,
                Message = "Drop je fantastičan! Svida mi se ovo.",
                CreatedAt = new DateTime(2026, 3, 12),
                IsResolved = false,
                Category = "Production",
                IsInternalNote = false,
                ProjectVersionId = 1,
                ProjectVersion = project1_v1,
                AuthorId = 1,
                Author = client1
            };

            project1_v1.Comments.AddRange(new[] { comment1_1, comment1_2 });
            _timecodedComments.AddRange(new[] { comment1_1, comment1_2 });

            // Dodatni komentari...
            var comment2_1 = new TimecodedComment
            {
                Id = 3,
                TimestampSeconds = 45.5m,
                Message = "Vokal je sada mnogo bolje balanciran s ostatkom mixa",
                CreatedAt = new DateTime(2026, 3, 22),
                IsResolved = true,
                Category = "Vocals",
                IsInternalNote = true,
                ProjectVersionId = 2,
                ProjectVersion = project1_v2,
                AuthorId = 1,
                Author = producer1
            };

            project1_v2.Comments.Add(comment2_1);
            _timecodedComments.Add(comment2_1);
        }

        // ============== BOOKING METODE ==============

        public IEnumerable<Booking> GetAllBookings()
        {
            return _bookings.OrderByDescending(b => b.StartTime).ToList();
        }

        public Booking GetBookingById(int id)
        {
            return _bookings.FirstOrDefault(b => b.Id == id);
        }

        public void AddBooking(Booking booking)
        {
            if (booking.Id == 0)
                booking.Id = _bookings.Max(b => b.Id) + 1;
            _bookings.Add(booking);
        }

        public void UpdateBooking(Booking booking)
        {
            var existing = GetBookingById(booking.Id);
            if (existing != null)
            {
                existing.StartTime = booking.StartTime;
                existing.EndTime = booking.EndTime;
                existing.Status = booking.Status;
                existing.Purpose = booking.Purpose;
                existing.TotalPrice = booking.TotalPrice;
                existing.RequiresEngineer = booking.RequiresEngineer;
                existing.AdditionalNotes = booking.AdditionalNotes;
                existing.ProducerId = booking.ProducerId;
                existing.StudioRoomId = booking.StudioRoomId;
            }
        }

        public void DeleteBooking(int id)
        {
            var booking = GetBookingById(id);
            if (booking != null)
                _bookings.Remove(booking);
        }

        public IEnumerable<Booking> GetBookingsByClient(int clientId)
        {
            return _bookings.Where(b => b.ClientId == clientId).OrderByDescending(b => b.StartTime).ToList();
        }

        public IEnumerable<Booking> GetBookingsByStudio(int studioId)
        {
            return _bookings.Where(b => b.StudioRoomId == studioId).OrderByDescending(b => b.StartTime).ToList();
        }

        public IEnumerable<Booking> GetAvailableBookingSlots(int studioId, DateTime startDate, DateTime endDate)
        {
            return _bookings
                .Where(b => b.StudioRoomId == studioId && 
                            b.StartTime >= startDate && 
                            b.EndTime <= endDate)
                .OrderBy(b => b.StartTime)
                .ToList();
        }

        // ============== CLIENT METODE ==============

        public IEnumerable<Client> GetAllClients()
        {
            return _clients.OrderBy(c => c.Name).ToList();
        }

        public Client GetClientById(int id)
        {
            return _clients.FirstOrDefault(c => c.Id == id);
        }

        public void AddClient(Client client)
        {
            if (client.Id == 0)
                client.Id = _clients.Max(c => c.Id) + 1;
            if (client.Bookings == null)
                client.Bookings = new List<Booking>();
            if (client.Projects == null)
                client.Projects = new List<AudioProject>();
            _clients.Add(client);
        }

        // ============== PRODUCER METODE ==============

        public IEnumerable<Producer> GetAllProducers()
        {
            return _producers.OrderBy(p => p.Name).ToList();
        }

        public Producer GetProducerById(int id)
        {
            return _producers.FirstOrDefault(p => p.Id == id);
        }

        // ============== STUDIO METODE ==============

        public IEnumerable<StudioRoom> GetAllStudioRooms()
        {
            return _studioRooms.OrderBy(s => s.Name).ToList();
        }

        public StudioRoom GetStudioRoomById(int id)
        {
            return _studioRooms.FirstOrDefault(s => s.Id == id);
        }

        // ============== PROJECT METODE ==============

        public IEnumerable<AudioProject> GetAllProjects()
        {
            return _audioProjects.OrderByDescending(p => p.CreatedAt).ToList();
        }

        public AudioProject GetProjectById(int id)
        {
            return _audioProjects.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<ProjectVersion> GetProjectVersionsByProjectId(int projectId)
        {
            return _projectVersions
                .Where(pv => pv.ProjectId == projectId)
                .OrderByDescending(pv => pv.VersionNumber)
                .ToList();
        }
    }
}

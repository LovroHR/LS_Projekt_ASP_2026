using AudioProductionManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace LS_Projekt_ASP_2026.Data
{
    /// <summary>
    /// Seed podaci iz Lab 1 - prebacivanje mock podataka u bazu
    /// </summary>
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                // Ako baza već sadrži podatke, preskoči seeding
                if (context.Clients.Any() || context.Producers.Any() || context.StudioRooms.Any())
                {
                    return;
                }

                // ============== KLIJENTI ==============
                var client1 = new Client
                {
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
                    Notes = "VIP klijent - često snima podkaste"
                };

                var client2 = new Client
                {
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
                    Notes = "Producira indie glazbu i albume"
                };

                var client3 = new Client
                {
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
                    Notes = "Specijalizirana za produkciju podkasta"
                };

                context.Clients.AddRange(client1, client2, client3);
                context.SaveChanges();

                // ============== PRODUCENTI ==============
                var producer1 = new Producer
                {
                    Name = "Filip",
                    Surname = "Marković",
                    Email = "filip.markovic@studio.hr",
                    PhoneNumber = "+385 1 234 0000",
                    CreatedAt = new DateTime(2023, 1, 5),
                    Role = UserRole.Producer,
                    Specialization = "Pop i Electronic",
                    HourlyRate = 150.00m,
                    IsExternalCollaborator = false,
                    Biography = "10+ godina iskustva u produkciji pop i elektroničke glazbe"
                };

                var producer2 = new Producer
                {
                    Name = "Juraj",
                    Surname = "Horvat",
                    Email = "juraj.horvat@studio.hr",
                    PhoneNumber = "+385 1 234 1111",
                    CreatedAt = new DateTime(2022, 3, 12),
                    Role = UserRole.Producer,
                    Specialization = "Rock i Metal",
                    HourlyRate = 175.00m,
                    IsExternalCollaborator = false,
                    Biography = "Producent specijaliziran za rock i metal produkciju"
                };

                var producer3 = new Producer
                {
                    Name = "Søren",
                    Surname = "Jensen",
                    Email = "soren.jensen@independent.producer",
                    PhoneNumber = "+45 40 12 3456",
                    CreatedAt = new DateTime(2024, 5, 8),
                    Role = UserRole.Producer,
                    Specialization = "Classical i Orchestral",
                    HourlyRate = 200.00m,
                    IsExternalCollaborator = true,
                    Biography = "Vanjski suradnik - specijalist za klasičnu glazbu"
                };

                context.Producers.AddRange(producer1, producer2, producer3);
                context.SaveChanges();

                // ============== STUDIO PROSTORIJE ==============
                var studio1 = new StudioRoom
                {
                    Name = "Studio A - Pro",
                    Location = "1. kat",
                    Capacity = 6,
                    HasVocalBooth = true,
                    HasAnalogGear = true,
                    HourlyPrice = 120.00m,
                    EquipmentSummary = "Neve 8088 console, Neumann U87 mic, Neve 1073 preamp"
                };

                var studio2 = new StudioRoom
                {
                    Name = "Studio B - Digital",
                    Location = "2. kat",
                    Capacity = 4,
                    HasVocalBooth = false,
                    HasAnalogGear = false,
                    HourlyPrice = 80.00m,
                    EquipmentSummary = "ProTools, SSL Nucleus, Genelec 8050B monitoring"
                };

                var studio3 = new StudioRoom
                {
                    Name = "Live Room",
                    Location = "Prizemlje",
                    Capacity = 12,
                    HasVocalBooth = true,
                    HasAnalogGear = true,
                    HourlyPrice = 150.00m,
                    EquipmentSummary = "Vintage Studer A820, Neve 1073 preamps, Live recording setup"
                };

                context.StudioRooms.AddRange(studio1, studio2, studio3);
                context.SaveChanges();

                // ============== REZERVACIJE ==============
                var booking1 = new Booking
                {
                    StartTime = new DateTime(2026, 4, 3, 10, 0, 0),
                    EndTime = new DateTime(2026, 4, 3, 13, 0, 0),
                    CreatedAt = new DateTime(2026, 3, 25),
                    Status = BookingStatus.Confirmed,
                    Purpose = "Vokal session za novi single",
                    TotalPrice = 360.00m,
                    RequiresEngineer = true,
                    AdditionalNotes = "Potreban inženjer sa iskustvom u vokalnim sesijama",
                    ClientId = client1.Id,
                    ProducerId = producer1.Id,
                    StudioRoomId = studio1.Id
                };

                var booking2 = new Booking
                {
                    StartTime = new DateTime(2026, 4, 4, 14, 0, 0),
                    EndTime = new DateTime(2026, 4, 4, 17, 0, 0),
                    CreatedAt = new DateTime(2026, 3, 26),
                    Status = BookingStatus.Confirmed,
                    Purpose = "Recording sesija za EP",
                    TotalPrice = 420.00m,
                    RequiresEngineer = true,
                    AdditionalNotes = "Banda od 5 članova",
                    ClientId = client2.Id,
                    ProducerId = producer2.Id,
                    StudioRoomId = studio3.Id
                };

                var booking3 = new Booking
                {
                    StartTime = new DateTime(2026, 4, 5, 9, 0, 0),
                    EndTime = new DateTime(2026, 4, 5, 12, 0, 0),
                    CreatedAt = new DateTime(2026, 3, 27),
                    Status = BookingStatus.Confirmed,
                    Purpose = "Mastering sesija - Podcast",
                    TotalPrice = 240.00m,
                    RequiresEngineer = false,
                    AdditionalNotes = "Mastering dva epizode",
                    ClientId = client3.Id,
                    ProducerId = producer1.Id,
                    StudioRoomId = studio2.Id
                };

                var booking4 = new Booking
                {
                    StartTime = new DateTime(2026, 4, 6, 11, 0, 0),
                    EndTime = new DateTime(2026, 4, 6, 15, 0, 0),
                    CreatedAt = new DateTime(2026, 3, 28),
                    Status = BookingStatus.Pending,
                    Purpose = "Orkestralna snemanja",
                    TotalPrice = 600.00m,
                    RequiresEngineer = true,
                    AdditionalNotes = "Klasična glazba, potreban iskusan sound engineer",
                    ClientId = client1.Id,
                    ProducerId = producer3.Id,
                    StudioRoomId = studio3.Id
                };

                var booking5 = new Booking
                {
                    StartTime = new DateTime(2026, 4, 7, 15, 0, 0),
                    EndTime = new DateTime(2026, 4, 7, 18, 0, 0),
                    CreatedAt = new DateTime(2026, 3, 29),
                    Status = BookingStatus.InProgress,
                    Purpose = "Mixdown - Prvi album",
                    TotalPrice = 360.00m,
                    RequiresEngineer = true,
                    AdditionalNotes = "Heavy rock album, 12 pjesama",
                    ClientId = client2.Id,
                    ProducerId = producer2.Id,
                    StudioRoomId = studio1.Id
                };

                var booking6 = new Booking
                {
                    StartTime = new DateTime(2026, 4, 8, 10, 0, 0),
                    EndTime = new DateTime(2026, 4, 8, 13, 0, 0),
                    CreatedAt = new DateTime(2026, 3, 30),
                    Status = BookingStatus.Completed,
                    Purpose = "Post-production podcast serije",
                    TotalPrice = 240.00m,
                    RequiresEngineer = false,
                    AdditionalNotes = "Audio editiranje i mastering",
                    ClientId = client3.Id,
                    ProducerId = producer1.Id,
                    StudioRoomId = studio2.Id
                };

                context.Bookings.AddRange(booking1, booking2, booking3, booking4, booking5, booking6);
                context.SaveChanges();

                // ============== AUDIO PROJEKTI ==============
                var project1 = new AudioProject
                {
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
                    ClientId = client1.Id,
                    ProducerId = producer1.Id
                };

                var project2 = new AudioProject
                {
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
                    ClientId = client2.Id,
                    ProducerId = producer2.Id
                };

                var project3 = new AudioProject
                {
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
                    ClientId = client3.Id,
                    ProducerId = producer1.Id
                };

                context.AudioProjects.AddRange(project1, project2, project3);
                context.SaveChanges();

                // ============== VERZIJE PROJEKATA ==============
                var project1_v1 = new ProjectVersion
                {
                    ProjectId = project1.Id,
                    VersionNumber = 1,
                    Name = "First Draft",
                    Description = "Prva verzija - raw recordings",
                    CreatedAt = new DateTime(2026, 3, 10),
                    DurationSeconds = 238,
                    FileSize = 150.5m,
                    FileUrl = "https://storage.com/marina-dreams-v1.wav",
                    Notes = "Potrebna mastering sesija",
                    IsApproved = false
                };

                var project1_v2 = new ProjectVersion
                {
                    ProjectId = project1.Id,
                    VersionNumber = 2,
                    Name = "Mixed Version",
                    Description = "Verzija nakon miksinga",
                    CreatedAt = new DateTime(2026, 3, 20),
                    DurationSeconds = 240,
                    FileSize = 175.2m,
                    FileUrl = "https://storage.com/marina-dreams-v2.wav",
                    Notes = "Mixdown gotov, čeka mastering",
                    IsApproved = false
                };

                var project2_v1 = new ProjectVersion
                {
                    ProjectId = project2.Id,
                    VersionNumber = 1,
                    Name = "Raw Recordings",
                    Description = "Sirove snimke sa sesija",
                    CreatedAt = new DateTime(2026, 2, 25),
                    DurationSeconds = 3580,
                    FileSize = 2200.0m,
                    FileUrl = "https://storage.com/heavy-thunder-v1.wav",
                    Notes = "Sve 12 pjesama snimljeno",
                    IsApproved = false
                };

                var project2_v2 = new ProjectVersion
                {
                    ProjectId = project2.Id,
                    VersionNumber = 2,
                    Name = "Mixed - First Pass",
                    Description = "Prvi mixdown - svi kanali",
                    CreatedAt = new DateTime(2026, 3, 15),
                    DurationSeconds = 3600,
                    FileSize = 2250.5m,
                    FileUrl = "https://storage.com/heavy-thunder-v2.wav",
                    Notes = "Čeka povratne informacije klijenta",
                    IsApproved = false
                };

                var project3_v1 = new ProjectVersion
                {
                    ProjectId = project3.Id,
                    VersionNumber = 1,
                    Name = "Raw Podcast Feed",
                    Description = "Sirove snimke - 6 epizoda",
                    CreatedAt = new DateTime(2026, 2, 10),
                    DurationSeconds = 1780,
                    FileSize = 800.0m,
                    FileUrl = "https://storage.com/business-talk-v1.wav",
                    Notes = "Trebalo editiranje i normalizacija",
                    IsApproved = false
                };

                var project3_v2 = new ProjectVersion
                {
                    ProjectId = project3.Id,
                    VersionNumber = 2,
                    Name = "Edited & Normalized",
                    Description = "Nakon editiranja i normalizacije",
                    CreatedAt = new DateTime(2026, 3, 5),
                    DurationSeconds = 1800,
                    FileSize = 820.0m,
                    FileUrl = "https://storage.com/business-talk-v2.wav",
                    Notes = "Revizije requested - trebaju izmjene na epizodi 3",
                    IsApproved = false
                };

                context.ProjectVersions.AddRange(project1_v1, project1_v2, project2_v1, project2_v2, project3_v1, project3_v2);
                context.SaveChanges();

                // ============== KOMENTARI ==============
                var comment1_1 = new TimecodedComment
                {
                    TimestampSeconds = 45.5m,
                    Message = "Previše reverba na vokalima - trebalo bi malo suhije",
                    CreatedAt = new DateTime(2026, 3, 12),
                    IsResolved = true,
                    Category = "Vocals",
                    IsInternalNote = false,
                    ProjectVersionId = project1_v1.Id,
                    AuthorId = producer1.Id
                };

                var comment1_2 = new TimecodedComment
                {
                    TimestampSeconds = 120.0m,
                    Message = "Drop je fantastičan! Svida mi se ovo.",
                    CreatedAt = new DateTime(2026, 3, 12),
                    IsResolved = false,
                    Category = "Production",
                    IsInternalNote = false,
                    ProjectVersionId = project1_v1.Id,
                    AuthorId = client1.Id
                };

                var comment2_1 = new TimecodedComment
                {
                    TimestampSeconds = 45.5m,
                    Message = "Vokal je sada mnogo bolje balanciran s ostatkom mixa",
                    CreatedAt = new DateTime(2026, 3, 22),
                    IsResolved = true,
                    Category = "Vocals",
                    IsInternalNote = true,
                    ProjectVersionId = project1_v2.Id,
                    AuthorId = producer1.Id
                };

                context.TimecodedComments.AddRange(comment1_1, comment1_2, comment2_1);
                context.SaveChanges();
            }
        }
    }
}

using System.Security.Claims;
using System.Text.Encodings.Web;
using AudioProductionManagement.Model;
using LS_Projekt_ASP_2026.Controllers;
using LS_Projekt_ASP_2026.Data;
using LS_Projekt_ASP_2026.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LS_Projekt_ASP_2026.Tests.Integration;

public sealed class ApiTestApplicationFactory : IDisposable
{
    private readonly IHost _host;

    public ApiTestApplicationFactory()
    {
        var databaseName = $"api-tests-{Guid.NewGuid()}";

        _host = new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder.UseTestServer();
                webBuilder.ConfigureServices(services =>
                {
                    services.AddControllers()
                        .AddApplicationPart(typeof(ApiClientsController).Assembly);

                    services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(databaseName));

                    services.AddAuthentication(TestAuthHandler.SchemeName)
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });
                    services.PostConfigure<AuthenticationOptions>(options =>
                    {
                        options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                        options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
                        options.DefaultForbidScheme = TestAuthHandler.SchemeName;
                    });

                    services.AddAuthorization();
                });

                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseAuthentication();
                    app.UseAuthorization();
                    app.UseEndpoints(endpoints => endpoints.MapControllers());
                });
            })
            .Start();

        using var scope = _host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
        Seed(context);
    }

    public HttpClient CreateClient()
    {
        return _host.GetTestClient();
    }

    public void Dispose()
    {
        _host.Dispose();
    }

    private static void Seed(AppDbContext context)
    {
        var client = new Client
        {
            Id = 1,
            Name = "Test",
            Surname = "Client",
            Email = "client@test.local",
            PhoneNumber = "+385 1 111 1111",
            CreatedAt = DateTime.UtcNow.AddDays(-10),
            Role = UserRole.Client,
            DateOfBirth = new DateTime(1995, 1, 1),
            Address = "Client Street 1",
            Country = "Hrvatska",
            CompanyName = "Client Co",
            BillingAddress = "Client Street 1",
            IsPriorityClient = true,
            Notes = "Seed client"
        };

        var producer = new Producer
        {
            Id = 2,
            Name = "Test",
            Surname = "Producer",
            Email = "producer@test.local",
            PhoneNumber = "+385 1 222 2222",
            CreatedAt = DateTime.UtcNow.AddDays(-9),
            Role = UserRole.Producer,
            Specialization = "Mix",
            HourlyRate = 120,
            IsExternalCollaborator = false,
            Biography = "Seed producer"
        };

        var room = new StudioRoom
        {
            Id = 3,
            Name = "Seed Room",
            Location = "Floor 1",
            Capacity = 4,
            HasVocalBooth = true,
            HasAnalogGear = false,
            HourlyPrice = 80,
            EquipmentSummary = "Seed equipment"
        };

        var booking = new Booking
        {
            Id = 4,
            StartTime = DateTime.UtcNow.AddDays(2),
            EndTime = DateTime.UtcNow.AddDays(2).AddHours(2),
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            Status = BookingStatus.Pending,
            Purpose = "Seed booking",
            TotalPrice = 160,
            RequiresEngineer = true,
            AdditionalNotes = "Seed notes",
            ClientId = client.Id,
            ProducerId = producer.Id,
            StudioRoomId = room.Id
        };

        var project = new AudioProject
        {
            Id = 5,
            Title = "Seed Project",
            Type = ProjectType.Single,
            Status = ProjectStatus.Active,
            Genre = "Pop",
            TargetDurationSeconds = 180,
            CreatedAt = DateTime.UtcNow.AddDays(-5),
            Deadline = DateTime.UtcNow.AddDays(20),
            Budget = 1000,
            AllowClientComments = true,
            SharedFolderUrl = "https://example.test/seed",
            ClientId = client.Id,
            ProducerId = producer.Id,
            StudioRoomId = room.Id
        };

        var version = new ProjectVersion
        {
            Id = 6,
            ProjectId = project.Id,
            VersionNumber = 1,
            Name = "Seed Version",
            Description = "Seed version description",
            CreatedAt = DateTime.UtcNow.AddDays(-4),
            DurationSeconds = 180,
            FileSize = 12.5m,
            FileUrl = "https://example.test/audio.wav",
            Notes = "Seed version notes",
            IsApproved = false
        };

        var comment = new TimecodedComment
        {
            Id = 7,
            TimestampSeconds = 12.3m,
            Message = "Seed comment",
            CreatedAt = DateTime.UtcNow.AddDays(-3),
            IsResolved = false,
            Category = "Mix",
            IsInternalNote = false,
            ProjectVersionId = version.Id,
            AuthorId = producer.Id
        };

        context.Clients.Add(client);
        context.Producers.Add(producer);
        context.StudioRooms.Add(room);
        context.Bookings.Add(booking);
        context.AudioProjects.Add(project);
        context.ProjectVersions.Add(version);
        context.TimecodedComments.Add(comment);
        context.SaveChanges();
    }
}

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "Test";

    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "100"),
            new Claim(ClaimTypes.Name, "Api Test Admin"),
            new Claim(ClaimTypes.Email, "admin@test.local"),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(AuthClaimTypes.FullName, "Api Test Admin"),
            new Claim(AuthClaimTypes.BusinessUserId, "2")
        };

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

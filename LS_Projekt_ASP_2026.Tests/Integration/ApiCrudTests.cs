using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using AudioProductionManagement.Model;
using Xunit;

namespace LS_Projekt_ASP_2026.Tests.Integration;

public class ApiCrudTests
{
    [Fact]
    public async Task Clients_endpoint_supports_full_crud()
    {
        using var factory = new ApiTestApplicationFactory();
        var client = factory.CreateClient();

        await AssertListEndpoint(client, "/api/v1/clients?q=Test");

        var id = await PostAndReadId(client, "/api/v1/clients", new
        {
            name = "Api",
            surname = "Client",
            email = $"api-client-{Guid.NewGuid():N}@test.local",
            phoneNumber = "+385 1 300 3000",
            password = "password123",
            dateOfBirth = new DateTime(1990, 2, 3),
            address = "Api Street 1",
            country = "Hrvatska",
            companyName = "Api Client Co",
            billingAddress = "Api Street 1",
            isPriorityClient = false,
            notes = "Created from integration test"
        });

        await AssertGetById(client, $"/api/v1/clients/{id}", id);

        var update = await client.PutAsJsonAsync($"/api/v1/clients/{id}", new
        {
            name = "Api Updated",
            surname = "Client",
            email = $"api-client-updated-{Guid.NewGuid():N}@test.local",
            phoneNumber = "+385 1 300 3001",
            password = "",
            dateOfBirth = new DateTime(1990, 2, 3),
            address = "Updated Street 1",
            country = "Hrvatska",
            companyName = "Updated Co",
            billingAddress = "Updated Street 1",
            isPriorityClient = true,
            notes = "Updated from integration test"
        });
        update.EnsureSuccessStatusCode();

        await DeleteAndAssertGone(client, $"/api/v1/clients/{id}");
    }

    [Fact]
    public async Task Clients_endpoint_returns_404_and_validates_input()
    {
        using var factory = new ApiTestApplicationFactory();
        var client = factory.CreateClient();

        await AssertMissingAndInvalidCreate(client, "/api/v1/clients", "/api/v1/clients/999999", new
        {
            name = "",
            surname = "",
            email = "not-an-email",
            phoneNumber = "",
            password = "",
            dateOfBirth = DateTime.MinValue,
            address = "",
            country = "",
            companyName = "",
            billingAddress = "",
            isPriorityClient = false,
            notes = ""
        }, new
        {
            name = "Missing",
            surname = "Client",
            email = $"missing-client-{Guid.NewGuid():N}@test.local",
            phoneNumber = "+385 1 300 3009",
            password = "password123",
            dateOfBirth = new DateTime(1990, 2, 3),
            address = "Missing Street 1",
            country = "Hrvatska",
            companyName = "Missing Co",
            billingAddress = "Missing Street 1",
            isPriorityClient = false,
            notes = "Missing update payload"
        });
    }

    [Fact]
    public async Task Producers_endpoint_supports_full_crud()
    {
        using var factory = new ApiTestApplicationFactory();
        var client = factory.CreateClient();

        await AssertListEndpoint(client, "/api/v1/producers?q=Test");

        var id = await PostAndReadId(client, "/api/v1/producers", new
        {
            name = "Api",
            surname = "Producer",
            email = $"api-producer-{Guid.NewGuid():N}@test.local",
            phoneNumber = "+385 1 400 4000",
            password = "password123",
            specialization = "Mastering",
            hourlyRate = 150m,
            isExternalCollaborator = false,
            biography = "Created from integration test"
        });

        await AssertGetById(client, $"/api/v1/producers/{id}", id);

        var update = await client.PutAsJsonAsync($"/api/v1/producers/{id}", new
        {
            name = "Api Updated",
            surname = "Producer",
            email = $"api-producer-updated-{Guid.NewGuid():N}@test.local",
            phoneNumber = "+385 1 400 4001",
            password = "",
            specialization = "Mixing",
            hourlyRate = 175m,
            isExternalCollaborator = true,
            biography = "Updated from integration test"
        });
        update.EnsureSuccessStatusCode();

        await DeleteAndAssertGone(client, $"/api/v1/producers/{id}");
    }

    [Fact]
    public async Task Producers_endpoint_returns_404_and_validates_input()
    {
        using var factory = new ApiTestApplicationFactory();
        var client = factory.CreateClient();

        await AssertMissingAndInvalidCreate(client, "/api/v1/producers", "/api/v1/producers/999999", new
        {
            name = "",
            surname = "",
            email = "not-an-email",
            phoneNumber = "",
            password = "",
            specialization = "",
            hourlyRate = 0m,
            isExternalCollaborator = false,
            biography = ""
        }, new
        {
            name = "Missing",
            surname = "Producer",
            email = $"missing-producer-{Guid.NewGuid():N}@test.local",
            phoneNumber = "+385 1 400 4009",
            password = "password123",
            specialization = "Mixing",
            hourlyRate = 175m,
            isExternalCollaborator = true,
            biography = "Missing update payload"
        });
    }

    [Fact]
    public async Task Studio_rooms_endpoint_supports_full_crud()
    {
        using var factory = new ApiTestApplicationFactory();
        var client = factory.CreateClient();

        await AssertListEndpoint(client, "/api/v1/studio-rooms?q=Seed");

        var id = await PostAndReadId(client, "/api/v1/studio-rooms", new
        {
            name = "Api Room",
            location = "Floor 2",
            capacity = 8,
            hasVocalBooth = true,
            hasAnalogGear = true,
            hourlyPrice = 120m,
            equipmentSummary = "Created from integration test"
        });

        await AssertGetById(client, $"/api/v1/studio-rooms/{id}", id);

        var update = await client.PutAsJsonAsync($"/api/v1/studio-rooms/{id}", new
        {
            name = "Api Room Updated",
            location = "Floor 3",
            capacity = 10,
            hasVocalBooth = false,
            hasAnalogGear = true,
            hourlyPrice = 140m,
            equipmentSummary = "Updated from integration test"
        });
        update.EnsureSuccessStatusCode();

        await DeleteAndAssertGone(client, $"/api/v1/studio-rooms/{id}");
    }

    [Fact]
    public async Task Studio_rooms_endpoint_returns_404_and_validates_input()
    {
        using var factory = new ApiTestApplicationFactory();
        var client = factory.CreateClient();

        await AssertMissingAndInvalidCreate(client, "/api/v1/studio-rooms", "/api/v1/studio-rooms/999999", new
        {
            name = "",
            location = "",
            capacity = 0,
            hasVocalBooth = false,
            hasAnalogGear = false,
            hourlyPrice = 0m,
            equipmentSummary = ""
        }, new
        {
            name = "Missing Room",
            location = "Floor 9",
            capacity = 12,
            hasVocalBooth = true,
            hasAnalogGear = true,
            hourlyPrice = 150m,
            equipmentSummary = "Missing update payload"
        });
    }

    [Fact]
    public async Task Bookings_endpoint_supports_full_crud()
    {
        using var factory = new ApiTestApplicationFactory();
        var client = factory.CreateClient();

        await AssertListEndpoint(client, "/api/v1/bookings?q=Seed");

        var start = DateTime.UtcNow.AddDays(7);
        var id = await PostAndReadId(client, "/api/v1/bookings", new
        {
            startTime = start,
            endTime = start.AddHours(3),
            status = BookingStatus.Confirmed,
            purpose = "Api booking",
            totalPrice = 240m,
            requiresEngineer = true,
            additionalNotes = "Created from integration test",
            clientId = 1,
            producerId = 2,
            studioRoomId = 3
        });

        await AssertGetById(client, $"/api/v1/bookings/{id}", id);

        var update = await client.PutAsJsonAsync($"/api/v1/bookings/{id}", new
        {
            startTime = start.AddDays(1),
            endTime = start.AddDays(1).AddHours(2),
            status = BookingStatus.Completed,
            purpose = "Api booking updated",
            totalPrice = 200m,
            requiresEngineer = false,
            additionalNotes = "Updated from integration test",
            clientId = 1,
            producerId = 2,
            studioRoomId = 3
        });
        update.EnsureSuccessStatusCode();

        await DeleteAndAssertGone(client, $"/api/v1/bookings/{id}");
    }

    [Fact]
    public async Task Bookings_endpoint_returns_404_and_validates_input()
    {
        using var factory = new ApiTestApplicationFactory();
        var client = factory.CreateClient();

        await AssertMissingAndInvalidCreate(client, "/api/v1/bookings", "/api/v1/bookings/999999", new
        {
            startTime = DateTime.UtcNow.AddHours(1),
            endTime = DateTime.UtcNow,
            status = BookingStatus.Pending,
            purpose = "",
            totalPrice = 0m,
            requiresEngineer = false,
            additionalNotes = "",
            clientId = 999999,
            producerId = 999999,
            studioRoomId = 999999
        }, new
        {
            startTime = DateTime.UtcNow.AddDays(2),
            endTime = DateTime.UtcNow.AddDays(2).AddHours(2),
            status = BookingStatus.Confirmed,
            purpose = "Missing booking",
            totalPrice = 100m,
            requiresEngineer = false,
            additionalNotes = "Missing update payload",
            clientId = 1,
            producerId = 2,
            studioRoomId = 3
        });
    }

    [Fact]
    public async Task Projects_endpoint_supports_full_crud()
    {
        using var factory = new ApiTestApplicationFactory();
        var client = factory.CreateClient();

        await AssertListEndpoint(client, "/api/v1/projects?q=Seed");

        var id = await PostAndReadId(client, "/api/v1/projects", new
        {
            title = "Api Project",
            type = ProjectType.EP,
            status = ProjectStatus.Draft,
            genre = "Rock",
            targetDurationSeconds = 900,
            deadline = DateTime.UtcNow.AddDays(30),
            budget = 2500m,
            allowClientComments = true,
            sharedFolderUrl = "https://example.test/api-project",
            clientId = 1,
            producerId = 2,
            studioRoomId = 3
        });

        await AssertGetById(client, $"/api/v1/projects/{id}", id);

        var update = await client.PutAsJsonAsync($"/api/v1/projects/{id}", new
        {
            title = "Api Project Updated",
            type = ProjectType.Album,
            status = ProjectStatus.Active,
            genre = "Alternative",
            targetDurationSeconds = 1800,
            deadline = DateTime.UtcNow.AddDays(45),
            budget = 3500m,
            allowClientComments = false,
            sharedFolderUrl = "https://example.test/api-project-updated",
            clientId = 1,
            producerId = 2,
            studioRoomId = 3
        });
        update.EnsureSuccessStatusCode();

        await DeleteAndAssertGone(client, $"/api/v1/projects/{id}");
    }

    [Fact]
    public async Task Projects_endpoint_returns_404_and_validates_input()
    {
        using var factory = new ApiTestApplicationFactory();
        var client = factory.CreateClient();

        await AssertMissingAndInvalidCreate(client, "/api/v1/projects", "/api/v1/projects/999999", new
        {
            title = "",
            type = ProjectType.EP,
            status = ProjectStatus.Draft,
            genre = "",
            targetDurationSeconds = 0,
            deadline = DateTime.UtcNow.AddDays(1),
            budget = 0m,
            allowClientComments = true,
            sharedFolderUrl = "",
            clientId = 999999,
            producerId = 999999,
            studioRoomId = 999999
        }, new
        {
            title = "Missing Project",
            type = ProjectType.Album,
            status = ProjectStatus.Active,
            genre = "Alternative",
            targetDurationSeconds = 1800,
            deadline = DateTime.UtcNow.AddDays(45),
            budget = 3500m,
            allowClientComments = false,
            sharedFolderUrl = "https://example.test/missing-project",
            clientId = 1,
            producerId = 2,
            studioRoomId = 3
        });
    }

    [Fact]
    public async Task Project_versions_endpoint_supports_full_crud()
    {
        using var factory = new ApiTestApplicationFactory();
        var client = factory.CreateClient();

        await AssertListEndpoint(client, "/api/v1/project-versions?q=Seed");

        var id = await PostAndReadId(client, "/api/v1/project-versions", new
        {
            projectId = 5,
            name = "Api Version",
            description = "Created from integration test",
            durationSeconds = 210,
            fileSize = 20.5m,
            fileUrl = "https://example.test/api-version.wav",
            notes = "Api version notes",
            isApproved = false
        });

        await AssertGetById(client, $"/api/v1/project-versions/{id}", id);

        var update = await client.PutAsJsonAsync($"/api/v1/project-versions/{id}", new
        {
            name = "Api Version Updated",
            description = "Updated from integration test",
            durationSeconds = 220,
            fileSize = 22.5m,
            fileUrl = "https://example.test/api-version-updated.wav",
            notes = "Updated version notes",
            isApproved = true
        });
        update.EnsureSuccessStatusCode();

        await DeleteAndAssertGone(client, $"/api/v1/project-versions/{id}");
    }

    [Fact]
    public async Task Project_versions_endpoint_returns_404_and_validates_input()
    {
        using var factory = new ApiTestApplicationFactory();
        var client = factory.CreateClient();

        await AssertMissingAndInvalidCreate(client, "/api/v1/project-versions", "/api/v1/project-versions/999999", new
        {
            projectId = 999999,
            name = "",
            description = "",
            durationSeconds = 0,
            fileSize = 0m,
            fileUrl = "",
            notes = "",
            isApproved = false
        }, new
        {
            name = "Missing Version",
            description = "Missing update payload",
            durationSeconds = 220,
            fileSize = 22.5m,
            fileUrl = "https://example.test/missing-version.wav",
            notes = "Updated version notes",
            isApproved = true
        });
    }

    [Fact]
    public async Task Comments_endpoint_supports_full_crud()
    {
        using var factory = new ApiTestApplicationFactory();
        var client = factory.CreateClient();

        await AssertListEndpoint(client, "/api/v1/comments?q=Seed");

        var id = await PostAndReadId(client, "/api/v1/comments", new
        {
            timestampSeconds = 32.5m,
            message = "Api comment",
            isResolved = false,
            category = "Arrangement",
            isInternalNote = false,
            projectVersionId = 6,
            authorId = 2
        });

        await AssertGetById(client, $"/api/v1/comments/{id}", id);

        var update = await client.PutAsJsonAsync($"/api/v1/comments/{id}", new
        {
            timestampSeconds = 40.5m,
            message = "Api comment updated",
            isResolved = true,
            category = "Mix",
            isInternalNote = true,
            projectVersionId = 6,
            authorId = 2
        });
        update.EnsureSuccessStatusCode();

        await DeleteAndAssertGone(client, $"/api/v1/comments/{id}");
    }

    [Fact]
    public async Task Comments_endpoint_returns_404_and_validates_input()
    {
        using var factory = new ApiTestApplicationFactory();
        var client = factory.CreateClient();

        await AssertMissingAndInvalidCreate(client, "/api/v1/comments", "/api/v1/comments/999999", new
        {
            timestampSeconds = 0m,
            message = "",
            isResolved = false,
            category = "",
            isInternalNote = false,
            projectVersionId = 999999,
            authorId = 999999
        }, new
        {
            timestampSeconds = 40.5m,
            message = "Missing comment",
            isResolved = true,
            category = "Mix",
            isInternalNote = true,
            projectVersionId = 6,
            authorId = 2
        });
    }

    private static async Task AssertListEndpoint(HttpClient client, string url)
    {
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await ReadJson(response);
        Assert.NotNull(json["count"]);
        Assert.NotNull(json["data"]);
    }

    private static async Task<int> PostAndReadId(HttpClient client, string url, object payload)
    {
        var response = await client.PostAsJsonAsync(url, payload);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var json = await ReadJson(response);
        var id = json["id"]?.GetValue<int>() ?? 0;
        Assert.True(id > 0);
        return id;
    }

    private static async Task AssertGetById(HttpClient client, string url, int expectedId)
    {
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await ReadJson(response);
        Assert.Equal(expectedId, json["id"]?.GetValue<int>());
    }

    private static async Task DeleteAndAssertGone(HttpClient client, string url)
    {
        var delete = await client.DeleteAsync(url);
        Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);

        var getDeleted = await client.GetAsync(url);
        Assert.Equal(HttpStatusCode.NotFound, getDeleted.StatusCode);
    }

    private static async Task<JsonNode> ReadJson(HttpResponseMessage response)
    {
        var json = await response.Content.ReadFromJsonAsync<JsonNode>();
        Assert.NotNull(json);
        return json!;
    }

    private static async Task AssertMissingAndInvalidCreate(HttpClient client, string createUrl, string missingUrl, object invalidPayload, object validUpdatePayload)
    {
        var missingGet = await client.GetAsync(missingUrl);
        Assert.Equal(HttpStatusCode.NotFound, missingGet.StatusCode);

        var missingPut = await client.PutAsJsonAsync(missingUrl, validUpdatePayload);
        Assert.Equal(HttpStatusCode.NotFound, missingPut.StatusCode);

        var invalidPost = await client.PostAsJsonAsync(createUrl, invalidPayload);
        Assert.Equal(HttpStatusCode.BadRequest, invalidPost.StatusCode);
    }
}

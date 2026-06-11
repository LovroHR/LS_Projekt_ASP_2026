using AudioProductionManagement.Model;

namespace LS_Projekt_ASP_2026.Api;

public static class ApiMappings
{
    public static UserSummaryDto ToSummaryDto(this AppUser user)
    {
        return new UserSummaryDto(user.Id, user.Name, user.Surname, user.Email, user.PhoneNumber, user.Role);
    }

    public static ClientDto ToDto(this Client client)
    {
        return new ClientDto(
            client.Id,
            client.Name,
            client.Surname,
            client.Email,
            client.PhoneNumber,
            client.CreatedAt,
            client.DateOfBirth,
            client.Address,
            client.Country,
            client.CompanyName,
            client.BillingAddress,
            client.IsPriorityClient,
            client.Notes);
    }

    public static ProducerDto ToDto(this Producer producer)
    {
        return new ProducerDto(
            producer.Id,
            producer.Name,
            producer.Surname,
            producer.Email,
            producer.PhoneNumber,
            producer.CreatedAt,
            producer.Specialization,
            producer.HourlyRate,
            producer.IsExternalCollaborator,
            producer.Biography);
    }

    public static StudioRoomSummaryDto ToSummaryDto(this StudioRoom room)
    {
        return new StudioRoomSummaryDto(room.Id, room.Name, room.Location, room.Capacity);
    }

    public static StudioRoomDto ToDto(this StudioRoom room)
    {
        return new StudioRoomDto(
            room.Id,
            room.Name,
            room.Location,
            room.Capacity,
            room.HasVocalBooth,
            room.HasAnalogGear,
            room.HourlyPrice,
            room.EquipmentSummary);
    }

    public static BookingDto ToDto(this Booking booking)
    {
        return new BookingDto(
            booking.Id,
            booking.StartTime,
            booking.EndTime,
            booking.CreatedAt,
            booking.Status,
            booking.Purpose,
            booking.TotalPrice,
            booking.RequiresEngineer,
            booking.AdditionalNotes,
            booking.Client.ToSummaryDto(),
            booking.Producer.ToSummaryDto(),
            booking.StudioRoom.ToSummaryDto());
    }

    public static AudioProjectSummaryDto ToSummaryDto(this AudioProject project)
    {
        return new AudioProjectSummaryDto(project.Id, project.Title, project.Type, project.Status);
    }

    public static AudioProjectDto ToDto(this AudioProject project, bool includeVersions = true)
    {
        return new AudioProjectDto(
            project.Id,
            project.Title,
            project.Type,
            project.Status,
            project.Genre,
            project.TargetDurationSeconds,
            project.CreatedAt,
            project.Deadline,
            project.Budget,
            project.AllowClientComments,
            project.SharedFolderUrl,
            project.Client.ToSummaryDto(),
            project.Producer.ToSummaryDto(),
            project.StudioRoom?.ToSummaryDto(),
            includeVersions
                ? project.Versions.OrderByDescending(v => v.VersionNumber).Select(v => v.ToDto(false))
                : Array.Empty<ProjectVersionDto>());
    }

    public static ProjectVersionDto ToDto(this ProjectVersion version, bool includeComments = true)
    {
        return new ProjectVersionDto(
            version.Id,
            version.ProjectId,
            version.VersionNumber,
            version.Name,
            version.Description,
            version.CreatedAt,
            version.UpdatedAt,
            version.DurationSeconds,
            version.FileSize,
            version.FileUrl,
            version.Notes,
            version.IsApproved,
            includeComments
                ? version.Comments.OrderBy(c => c.TimestampSeconds).Select(c => c.ToDto())
                : Array.Empty<TimecodedCommentDto>());
    }

    public static TimecodedCommentDto ToDto(this TimecodedComment comment)
    {
        return new TimecodedCommentDto(
            comment.Id,
            comment.TimestampSeconds,
            comment.Message,
            comment.CreatedAt,
            comment.IsResolved,
            comment.Category,
            comment.IsInternalNote,
            comment.ProjectVersionId,
            comment.Author.ToSummaryDto());
    }
}

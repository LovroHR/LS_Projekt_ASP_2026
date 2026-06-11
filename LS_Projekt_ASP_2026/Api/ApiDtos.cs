using AudioProductionManagement.Model;
using System.ComponentModel.DataAnnotations;

namespace LS_Projekt_ASP_2026.Api;

public record ApiListResponse<T>(int Count, IEnumerable<T> Data);

public record UserSummaryDto(int Id, string Name, string Surname, string Email, string PhoneNumber, UserRole Role);

public record ClientDto(
    int Id,
    string Name,
    string Surname,
    string Email,
    string PhoneNumber,
    DateTime CreatedAt,
    DateTime DateOfBirth,
    string Address,
    string Country,
    string CompanyName,
    string BillingAddress,
    bool IsPriorityClient,
    string Notes);

public record ClientCreateDto(
    [Required, MaxLength(100)] string Name,
    [Required, MaxLength(100)] string Surname,
    [Required, EmailAddress, MaxLength(255)] string Email,
    [Required, MaxLength(30)] string PhoneNumber,
    string? Password,
    DateTime DateOfBirth,
    [MaxLength(255)] string Address,
    [MaxLength(100)] string Country,
    [MaxLength(150)] string CompanyName,
    [MaxLength(255)] string BillingAddress,
    bool IsPriorityClient,
    [MaxLength(2000)] string Notes);

public record ClientUpdateDto(
    [Required, MaxLength(100)] string Name,
    [Required, MaxLength(100)] string Surname,
    [Required, EmailAddress, MaxLength(255)] string Email,
    [Required, MaxLength(30)] string PhoneNumber,
    string? Password,
    DateTime DateOfBirth,
    [MaxLength(255)] string Address,
    [MaxLength(100)] string Country,
    [MaxLength(150)] string CompanyName,
    [MaxLength(255)] string BillingAddress,
    bool IsPriorityClient,
    [MaxLength(2000)] string Notes);

public record ProducerDto(
    int Id,
    string Name,
    string Surname,
    string Email,
    string PhoneNumber,
    DateTime CreatedAt,
    string Specialization,
    decimal HourlyRate,
    bool IsExternalCollaborator,
    string Biography);

public record ProducerCreateDto(
    [Required, MaxLength(100)] string Name,
    [Required, MaxLength(100)] string Surname,
    [Required, EmailAddress, MaxLength(255)] string Email,
    [Required, MaxLength(30)] string PhoneNumber,
    string? Password,
    [MaxLength(120)] string Specialization,
    decimal HourlyRate,
    bool IsExternalCollaborator,
    [MaxLength(3000)] string Biography);

public record ProducerUpdateDto(
    [Required, MaxLength(100)] string Name,
    [Required, MaxLength(100)] string Surname,
    [Required, EmailAddress, MaxLength(255)] string Email,
    [Required, MaxLength(30)] string PhoneNumber,
    string? Password,
    [MaxLength(120)] string Specialization,
    decimal HourlyRate,
    bool IsExternalCollaborator,
    [MaxLength(3000)] string Biography);

public record StudioRoomSummaryDto(int Id, string Name, string Location, int Capacity);

public record StudioRoomDto(
    int Id,
    string Name,
    string Location,
    int Capacity,
    bool HasVocalBooth,
    bool HasAnalogGear,
    decimal HourlyPrice,
    string EquipmentSummary);

public record StudioRoomCreateDto(
    [Required, MaxLength(120)] string Name,
    [Required, MaxLength(120)] string Location,
    int Capacity,
    bool HasVocalBooth,
    bool HasAnalogGear,
    decimal HourlyPrice,
    [MaxLength(2000)] string EquipmentSummary);

public record StudioRoomUpdateDto(
    [Required, MaxLength(120)] string Name,
    [Required, MaxLength(120)] string Location,
    int Capacity,
    bool HasVocalBooth,
    bool HasAnalogGear,
    decimal HourlyPrice,
    [MaxLength(2000)] string EquipmentSummary);

public record BookingDto(
    int Id,
    DateTime StartTime,
    DateTime EndTime,
    DateTime CreatedAt,
    BookingStatus Status,
    string Purpose,
    decimal TotalPrice,
    bool RequiresEngineer,
    string AdditionalNotes,
    UserSummaryDto Client,
    UserSummaryDto Producer,
    StudioRoomSummaryDto StudioRoom);

public record BookingCreateDto(
    DateTime StartTime,
    DateTime EndTime,
    BookingStatus Status,
    [Required, MaxLength(300)] string Purpose,
    decimal TotalPrice,
    bool RequiresEngineer,
    [MaxLength(2000)] string AdditionalNotes,
    int ClientId,
    int ProducerId,
    int StudioRoomId);

public record BookingUpdateDto(
    DateTime StartTime,
    DateTime EndTime,
    BookingStatus Status,
    [Required, MaxLength(300)] string Purpose,
    decimal TotalPrice,
    bool RequiresEngineer,
    [MaxLength(2000)] string AdditionalNotes,
    int ClientId,
    int ProducerId,
    int StudioRoomId);

public record AudioProjectSummaryDto(int Id, string Title, ProjectType Type, ProjectStatus Status);

public record AudioProjectDto(
    int Id,
    string Title,
    ProjectType Type,
    ProjectStatus Status,
    string Genre,
    int TargetDurationSeconds,
    DateTime CreatedAt,
    DateTime? Deadline,
    decimal Budget,
    bool AllowClientComments,
    string SharedFolderUrl,
    UserSummaryDto Client,
    UserSummaryDto Producer,
    StudioRoomSummaryDto? StudioRoom,
    IEnumerable<ProjectVersionDto> Versions);

public record AudioProjectCreateDto(
    [Required, MaxLength(200)] string Title,
    ProjectType Type,
    ProjectStatus Status,
    [MaxLength(80)] string Genre,
    int TargetDurationSeconds,
    DateTime? Deadline,
    decimal Budget,
    bool AllowClientComments,
    [MaxLength(500)] string SharedFolderUrl,
    int ClientId,
    int ProducerId,
    int? StudioRoomId);

public record AudioProjectUpdateDto(
    [Required, MaxLength(200)] string Title,
    ProjectType Type,
    ProjectStatus Status,
    [MaxLength(80)] string Genre,
    int TargetDurationSeconds,
    DateTime? Deadline,
    decimal Budget,
    bool AllowClientComments,
    [MaxLength(500)] string SharedFolderUrl,
    int ClientId,
    int ProducerId,
    int? StudioRoomId);

public record ProjectVersionDto(
    int Id,
    int ProjectId,
    int VersionNumber,
    string Name,
    string Description,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    int DurationSeconds,
    decimal FileSize,
    string FileUrl,
    string Notes,
    bool IsApproved,
    IEnumerable<TimecodedCommentDto> Comments);

public record ProjectVersionCreateDto(
    int ProjectId,
    [Required, MaxLength(200)] string Name,
    [MaxLength(2000)] string Description,
    int DurationSeconds,
    decimal FileSize,
    [Required, MaxLength(500)] string FileUrl,
    [MaxLength(2000)] string Notes,
    bool IsApproved);

public record ProjectVersionUpdateDto(
    [Required, MaxLength(200)] string Name,
    [MaxLength(2000)] string Description,
    int DurationSeconds,
    decimal FileSize,
    [Required, MaxLength(500)] string FileUrl,
    [MaxLength(2000)] string Notes,
    bool IsApproved);

public record TimecodedCommentDto(
    int Id,
    decimal TimestampSeconds,
    string Message,
    DateTime CreatedAt,
    bool IsResolved,
    string Category,
    bool IsInternalNote,
    int ProjectVersionId,
    UserSummaryDto Author);

public record TimecodedCommentCreateDto(
    decimal TimestampSeconds,
    [Required, MaxLength(2000)] string Message,
    bool IsResolved,
    [MaxLength(100)] string Category,
    bool IsInternalNote,
    int ProjectVersionId,
    int AuthorId);

public record TimecodedCommentUpdateDto(
    decimal TimestampSeconds,
    [Required, MaxLength(2000)] string Message,
    bool IsResolved,
    [MaxLength(100)] string Category,
    bool IsInternalNote,
    int ProjectVersionId,
    int AuthorId);

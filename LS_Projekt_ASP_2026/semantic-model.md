# Semanticki DB model

## Pregled modela/tablica
- AppUser (bazna tablica za korisnike; discriminator `Role` za podtipove)
- Client (nasljedjuje AppUser)
- Producer (nasljedjuje AppUser)
- StudioRoom
- Booking
- AudioProject
- ProjectVersion
- TimecodedComment

## Glavna svojstva

### AppUser
- Id (PK)
- Name, Surname
- Email (unique)
- PhoneNumber
- CreatedAt
- Role (UserRole discriminator)

### Client
- Password
- DateOfBirth
- Address, Country, CompanyName, BillingAddress
- IsPriorityClient
- Notes

### Producer
- Specialization
- HourlyRate
- IsExternalCollaborator
- Biography

### StudioRoom
- Id (PK)
- Name
- Location
- Capacity
- HasVocalBooth
- HasAnalogGear
- HourlyPrice
- EquipmentSummary

### Booking
- Id (PK)
- StartTime, EndTime, CreatedAt
- Status (BookingStatus)
- Purpose
- TotalPrice
- RequiresEngineer
- AdditionalNotes
- ClientId (FK)
- ProducerId (FK)
- StudioRoomId (FK)

### AudioProject
- Id (PK)
- Title
- Type (ProjectType)
- Status (ProjectStatus)
- Genre
- TargetDurationSeconds
- CreatedAt
- Deadline
- Budget
- AllowClientComments
- SharedFolderUrl
- ClientId (FK)
- ProducerId (FK)

### ProjectVersion
- Id (PK)
- ProjectId (FK -> AudioProject)
- VersionNumber
- Name
- Description
- CreatedAt, UpdatedAt
- DurationSeconds
- FileSize
- FileUrl
- Notes
- IsApproved
- Unique index: (ProjectId, VersionNumber)

### TimecodedComment
- Id (PK)
- TimestampSeconds
- Message
- CreatedAt
- IsResolved
- Category
- IsInternalNote
- ProjectVersionId (FK -> ProjectVersion)
- AuthorId (FK -> AppUser)

## Veze medju tablicama/klasama
- AppUser (TPH) -> Client, Producer (discriminator `Role`)
- Client 1 - N Booking (ClientId)
- Producer 1 - N Booking (ProducerId)
- StudioRoom 1 - N Booking (StudioRoomId)
- Client 1 - N AudioProject (ClientId)
- Producer 1 - N AudioProject (ProducerId)
- AudioProject 1 - N ProjectVersion (ProjectId)
- ProjectVersion 1 - N TimecodedComment (ProjectVersionId)
- AppUser 1 - N TimecodedComment (AuthorId)

## Enumi
- UserRole: Client, Producer, Admin
- BookingStatus: Pending, Confirmed, InProgress, Completed, Cancelled
- ProjectStatus: Draft, Active, WaitingForFeedback, Revision, Approved, Archived
- ProjectType: Single, EP, Album, Podcast, VoiceOver

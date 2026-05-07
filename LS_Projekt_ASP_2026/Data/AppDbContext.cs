using AudioProductionManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace LS_Projekt_ASP_2026.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Producer> Producers => Set<Producer>();
    public DbSet<StudioRoom> StudioRooms => Set<StudioRoom>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<AudioProject> AudioProjects => Set<AudioProject>();
    public DbSet<ProjectVersion> ProjectVersions => Set<ProjectVersion>();
    public DbSet<TimecodedComment> TimecodedComments => Set<TimecodedComment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Surname).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(255).IsRequired();
            entity.HasIndex(x => x.Email).IsUnique();
            entity.Property(x => x.PhoneNumber).HasMaxLength(30).IsRequired();

            entity
                .HasDiscriminator<UserRole>(x => x.Role)
                .HasValue<Client>(UserRole.Client)
                .HasValue<Producer>(UserRole.Producer);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.Property(x => x.Password).HasMaxLength(255).IsRequired();
            entity.Property(x => x.Address).HasMaxLength(255);
            entity.Property(x => x.Country).HasMaxLength(100);
            entity.Property(x => x.CompanyName).HasMaxLength(150);
            entity.Property(x => x.BillingAddress).HasMaxLength(255);
            entity.Property(x => x.Notes).HasMaxLength(2000);
        });

        modelBuilder.Entity<Producer>(entity =>
        {
            entity.Property(x => x.Specialization).HasMaxLength(120);
            entity.Property(x => x.HourlyRate).HasPrecision(18, 2);
            entity.Property(x => x.Biography).HasMaxLength(3000);
        });

        modelBuilder.Entity<StudioRoom>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Location).HasMaxLength(120).IsRequired();
            entity.Property(x => x.HourlyPrice).HasPrecision(18, 2);
            entity.Property(x => x.EquipmentSummary).HasMaxLength(2000);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Purpose).HasMaxLength(300).IsRequired();
            entity.Property(x => x.TotalPrice).HasPrecision(18, 2);
            entity.Property(x => x.AdditionalNotes).HasMaxLength(2000);

            entity
                .HasOne(x => x.Client)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(x => x.Producer)
                .WithMany(x => x.AssignedBookings)
                .HasForeignKey(x => x.ProducerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(x => x.StudioRoom)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.StudioRoomId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<AudioProject>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Genre).HasMaxLength(80);
            entity.Property(x => x.Budget).HasPrecision(18, 2);
            entity.Property(x => x.SharedFolderUrl).HasMaxLength(500);

            entity
                .HasOne(x => x.Client)
                .WithMany(x => x.Projects)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(x => x.Producer)
                .WithMany(x => x.ManagedProjects)
                .HasForeignKey(x => x.ProducerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProjectVersion>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(2000);
            entity.Property(x => x.FileSize).HasPrecision(18, 2);
            entity.Property(x => x.FileUrl).HasMaxLength(500);
            entity.Property(x => x.Notes).HasMaxLength(2000);

            entity
                .HasOne(x => x.Project)
                .WithMany(x => x.Versions)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new { x.ProjectId, x.VersionNumber }).IsUnique();
        });

        modelBuilder.Entity<TimecodedComment>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TimestampSeconds).HasPrecision(10, 3);
            entity.Property(x => x.Message).HasMaxLength(2000).IsRequired();
            entity.Property(x => x.Category).HasMaxLength(100);

            entity
                .HasOne(x => x.ProjectVersion)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.ProjectVersionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasOne(x => x.Author)
                .WithMany()
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

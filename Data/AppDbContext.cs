using CinemaProj.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaProj.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options){ }

    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Screening> Screenings => Set<Screening>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<ReservationSeat> ReservationSeats => Set<ReservationSeat>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Seat>()
            .HasIndex(s => new { s.EventId, s.SeatNumber })
            .IsUnique();
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(150);
            entity.Property(e => e.Genre)
                .IsRequired()
                .HasMaxLength(100);
            entity.HasMany(e => e.SeatTemplate).WithOne(s => s.Event)
                .HasForeignKey(s => s.EventId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Screenings).WithOne(s => s.Event)
                .HasForeignKey(s => s.EventId).OnDelete(DeleteBehavior.Cascade);

        });
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);
            entity.HasIndex(u => u.Email)
                .IsUnique();
            entity.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(256);
        });
        modelBuilder.Entity<Screening>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.StartsAt)
                .HasColumnType("timestamptz")
                .IsRequired();
            entity.Property(s => s.EndsAt)
                .HasColumnType("timestamptz")
                .IsRequired();
            entity.Property(s => s.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();
            entity.HasIndex(s => new { s.EventId, s.StartsAt });
            entity.HasIndex(s => s.StartsAt);
            entity.HasOne(s => s.Event)
                .WithMany(e => e.Screenings)
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();
            entity.Property(r => r.CreatedAt)
                .HasColumnType("timestamptz").IsRequired();
            entity.Property(r => r.ExpiresAt)
                .HasColumnType("timestamptz").IsRequired();

            entity.HasIndex(r => new { r.ScreeningId, r.Status });

            entity.HasOne(r => r.Screening).WithMany()
                .HasForeignKey(r => r.ScreeningId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(r => r.User).WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ReservationSeat>(entity =>
        {
            entity.HasKey(rs => rs.Id);
            entity.Property(rs => rs.SeatNumber).IsRequired().HasMaxLength(10);
            entity.HasIndex(rs => new { rs.ScreeningId, rs.SeatNumber }).IsUnique(); // anti-double-book
        });
    }
}
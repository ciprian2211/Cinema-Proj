using CinemaProj.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaProj.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options){ }

    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<User> Users => Set<User>();

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
    }
}
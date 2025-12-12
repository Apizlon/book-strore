using UserService.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace UserService.DataAccess;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .ValueGeneratedNever();

            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.Email)
                .HasMaxLength(256);

            // Store enum as string (text) in database, not as integer
            entity.Property(e => e.Role)
                .HasConversion<string>()
                .HasMaxLength(50);

            // Indexes for performance
            entity.HasIndex(e => e.Username)
                .IsUnique();

            entity.HasIndex(e => e.Email)
                .IsUnique();

            entity.HasIndex(e => e.IsActive);
        });
    }
}
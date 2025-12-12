using AuthService.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DataAccess;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    public DbSet<AuthUser> AuthUsers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure AuthUser entity
        modelBuilder.Entity<AuthUser>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .ValueGeneratedNever();

            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.PasswordHash)
                .IsRequired();

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("NOW()");

            // Indexes for performance
            entity.HasIndex(e => e.Username)
                .IsUnique();

            entity.Property(e => e.Role)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.HasIndex(e => e.IsActive);
        });
    }
}
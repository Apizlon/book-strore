using BookService.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BookService.DataAccess;

public class BookDbContext : DbContext
{
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; } = null!;
    public DbSet<CartItem> CartItems { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;

    public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Author
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
            entity.Property(e => e.Bio).HasMaxLength(1000);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Configure Book
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(512);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Price).HasPrecision(10, 2);
            
            // Genre as string in database
            entity.Property(e => e.Genre)
                .HasConversion(new EnumToStringConverter<BookGenre>())
                .HasMaxLength(50)
                .IsRequired();

            entity.HasOne(e => e.Author)
                .WithMany()
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.Title);
            entity.HasIndex(e => e.AuthorId);
            entity.HasIndex(e => e.Genre);
        });

        // Configure Review
        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(256);
            entity.Property(e => e.Rating).IsRequired();
            entity.Property(e => e.Text).IsRequired();

            entity.HasOne(e => e.Book)
                .WithMany(b => b.Reviews)
                .HasForeignKey(e => e.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.BookId);
        });

        // Configure Cart
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired().HasMaxLength(36);

            entity.HasIndex(e => e.UserId).IsUnique();
        });

        // Configure CartItem
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(e => e.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Book)
                .WithMany()
                .HasForeignKey(e => e.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.CartId);
        });

        // Configure Order
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired().HasMaxLength(36);
            entity.Property(e => e.TotalPrice).HasPrecision(10, 2);

            // Order Status as string
            entity.Property(e => e.Status)
                .HasConversion(new EnumToStringConverter<OrderStatus>())
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Status);
        });

        // Configure OrderItem
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PriceAtPurchase).HasPrecision(10, 2);

            entity.HasOne(e => e.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Book)
                .WithMany()
                .HasForeignKey(e => e.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.OrderId);
        });
    }
}

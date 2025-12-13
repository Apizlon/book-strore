using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert Authors
            var authorIds = new string[15];
            var authors = new[]
            {
                "George R.R. Martin",
                "J.R.R. Tolkien",
                "J.K. Rowling",
                "Stephen King",
                "Agatha Christie",
                "Isaac Asimov",
                "Arthur C. Clarke",
                "Philip K. Dick",
                "Haruki Murakami",
                "Gabriel García Márquez",
                "Margaret Atwood",
                "Ursula K. Le Guin",
                "Neil Gaiman",
                "Terry Pratchett",
                "Brandon Sanderson"
            };

            for (int i = 0; i < 15; i++)
            {
                authorIds[i] = Guid.NewGuid().ToString();
                migrationBuilder.InsertData(
                    table: "Authors",
                    columns: new[] { "Id", "Name", "Bio", "CreatedAt" },
                    values: new object[] {
                        authorIds[i],
                        authors[i],
                        $"Bio for {authors[i]}",
                        DateTime.UtcNow
                    });
            }

            // Insert Books
            var bookIds = new string[25];
            var books = new[]
            {
                new { Title = "A Game of Thrones", Genre = "Fantasy", AuthorIdx = 0, Price = 15.99m },
                new { Title = "The Fellowship of the Ring", Genre = "Fantasy", AuthorIdx = 1, Price = 16.99m },
                new { Title = "Harry Potter and the Philosopher's Stone", Genre = "Fantasy", AuthorIdx = 2, Price = 14.99m },
                new { Title = "The Shining", Genre = "Horror", AuthorIdx = 3, Price = 13.99m },
                new { Title = "Murder on the Orient Express", Genre = "Mystery", AuthorIdx = 4, Price = 12.99m },
                new { Title = "Foundation", Genre = "ScienceFiction", AuthorIdx = 5, Price = 14.99m },
                new { Title = "2001: A Space Odyssey", Genre = "ScienceFiction", AuthorIdx = 6, Price = 15.99m },
                new { Title = "Ubik", Genre = "ScienceFiction", AuthorIdx = 7, Price = 13.99m },
                new { Title = "Norwegian Wood", Genre = "Fiction", AuthorIdx = 8, Price = 14.99m },
                new { Title = "One Hundred Years of Solitude", Genre = "Fiction", AuthorIdx = 9, Price = 15.99m },
                new { Title = "The Handmaid's Tale", Genre = "Thriller", AuthorIdx = 10, Price = 15.99m },
                new { Title = "The Left Hand of Darkness", Genre = "ScienceFiction", AuthorIdx = 11, Price = 14.99m },
                new { Title = "American Gods", Genre = "Fantasy", AuthorIdx = 12, Price = 17.99m },
                new { Title = "Discworld: The Colour of Magic", Genre = "Fantasy", AuthorIdx = 13, Price = 13.99m },
                new { Title = "Mistborn", Genre = "Fantasy", AuthorIdx = 14, Price = 16.99m },
                new { Title = "A Clash of Kings", Genre = "Fantasy", AuthorIdx = 0, Price = 16.99m },
                new { Title = "The Two Towers", Genre = "Fantasy", AuthorIdx = 1, Price = 16.99m },
                new { Title = "Harry Potter and the Chamber of Secrets", Genre = "Fantasy", AuthorIdx = 2, Price = 14.99m },
                new { Title = "It", Genre = "Horror", AuthorIdx = 3, Price = 18.99m },
                new { Title = "Death on the Nile", Genre = "Mystery", AuthorIdx = 4, Price = 12.99m },
                new { Title = "I, Robot", Genre = "ScienceFiction", AuthorIdx = 5, Price = 13.99m },
                new { Title = "Rendezvous with Rama", Genre = "ScienceFiction", AuthorIdx = 6, Price = 14.99m },
                new { Title = "Do Androids Dream of Electric Sheep?", Genre = "ScienceFiction", AuthorIdx = 7, Price = 13.99m },
                new { Title = "Kafka on the Shore", Genre = "Fiction", AuthorIdx = 8, Price = 15.99m },
                new { Title = "Love in the Time of Cholera", Genre = "Romance", AuthorIdx = 9, Price = 14.99m }
            };

            for (int i = 0; i < 25; i++)
            {
                bookIds[i] = Guid.NewGuid().ToString();
                migrationBuilder.InsertData(
                    table: "Books",
                    columns: new[] { "Id", "Title", "Description", "Price", "AuthorId", "Genre", "PublishedDate", "CreatedAt" },
                    values: new object[] {
                        bookIds[i],
                        books[i].Title,
                        $"Description for {books[i].Title}",
                        books[i].Price,
                        authorIds[books[i].AuthorIdx],
                        books[i].Genre,
                        DateTime.UtcNow.AddYears(-5),
                        DateTime.UtcNow
                    });
            }

            // Insert Reviews (0-5 per book)
            var random = new Random(42); // Fixed seed for reproducibility
            var ratings = new[] { 1, 2, 3, 4, 5 };
            var reviewTexts = new[]
            {
                "Absolutely amazing book! Highly recommend.",
                "Could not put it down.",
                "Great storytelling and character development.",
                "Masterpiece of literature.",
                "Engaging from start to finish.",
                "Thought-provoking and entertaining.",
                "A must-read for everyone.",
                "Excellent work by the author.",
                "Worth every penny.",
                "Perfectly executed narrative."
            };

            foreach (var bookId in bookIds)
            {
                int reviewCount = random.Next(0, 6); // 0-5 reviews per book
                for (int j = 0; j < reviewCount; j++)
                {
                    var reviewId = Guid.NewGuid().ToString();
                    var rating = ratings[random.Next(ratings.Length)];
                    var reviewText = reviewTexts[random.Next(reviewTexts.Length)];
                    var username = $"user{random.Next(1, 100)}";

                    migrationBuilder.InsertData(
                        table: "Reviews",
                        columns: new[] { "Id", "BookId", "Username", "Rating", "Text", "CreatedAt" },
                        values: new object[] {
                            reviewId,
                            bookId,
                            username,
                            rating,
                            reviewText,
                            DateTime.UtcNow.AddDays(-random.Next(1, 365))
                        });
                }
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Удалить все отзывы
            migrationBuilder.Sql("DELETE FROM \"Reviews\"");
    
            // Удалить все элементы корзины и заказы
            migrationBuilder.Sql("DELETE FROM \"CartItems\"");
            migrationBuilder.Sql("DELETE FROM \"OrderItems\"");
            migrationBuilder.Sql("DELETE FROM \"Carts\"");
            migrationBuilder.Sql("DELETE FROM \"Orders\"");
    
            // Удалить все книги и авторов
            migrationBuilder.Sql("DELETE FROM \"Books\"");
            migrationBuilder.Sql("DELETE FROM \"Authors\"");
        }

    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.DataAccess.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AuthUsers",
            columns: table => new
            {
                Id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                Username = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                PasswordHash = table.Column<string>(type: "text", nullable: false),
                IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AuthUsers", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AuthUsers_IsActive",
            table: "AuthUsers",
            column: "IsActive");

        migrationBuilder.CreateIndex(
            name: "IX_AuthUsers_Username",
            table: "AuthUsers",
            column: "Username",
            unique: true);

        // Insert admin user with password "admin"
        var adminPassword = HashPassword("admin");
        migrationBuilder.InsertData(
            table: "AuthUsers",
            columns: new[] { "Id", "Username", "PasswordHash", "IsActive", "CreatedAt" },
            values: new object[] { 
                "550e8400-e29b-41d4-a716-446655440000", 
                "admin", 
                adminPassword,
                true,
                new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AuthUsers");
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}

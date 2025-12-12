using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var adminPassword = HashPassword("admin");
            
            migrationBuilder.InsertData(
                table: "AuthUsers",
                columns: new[] { "Id", "Username", "PasswordHash", "IsActive", "Role", "CreatedAt" },
                values: new object[] { 
                    "550e8400-e29b-41d4-a716-446655440000", 
                    "admin", 
                    adminPassword,
                    true,
                    0, // Admin role
                    new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AuthUsers",
                keyColumn: "Id",
                keyValue: "550e8400-e29b-41d4-a716-446655440000");
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
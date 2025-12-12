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
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var adminPassword = HashPassword("admin");

            migrationBuilder.Sql($@"
                INSERT INTO ""AuthUsers"" (""Id"", ""Username"", ""PasswordHash"", ""IsActive"", ""Role"", ""CreatedAt"")
                VALUES ('550e8400-e29b-41d4-a716-446655440000', 'admin', '{adminPassword}', TRUE, 'Admin', TIMESTAMPTZ '2024-01-01T00:00:00Z')
            ");
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
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Username", "Email", "DateOfBirth", "RegistrationDate", "Role", "IsActive" },
                values: new object[] { 
                    "550e8400-e29b-41d4-a716-446655440000",
                    "admin",
                    "admin@example.com",
                    null,
                    new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    "Admin",
                    true
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "550e8400-e29b-41d4-a716-446655440000");
        }
    }
}
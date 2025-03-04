using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "HashPassword");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 4, 18, 34, 45, 325, DateTimeKind.Utc).AddTicks(7261), new DateTime(2025, 3, 4, 18, 34, 45, 325, DateTimeKind.Utc).AddTicks(7263) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 4, 18, 34, 45, 325, DateTimeKind.Utc).AddTicks(7266), new DateTime(2025, 3, 4, 18, 34, 45, 325, DateTimeKind.Utc).AddTicks(7266) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 4, 18, 34, 45, 325, DateTimeKind.Utc).AddTicks(7267), new DateTime(2025, 3, 4, 18, 34, 45, 325, DateTimeKind.Utc).AddTicks(7268) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HashPassword",
                table: "Users",
                newName: "Password");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 4, 17, 31, 58, 938, DateTimeKind.Utc).AddTicks(5290), new DateTime(2025, 3, 4, 17, 31, 58, 938, DateTimeKind.Utc).AddTicks(5293) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 4, 17, 31, 58, 938, DateTimeKind.Utc).AddTicks(5295), new DateTime(2025, 3, 4, 17, 31, 58, 938, DateTimeKind.Utc).AddTicks(5295) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 4, 17, 31, 58, 938, DateTimeKind.Utc).AddTicks(5297), new DateTime(2025, 3, 4, 17, 31, 58, 938, DateTimeKind.Utc).AddTicks(5297) });
        }
    }
}

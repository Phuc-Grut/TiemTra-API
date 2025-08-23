using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upTableUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetPasswordExpiry",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 23, 15, 44, 48, 104, DateTimeKind.Unspecified).AddTicks(236), new DateTime(2025, 8, 23, 15, 44, 48, 104, DateTimeKind.Unspecified).AddTicks(278) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 23, 15, 44, 48, 104, DateTimeKind.Unspecified).AddTicks(281), new DateTime(2025, 8, 23, 15, 44, 48, 104, DateTimeKind.Unspecified).AddTicks(282) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 23, 15, 44, 48, 104, DateTimeKind.Unspecified).AddTicks(284), new DateTime(2025, 8, 23, 15, 44, 48, 104, DateTimeKind.Unspecified).AddTicks(284) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPasswordCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResetPasswordExpiry",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 27, 20, 14, 41, 262, DateTimeKind.Unspecified).AddTicks(2777), new DateTime(2025, 7, 27, 20, 14, 41, 262, DateTimeKind.Unspecified).AddTicks(2813) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 27, 20, 14, 41, 262, DateTimeKind.Unspecified).AddTicks(2816), new DateTime(2025, 7, 27, 20, 14, 41, 262, DateTimeKind.Unspecified).AddTicks(2817) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 27, 20, 14, 41, 262, DateTimeKind.Unspecified).AddTicks(2819), new DateTime(2025, 7, 27, 20, 14, 41, 262, DateTimeKind.Unspecified).AddTicks(2819) });
        }
    }
}

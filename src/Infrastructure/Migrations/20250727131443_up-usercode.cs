using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upusercode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Customers");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 15, 17, 5, 1, 637, DateTimeKind.Unspecified).AddTicks(1601), new DateTime(2025, 7, 15, 17, 5, 1, 637, DateTimeKind.Unspecified).AddTicks(1638) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 15, 17, 5, 1, 637, DateTimeKind.Unspecified).AddTicks(1641), new DateTime(2025, 7, 15, 17, 5, 1, 637, DateTimeKind.Unspecified).AddTicks(1642) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 15, 17, 5, 1, 637, DateTimeKind.Unspecified).AddTicks(1643), new DateTime(2025, 7, 15, 17, 5, 1, 637, DateTimeKind.Unspecified).AddTicks(1644) });
        }
    }
}

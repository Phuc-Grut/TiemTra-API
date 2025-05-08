using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upProductImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "ProductImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 7, 9, 15, 14, 180, DateTimeKind.Utc).AddTicks(1907), new DateTime(2025, 5, 7, 9, 15, 14, 180, DateTimeKind.Utc).AddTicks(1910) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 7, 9, 15, 14, 180, DateTimeKind.Utc).AddTicks(1913), new DateTime(2025, 5, 7, 9, 15, 14, 180, DateTimeKind.Utc).AddTicks(1913) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 7, 9, 15, 14, 180, DateTimeKind.Utc).AddTicks(1914), new DateTime(2025, 5, 7, 9, 15, 14, 180, DateTimeKind.Utc).AddTicks(1914) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "ProductImages");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 25, 14, 54, 21, 525, DateTimeKind.Utc).AddTicks(7166), new DateTime(2025, 4, 25, 14, 54, 21, 525, DateTimeKind.Utc).AddTicks(7169) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 25, 14, 54, 21, 525, DateTimeKind.Utc).AddTicks(7172), new DateTime(2025, 4, 25, 14, 54, 21, 525, DateTimeKind.Utc).AddTicks(7172) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 25, 14, 54, 21, 525, DateTimeKind.Utc).AddTicks(7174), new DateTime(2025, 4, 25, 14, 54, 21, 525, DateTimeKind.Utc).AddTicks(7174) });
        }
    }
}

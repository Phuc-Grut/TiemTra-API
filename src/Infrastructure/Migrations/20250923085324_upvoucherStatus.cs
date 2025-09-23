using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upvoucherStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 23, 15, 53, 23, 203, DateTimeKind.Unspecified).AddTicks(5539), new DateTime(2025, 9, 23, 15, 53, 23, 203, DateTimeKind.Unspecified).AddTicks(5577) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 23, 15, 53, 23, 203, DateTimeKind.Unspecified).AddTicks(5580), new DateTime(2025, 9, 23, 15, 53, 23, 203, DateTimeKind.Unspecified).AddTicks(5580) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 23, 15, 53, 23, 203, DateTimeKind.Unspecified).AddTicks(5582), new DateTime(2025, 9, 23, 15, 53, 23, 203, DateTimeKind.Unspecified).AddTicks(5583) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 23, 15, 32, 37, 552, DateTimeKind.Unspecified).AddTicks(985), new DateTime(2025, 9, 23, 15, 32, 37, 552, DateTimeKind.Unspecified).AddTicks(1021) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 23, 15, 32, 37, 552, DateTimeKind.Unspecified).AddTicks(1024), new DateTime(2025, 9, 23, 15, 32, 37, 552, DateTimeKind.Unspecified).AddTicks(1025) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 23, 15, 32, 37, 552, DateTimeKind.Unspecified).AddTicks(1026), new DateTime(2025, 9, 23, 15, 32, 37, 552, DateTimeKind.Unspecified).AddTicks(1027) });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upTableVoucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 14, 40, 5, 499, DateTimeKind.Unspecified).AddTicks(6779), new DateTime(2025, 9, 19, 14, 40, 5, 499, DateTimeKind.Unspecified).AddTicks(6812) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 14, 40, 5, 499, DateTimeKind.Unspecified).AddTicks(6815), new DateTime(2025, 9, 19, 14, 40, 5, 499, DateTimeKind.Unspecified).AddTicks(6815) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 19, 14, 40, 5, 499, DateTimeKind.Unspecified).AddTicks(6817), new DateTime(2025, 9, 19, 14, 40, 5, 499, DateTimeKind.Unspecified).AddTicks(6818) });
        }
    }
}

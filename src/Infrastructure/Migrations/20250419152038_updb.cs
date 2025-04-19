using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 19, 15, 20, 36, 107, DateTimeKind.Utc).AddTicks(1803), new DateTime(2025, 4, 19, 15, 20, 36, 107, DateTimeKind.Utc).AddTicks(1805) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 19, 15, 20, 36, 107, DateTimeKind.Utc).AddTicks(1815), new DateTime(2025, 4, 19, 15, 20, 36, 107, DateTimeKind.Utc).AddTicks(1815) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 19, 15, 20, 36, 107, DateTimeKind.Utc).AddTicks(1817), new DateTime(2025, 4, 19, 15, 20, 36, 107, DateTimeKind.Utc).AddTicks(1817) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 18, 1, 19, 27, 796, DateTimeKind.Utc).AddTicks(8677), new DateTime(2025, 4, 18, 1, 19, 27, 796, DateTimeKind.Utc).AddTicks(8683) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 18, 1, 19, 27, 796, DateTimeKind.Utc).AddTicks(8688), new DateTime(2025, 4, 18, 1, 19, 27, 796, DateTimeKind.Utc).AddTicks(8688) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 18, 1, 19, 27, 796, DateTimeKind.Utc).AddTicks(8691), new DateTime(2025, 4, 18, 1, 19, 27, 796, DateTimeKind.Utc).AddTicks(8691) });
        }
    }
}

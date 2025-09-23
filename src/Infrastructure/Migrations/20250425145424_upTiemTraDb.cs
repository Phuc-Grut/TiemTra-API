using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upTiemTraDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 25, 8, 4, 31, 308, DateTimeKind.Utc).AddTicks(2235), new DateTime(2025, 4, 25, 8, 4, 31, 308, DateTimeKind.Utc).AddTicks(2239) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 25, 8, 4, 31, 308, DateTimeKind.Utc).AddTicks(2242), new DateTime(2025, 4, 25, 8, 4, 31, 308, DateTimeKind.Utc).AddTicks(2242) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 25, 8, 4, 31, 308, DateTimeKind.Utc).AddTicks(2244), new DateTime(2025, 4, 25, 8, 4, 31, 308, DateTimeKind.Utc).AddTicks(2244) });
        }
    }
}
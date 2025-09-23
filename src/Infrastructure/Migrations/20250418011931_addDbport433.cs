using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addDbport433 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 9, 11, 9, 51, 245, DateTimeKind.Utc).AddTicks(6243), new DateTime(2025, 4, 9, 11, 9, 51, 245, DateTimeKind.Utc).AddTicks(6245) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 9, 11, 9, 51, 245, DateTimeKind.Utc).AddTicks(6247), new DateTime(2025, 4, 9, 11, 9, 51, 245, DateTimeKind.Utc).AddTicks(6248) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 9, 11, 9, 51, 245, DateTimeKind.Utc).AddTicks(6249), new DateTime(2025, 4, 9, 11, 9, 51, 245, DateTimeKind.Utc).AddTicks(6249) });
        }
    }
}
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpStatusProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 15, 16, 58, 13, 984, DateTimeKind.Unspecified).AddTicks(7244), new DateTime(2025, 7, 15, 16, 58, 13, 984, DateTimeKind.Unspecified).AddTicks(7855) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 15, 16, 58, 13, 984, DateTimeKind.Unspecified).AddTicks(7864), new DateTime(2025, 7, 15, 16, 58, 13, 984, DateTimeKind.Unspecified).AddTicks(7865) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 15, 16, 58, 13, 984, DateTimeKind.Unspecified).AddTicks(7867), new DateTime(2025, 7, 15, 16, 58, 13, 984, DateTimeKind.Unspecified).AddTicks(7868) });
        }
    }
}
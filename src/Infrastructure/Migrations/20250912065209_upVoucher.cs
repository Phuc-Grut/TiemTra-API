using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upVoucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 12, 13, 52, 6, 829, DateTimeKind.Unspecified).AddTicks(5231), new DateTime(2025, 9, 12, 13, 52, 6, 829, DateTimeKind.Unspecified).AddTicks(5277) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 12, 13, 52, 6, 829, DateTimeKind.Unspecified).AddTicks(5282), new DateTime(2025, 9, 12, 13, 52, 6, 829, DateTimeKind.Unspecified).AddTicks(5283) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 12, 13, 52, 6, 829, DateTimeKind.Unspecified).AddTicks(5286), new DateTime(2025, 9, 12, 13, 52, 6, 829, DateTimeKind.Unspecified).AddTicks(5287) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 8, 16, 58, 29, 346, DateTimeKind.Unspecified).AddTicks(6601), new DateTime(2025, 9, 8, 16, 58, 29, 346, DateTimeKind.Unspecified).AddTicks(6643) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 8, 16, 58, 29, 346, DateTimeKind.Unspecified).AddTicks(6647), new DateTime(2025, 9, 8, 16, 58, 29, 346, DateTimeKind.Unspecified).AddTicks(6648) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 8, 16, 58, 29, 346, DateTimeKind.Unspecified).AddTicks(6650), new DateTime(2025, 9, 8, 16, 58, 29, 346, DateTimeKind.Unspecified).AddTicks(6650) });
        }
    }
}
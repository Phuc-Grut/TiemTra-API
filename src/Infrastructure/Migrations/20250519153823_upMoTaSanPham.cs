using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upMoTaSanPham : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 19, 15, 38, 21, 884, DateTimeKind.Utc).AddTicks(2703), new DateTime(2025, 5, 19, 15, 38, 21, 884, DateTimeKind.Utc).AddTicks(2705) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 19, 15, 38, 21, 884, DateTimeKind.Utc).AddTicks(2707), new DateTime(2025, 5, 19, 15, 38, 21, 884, DateTimeKind.Utc).AddTicks(2707) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 19, 15, 38, 21, 884, DateTimeKind.Utc).AddTicks(2709), new DateTime(2025, 5, 19, 15, 38, 21, 884, DateTimeKind.Utc).AddTicks(2709) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 18, 16, 13, 32, 577, DateTimeKind.Utc).AddTicks(3605), new DateTime(2025, 5, 18, 16, 13, 32, 577, DateTimeKind.Utc).AddTicks(3608) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 18, 16, 13, 32, 577, DateTimeKind.Utc).AddTicks(3610), new DateTime(2025, 5, 18, 16, 13, 32, 577, DateTimeKind.Utc).AddTicks(3610) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 18, 16, 13, 32, 577, DateTimeKind.Utc).AddTicks(3611), new DateTime(2025, 5, 18, 16, 13, 32, 577, DateTimeKind.Utc).AddTicks(3612) });
        }
    }
}
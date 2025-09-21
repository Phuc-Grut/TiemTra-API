using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upCart3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalItems",
                table: "Cart",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Cart",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 8, 50, 16, 896, DateTimeKind.Utc).AddTicks(6845), new DateTime(2025, 7, 5, 8, 50, 16, 896, DateTimeKind.Utc).AddTicks(6849) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 8, 50, 16, 896, DateTimeKind.Utc).AddTicks(6854), new DateTime(2025, 7, 5, 8, 50, 16, 896, DateTimeKind.Utc).AddTicks(6855) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 8, 50, 16, 896, DateTimeKind.Utc).AddTicks(6856), new DateTime(2025, 7, 5, 8, 50, 16, 896, DateTimeKind.Utc).AddTicks(6856) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalItems",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Cart");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 8, 42, 3, 413, DateTimeKind.Utc).AddTicks(1278), new DateTime(2025, 7, 5, 8, 42, 3, 413, DateTimeKind.Utc).AddTicks(1281) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 8, 42, 3, 413, DateTimeKind.Utc).AddTicks(1283), new DateTime(2025, 7, 5, 8, 42, 3, 413, DateTimeKind.Utc).AddTicks(1283) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 5, 8, 42, 3, 413, DateTimeKind.Utc).AddTicks(1285), new DateTime(2025, 7, 5, 8, 42, 3, 413, DateTimeKind.Utc).AddTicks(1285) });
        }
    }
}
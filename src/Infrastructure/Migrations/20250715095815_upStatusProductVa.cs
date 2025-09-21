using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upStatusProductVa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ProductVariations",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProductVariations");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 15, 15, 58, 32, 716, DateTimeKind.Unspecified).AddTicks(5588), new DateTime(2025, 7, 15, 15, 58, 32, 716, DateTimeKind.Unspecified).AddTicks(5628) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 15, 15, 58, 32, 716, DateTimeKind.Unspecified).AddTicks(5630), new DateTime(2025, 7, 15, 15, 58, 32, 716, DateTimeKind.Unspecified).AddTicks(5631) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 15, 15, 58, 32, 716, DateTimeKind.Unspecified).AddTicks(5632), new DateTime(2025, 7, 15, 15, 58, 32, 716, DateTimeKind.Unspecified).AddTicks(5633) });
        }
    }
}
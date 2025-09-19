using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upTableOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ItemsSubtotal",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemsSubtotal",
                table: "Orders");

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
    }
}

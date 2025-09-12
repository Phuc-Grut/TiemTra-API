using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upOrderStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledAt",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveredAt",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippedAt",
                table: "Orders",
                type: "datetime2",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelledAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveredAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippedAt",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 23, 15, 44, 48, 104, DateTimeKind.Unspecified).AddTicks(236), new DateTime(2025, 8, 23, 15, 44, 48, 104, DateTimeKind.Unspecified).AddTicks(278) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 23, 15, 44, 48, 104, DateTimeKind.Unspecified).AddTicks(281), new DateTime(2025, 8, 23, 15, 44, 48, 104, DateTimeKind.Unspecified).AddTicks(282) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 23, 15, 44, 48, 104, DateTimeKind.Unspecified).AddTicks(284), new DateTime(2025, 8, 23, 15, 44, 48, 104, DateTimeKind.Unspecified).AddTicks(284) });
        }
    }
}

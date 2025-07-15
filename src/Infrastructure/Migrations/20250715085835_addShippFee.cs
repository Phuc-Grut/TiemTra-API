using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addShippFee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ShippingFee",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingFee",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 13, 22, 34, 50, 928, DateTimeKind.Unspecified).AddTicks(3831), new DateTime(2025, 7, 13, 22, 34, 50, 928, DateTimeKind.Unspecified).AddTicks(3874) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 13, 22, 34, 50, 928, DateTimeKind.Unspecified).AddTicks(3879), new DateTime(2025, 7, 13, 22, 34, 50, 928, DateTimeKind.Unspecified).AddTicks(3880) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 13, 22, 34, 50, 928, DateTimeKind.Unspecified).AddTicks(3882), new DateTime(2025, 7, 13, 22, 34, 50, 928, DateTimeKind.Unspecified).AddTicks(3883) });
        }
    }
}

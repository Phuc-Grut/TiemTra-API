using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OrderItems",
                newName: "UnitPrice");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 12, 9, 16, 5, 568, DateTimeKind.Utc).AddTicks(380), new DateTime(2025, 7, 12, 9, 16, 5, 568, DateTimeKind.Utc).AddTicks(384) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 12, 9, 16, 5, 568, DateTimeKind.Utc).AddTicks(388), new DateTime(2025, 7, 12, 9, 16, 5, 568, DateTimeKind.Utc).AddTicks(389) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 12, 9, 16, 5, 568, DateTimeKind.Utc).AddTicks(390), new DateTime(2025, 7, 12, 9, 16, 5, 568, DateTimeKind.Utc).AddTicks(390) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "OrderItems",
                newName: "Price");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 9, 9, 38, 29, 432, DateTimeKind.Utc).AddTicks(7567), new DateTime(2025, 7, 9, 9, 38, 29, 432, DateTimeKind.Utc).AddTicks(7572) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 9, 9, 38, 29, 432, DateTimeKind.Utc).AddTicks(7577), new DateTime(2025, 7, 9, 9, 38, 29, 432, DateTimeKind.Utc).AddTicks(7577) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 9, 9, 38, 29, 432, DateTimeKind.Utc).AddTicks(7579), new DateTime(2025, 7, 9, 9, 38, 29, 432, DateTimeKind.Utc).AddTicks(7579) });
        }
    }
}

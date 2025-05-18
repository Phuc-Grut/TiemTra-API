using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upProductStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductStatus",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 18, 15, 36, 6, 367, DateTimeKind.Utc).AddTicks(4410), new DateTime(2025, 5, 18, 15, 36, 6, 367, DateTimeKind.Utc).AddTicks(4412) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 18, 15, 36, 6, 367, DateTimeKind.Utc).AddTicks(4413), new DateTime(2025, 5, 18, 15, 36, 6, 367, DateTimeKind.Utc).AddTicks(4414) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 18, 15, 36, 6, 367, DateTimeKind.Utc).AddTicks(4415), new DateTime(2025, 5, 18, 15, 36, 6, 367, DateTimeKind.Utc).AddTicks(4415) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductStatus",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 7, 9, 15, 14, 180, DateTimeKind.Utc).AddTicks(1907), new DateTime(2025, 5, 7, 9, 15, 14, 180, DateTimeKind.Utc).AddTicks(1910) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 7, 9, 15, 14, 180, DateTimeKind.Utc).AddTicks(1913), new DateTime(2025, 5, 7, 9, 15, 14, 180, DateTimeKind.Utc).AddTicks(1913) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 7, 9, 15, 14, 180, DateTimeKind.Utc).AddTicks(1914), new DateTime(2025, 5, 7, 9, 15, 14, 180, DateTimeKind.Utc).AddTicks(1914) });
        }
    }
}

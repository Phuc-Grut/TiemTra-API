using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updb3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PackageSize",
                table: "ProductVariations",
                newName: "TypeName");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 25, 8, 4, 31, 308, DateTimeKind.Utc).AddTicks(2235), new DateTime(2025, 4, 25, 8, 4, 31, 308, DateTimeKind.Utc).AddTicks(2239) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 25, 8, 4, 31, 308, DateTimeKind.Utc).AddTicks(2242), new DateTime(2025, 4, 25, 8, 4, 31, 308, DateTimeKind.Utc).AddTicks(2242) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 25, 8, 4, 31, 308, DateTimeKind.Utc).AddTicks(2244), new DateTime(2025, 4, 25, 8, 4, 31, 308, DateTimeKind.Utc).AddTicks(2244) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TypeName",
                table: "ProductVariations",
                newName: "PackageSize");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 25, 3, 40, 37, 197, DateTimeKind.Utc).AddTicks(5018), new DateTime(2025, 4, 25, 3, 40, 37, 197, DateTimeKind.Utc).AddTicks(5021) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 25, 3, 40, 37, 197, DateTimeKind.Utc).AddTicks(5023), new DateTime(2025, 4, 25, 3, 40, 37, 197, DateTimeKind.Utc).AddTicks(5024) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 25, 3, 40, 37, 197, DateTimeKind.Utc).AddTicks(5025), new DateTime(2025, 4, 25, 3, 40, 37, 197, DateTimeKind.Utc).AddTicks(5026) });
        }
    }
}

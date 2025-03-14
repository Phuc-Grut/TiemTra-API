using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updb2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 14, 12, 50, 54, 688, DateTimeKind.Utc).AddTicks(3210), new DateTime(2025, 3, 14, 12, 50, 54, 688, DateTimeKind.Utc).AddTicks(3212) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 14, 12, 50, 54, 688, DateTimeKind.Utc).AddTicks(3214), new DateTime(2025, 3, 14, 12, 50, 54, 688, DateTimeKind.Utc).AddTicks(3215) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 14, 12, 50, 54, 688, DateTimeKind.Utc).AddTicks(3216), new DateTime(2025, 3, 14, 12, 50, 54, 688, DateTimeKind.Utc).AddTicks(3216) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 14, 12, 50, 8, 584, DateTimeKind.Utc).AddTicks(1875), new DateTime(2025, 3, 14, 12, 50, 8, 584, DateTimeKind.Utc).AddTicks(1878) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 14, 12, 50, 8, 584, DateTimeKind.Utc).AddTicks(1880), new DateTime(2025, 3, 14, 12, 50, 8, 584, DateTimeKind.Utc).AddTicks(1880) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 14, 12, 50, 8, 584, DateTimeKind.Utc).AddTicks(1882), new DateTime(2025, 3, 14, 12, 50, 8, 584, DateTimeKind.Utc).AddTicks(1882) });
        }
    }
}

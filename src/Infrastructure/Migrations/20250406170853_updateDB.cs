using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 6, 17, 8, 51, 755, DateTimeKind.Utc).AddTicks(3622), new DateTime(2025, 4, 6, 17, 8, 51, 755, DateTimeKind.Utc).AddTicks(3629) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 6, 17, 8, 51, 755, DateTimeKind.Utc).AddTicks(3630), new DateTime(2025, 4, 6, 17, 8, 51, 755, DateTimeKind.Utc).AddTicks(3631) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 6, 17, 8, 51, 755, DateTimeKind.Utc).AddTicks(3632), new DateTime(2025, 4, 6, 17, 8, 51, 755, DateTimeKind.Utc).AddTicks(3632) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 1, 10, 26, 45, 274, DateTimeKind.Utc).AddTicks(3886), new DateTime(2025, 4, 1, 10, 26, 45, 274, DateTimeKind.Utc).AddTicks(3889) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 1, 10, 26, 45, 274, DateTimeKind.Utc).AddTicks(3891), new DateTime(2025, 4, 1, 10, 26, 45, 274, DateTimeKind.Utc).AddTicks(3891) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 1, 10, 26, 45, 274, DateTimeKind.Utc).AddTicks(3892), new DateTime(2025, 4, 1, 10, 26, 45, 274, DateTimeKind.Utc).AddTicks(3893) });
        }
    }
}

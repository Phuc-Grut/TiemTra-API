using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class up2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(2025, 3, 19, 12, 18, 41, 823, DateTimeKind.Utc).AddTicks(9015), new DateTime(2025, 3, 19, 12, 18, 41, 823, DateTimeKind.Utc).AddTicks(9019), new Guid("00000000-0000-0000-0000-000000000000") });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(2025, 3, 19, 12, 18, 41, 823, DateTimeKind.Utc).AddTicks(9021), new DateTime(2025, 3, 19, 12, 18, 41, 823, DateTimeKind.Utc).AddTicks(9022), new Guid("00000000-0000-0000-0000-000000000000") });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(2025, 3, 19, 12, 18, 41, 823, DateTimeKind.Utc).AddTicks(9023), new DateTime(2025, 3, 19, 12, 18, 41, 823, DateTimeKind.Utc).AddTicks(9024), new Guid("00000000-0000-0000-0000-000000000000") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(2025, 3, 19, 12, 16, 6, 928, DateTimeKind.Utc).AddTicks(3667), new DateTime(2025, 3, 19, 12, 16, 6, 928, DateTimeKind.Utc).AddTicks(3669), null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(2025, 3, 19, 12, 16, 6, 928, DateTimeKind.Utc).AddTicks(3672), new DateTime(2025, 3, 19, 12, 16, 6, 928, DateTimeKind.Utc).AddTicks(3672), null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(2025, 3, 19, 12, 16, 6, 928, DateTimeKind.Utc).AddTicks(3713), new DateTime(2025, 3, 19, 12, 16, 6, 928, DateTimeKind.Utc).AddTicks(3713), null });
        }
    }
}

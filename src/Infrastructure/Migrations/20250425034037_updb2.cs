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
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_ProductVariations_ProductVariationId1",
                table: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_ProductVariationId1",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductVariationId",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "ProductVariationId1",
                table: "ProductImages");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductVariationId",
                table: "ProductImages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductVariationId1",
                table: "ProductImages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 25, 3, 1, 59, 462, DateTimeKind.Utc).AddTicks(4517), new DateTime(2025, 4, 25, 3, 1, 59, 462, DateTimeKind.Utc).AddTicks(4520) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 25, 3, 1, 59, 462, DateTimeKind.Utc).AddTicks(4522), new DateTime(2025, 4, 25, 3, 1, 59, 462, DateTimeKind.Utc).AddTicks(4522) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 25, 3, 1, 59, 462, DateTimeKind.Utc).AddTicks(4524), new DateTime(2025, 4, 25, 3, 1, 59, 462, DateTimeKind.Utc).AddTicks(4524) });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductVariationId1",
                table: "ProductImages",
                column: "ProductVariationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_ProductVariations_ProductVariationId1",
                table: "ProductImages",
                column: "ProductVariationId1",
                principalTable: "ProductVariations",
                principalColumn: "ProductVariationId");
        }
    }
}

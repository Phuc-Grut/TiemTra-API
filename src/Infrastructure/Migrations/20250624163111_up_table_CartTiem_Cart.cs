using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class up_table_CartTiem_Cart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductVariationId",
                table: "CartItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Cart",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 24, 16, 31, 9, 224, DateTimeKind.Utc).AddTicks(456), new DateTime(2025, 6, 24, 16, 31, 9, 224, DateTimeKind.Utc).AddTicks(461) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 24, 16, 31, 9, 224, DateTimeKind.Utc).AddTicks(463), new DateTime(2025, 6, 24, 16, 31, 9, 224, DateTimeKind.Utc).AddTicks(463) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 24, 16, 31, 9, 224, DateTimeKind.Utc).AddTicks(464), new DateTime(2025, 6, 24, 16, 31, 9, 224, DateTimeKind.Utc).AddTicks(464) });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductVariationId",
                table: "CartItems",
                column: "ProductVariationId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_ProductId",
                table: "Cart",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Products_ProductId",
                table: "Cart",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ProductVariations_ProductVariationId",
                table: "CartItems",
                column: "ProductVariationId",
                principalTable: "ProductVariations",
                principalColumn: "ProductVariationId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Products_ProductId",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ProductVariations_ProductVariationId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ProductVariationId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_Cart_ProductId",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "ProductVariationId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Cart");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 15, 43, 53, 272, DateTimeKind.Utc).AddTicks(8352), new DateTime(2025, 6, 20, 15, 43, 53, 272, DateTimeKind.Utc).AddTicks(8356) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 15, 43, 53, 272, DateTimeKind.Utc).AddTicks(8359), new DateTime(2025, 6, 20, 15, 43, 53, 272, DateTimeKind.Utc).AddTicks(8359) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 20, 15, 43, 53, 272, DateTimeKind.Utc).AddTicks(8360), new DateTime(2025, 6, 20, 15, 43, 53, 272, DateTimeKind.Utc).AddTicks(8360) });
        }
    }
}
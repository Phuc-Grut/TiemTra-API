using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class deleteVariationsDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_ProductVariations_ProductVariationId",
                table: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductVariationDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVariations",
                table: "ProductVariations");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_ProductVariationId",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "ProductVariationsId",
                table: "ProductVariations");

            migrationBuilder.DropColumn(
                name: "VariationType",
                table: "ProductVariations");

            migrationBuilder.RenameColumn(
                name: "VariationValue",
                table: "ProductVariations",
                newName: "PackageSize");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "ProductVariations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductVariationId",
                table: "ProductVariations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ProductVariations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "ProductVariations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductVariationId1",
                table: "ProductImages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVariations",
                table: "ProductVariations",
                column: "ProductVariationId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_ProductVariations_ProductVariationId1",
                table: "ProductImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVariations",
                table: "ProductVariations");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_ProductVariationId1",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "ProductVariationId",
                table: "ProductVariations");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ProductVariations");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "ProductVariations");

            migrationBuilder.DropColumn(
                name: "ProductVariationId1",
                table: "ProductImages");

            migrationBuilder.RenameColumn(
                name: "PackageSize",
                table: "ProductVariations",
                newName: "VariationValue");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "ProductVariations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "ProductVariationsId",
                table: "ProductVariations",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "VariationType",
                table: "ProductVariations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVariations",
                table: "ProductVariations",
                column: "ProductVariationsId");

            migrationBuilder.CreateTable(
                name: "ProductVariationDetails",
                columns: table => new
                {
                    ProductVariationDetailsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductVariationsId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stock = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariationDetails", x => x.ProductVariationDetailsId);
                    table.ForeignKey(
                        name: "FK_ProductVariationDetails_ProductVariations_ProductVariationsId",
                        column: x => x.ProductVariationsId,
                        principalTable: "ProductVariations",
                        principalColumn: "ProductVariationsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 19, 15, 20, 36, 107, DateTimeKind.Utc).AddTicks(1803), new DateTime(2025, 4, 19, 15, 20, 36, 107, DateTimeKind.Utc).AddTicks(1805) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 19, 15, 20, 36, 107, DateTimeKind.Utc).AddTicks(1815), new DateTime(2025, 4, 19, 15, 20, 36, 107, DateTimeKind.Utc).AddTicks(1815) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 4, 19, 15, 20, 36, 107, DateTimeKind.Utc).AddTicks(1817), new DateTime(2025, 4, 19, 15, 20, 36, 107, DateTimeKind.Utc).AddTicks(1817) });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductVariationId",
                table: "ProductImages",
                column: "ProductVariationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariationDetails_ProductVariationsId",
                table: "ProductVariationDetails",
                column: "ProductVariationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_ProductVariations_ProductVariationId",
                table: "ProductImages",
                column: "ProductVariationId",
                principalTable: "ProductVariations",
                principalColumn: "ProductVariationsId");
        }
    }
}
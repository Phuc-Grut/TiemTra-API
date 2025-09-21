using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upComfirmAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedAt",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "OrderItems",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmedAt",
                table: "Orders");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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
    }
}
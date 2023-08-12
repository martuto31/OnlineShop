using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.DAL.Migrations
{
    public partial class AddedManyToManyUserToProductsRel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "768e20a8-00a1-4bfc-bbc4-4bbff670e56c");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "8389c099-72c8-47ad-9e4d-fc41b34ce276");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 12, 14, 2, 2, 221, DateTimeKind.Utc).AddTicks(6783),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 8, 4, 18, 10, 22, 749, DateTimeKind.Utc).AddTicks(7197));

            migrationBuilder.CreateTable(
                name: "UserWithProducts",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWithProducts", x => new { x.UserId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_UserWithProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWithProducts_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2dbc598b-0fdf-415b-a0cb-fcb5c1b31c1c", "2", "User", "User" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "aa955bce-5940-4404-928e-749047647791", "1", "Admin", "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_UserWithProducts_ProductId",
                table: "UserWithProducts",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserWithProducts");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "2dbc598b-0fdf-415b-a0cb-fcb5c1b31c1c");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "aa955bce-5940-4404-928e-749047647791");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 4, 18, 10, 22, 749, DateTimeKind.Utc).AddTicks(7197),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 8, 12, 14, 2, 2, 221, DateTimeKind.Utc).AddTicks(6783));

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "768e20a8-00a1-4bfc-bbc4-4bbff670e56c", "2", "User", "User" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8389c099-72c8-47ad-9e4d-fc41b34ce276", "1", "Admin", "Admin" });
        }
    }
}

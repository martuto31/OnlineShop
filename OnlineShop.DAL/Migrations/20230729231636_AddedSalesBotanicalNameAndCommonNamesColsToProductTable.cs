using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedSalesBotanicalNameAndCommonNamesColsToProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalDescription",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BotanicalName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CommonName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Sales",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalDescription",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BotanicalName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CommonName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Sales",
                table: "Products");
        }
    }
}

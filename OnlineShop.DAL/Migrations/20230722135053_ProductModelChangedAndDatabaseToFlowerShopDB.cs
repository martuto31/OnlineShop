using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ProductModelChangedAndDatabaseToFlowerShopDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductTarget",
                table: "Products",
                newName: "LightIntensity");

            migrationBuilder.AddColumn<bool>(
                name: "AirPurify",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "GrowDifficulty",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "PetCompatibility",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AirPurify",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GrowDifficulty",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PetCompatibility",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "LightIntensity",
                table: "Products",
                newName: "ProductTarget");
        }
    }
}

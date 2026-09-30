using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RolexWatches.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixBrands : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brands",
                table: "Brands");

            migrationBuilder.RenameTable(
                name: "Brands",
                newName: "Qwin9Brands");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Qwin9Brands",
                table: "Qwin9Brands",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Qwin9Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Qwin9Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Qwin9Brands_BrandId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Qwin9Brands",
                table: "Qwin9Brands");

            migrationBuilder.RenameTable(
                name: "Qwin9Brands",
                newName: "Brands");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brands",
                table: "Brands",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

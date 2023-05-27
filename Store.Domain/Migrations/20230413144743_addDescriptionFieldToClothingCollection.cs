using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Domain.Migrations
{
    /// <inheritdoc />
    public partial class addDescriptionFieldToClothingCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ClothingCollections",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ClothingCollections");
        }
    }
}

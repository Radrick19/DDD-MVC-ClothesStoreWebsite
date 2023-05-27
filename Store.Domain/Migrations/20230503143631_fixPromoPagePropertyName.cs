using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Domain.Migrations
{
    /// <inheritdoc />
    public partial class fixPromoPagePropertyName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Titile",
                table: "PromoPages",
                newName: "Title");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Subcategories",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(240)",
                oldMaxLength: 240);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "PromoPages",
                newName: "Titile");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Subcategories",
                type: "character varying(240)",
                maxLength: 240,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300);
        }
    }
}

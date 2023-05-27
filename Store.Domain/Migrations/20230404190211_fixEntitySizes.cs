using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Domain.Migrations
{
    /// <inheritdoc />
    public partial class fixEntitySizes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductsSizes_ProductId",
                table: "ProductsSizes");

            migrationBuilder.DropIndex(
                name: "IX_ProductsShops_ProductId",
                table: "ProductsShops");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSizes_ProductId",
                table: "ProductsSizes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsShops_ProductId",
                table: "ProductsShops",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductsSizes_ProductId",
                table: "ProductsSizes");

            migrationBuilder.DropIndex(
                name: "IX_ProductsShops_ProductId",
                table: "ProductsShops");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSizes_ProductId",
                table: "ProductsSizes",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductsShops_ProductId",
                table: "ProductsShops",
                column: "ProductId",
                unique: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Domain.Migrations
{
    /// <inheritdoc />
    public partial class deleteIsUniqueFromEntityIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ColorsProducts_ProductId",
                table: "ColorsProducts");

            migrationBuilder.DropIndex(
                name: "IX_CollectionsProducts_ProductId",
                table: "CollectionsProducts");

            migrationBuilder.CreateIndex(
                name: "IX_ColorsProducts_ProductId",
                table: "ColorsProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionsProducts_ProductId",
                table: "CollectionsProducts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ColorsProducts_ProductId",
                table: "ColorsProducts");

            migrationBuilder.DropIndex(
                name: "IX_CollectionsProducts_ProductId",
                table: "CollectionsProducts");

            migrationBuilder.CreateIndex(
                name: "IX_ColorsProducts_ProductId",
                table: "ColorsProducts",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CollectionsProducts_ProductId",
                table: "CollectionsProducts",
                column: "ProductId",
                unique: true);
        }
    }
}

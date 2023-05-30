using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Store.Domain.Migrations
{
    /// <inheritdoc />
    public partial class addUserAccountToSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Subcategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subcategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Subcategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Subcategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Guid", "IsEmailConfirmed", "Login", "Password", "RegistrationDate", "Salt", "UserRole" },
                values: new object[] { 1, "Admin@admin.com", new Guid("ab331490-b84f-47d9-9276-8a821944b1fe"), true, "admin", "ZlkHh6U5OhuqN08dKpvcVl+6ab+qJ400+QMu/T48+Og=", new DateTime(2023, 5, 30, 11, 56, 22, 261, DateTimeKind.Utc).AddTicks(7689), "6028927", "Administrator" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Id", "Hex", "Name", "Order" },
                values: new object[,]
                {
                    { 1, "#f3f3f3", "Белый", 1 },
                    { 2, "#18181a", "Чёрный", 2 }
                });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "Id", "CanReturn", "CategoryId", "Description", "DisplayName", "Name", "Order" },
                values: new object[,]
                {
                    { 1, true, 1, "В погоне за идеальной парой джинсов? Наш ассортимент женских джинсов охватывает каждую подгонку, порез и стиль для всех видов гардероба, от наших джинсов с высокой талией до расслабленных джинсов с широкими ногами. Откройте для себя наши совершенно новые изогнутые джинсы для совершенно новой модной", "Джинсы", "jeans", 1 },
                    { 2, true, 2, "Джинсы: этот проверенный временем, в течение всего сезона. Примите сознательно случайный образ с нашей коллекцией мужских джинсов с подходящими для каждого стиля, настроения и ансамбля. Мы обновили наши вечные джинсы с обычной подгонкой для совершенно нового сезона с тонкой конусочкой", "Джинсы", "jeans", 1 },
                    { 3, true, 3, "UT Архив переиздает тщательно отобранные дизайны из прошлых коллекций. Самые популярные дизайны за последние 20 лет были отобраны для этой переизданной коллекции от художников Энди Уорхола, Жана-Мишеля Баскиата и Кейта Харинга.", "UT футболки с принтами", "ut-shirts", 1 },
                    { 4, false, 4, "Убедитесь, что ваш малыш остается уютным днем и ночью с восхитительным ассортиментом Oniqlo One Piece Sleepings и BodySuits. Благословные, стеганые и микрофлиальные выборы подготовлены к детской уютной, в то время как дизайны с легким изменением с помощью молнии и кнопок с цветами, снимая стресс", "Боди", "bodysuit", 1 }
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Store.Domain.Migrations
{
    /// <inheritdoc />
    public partial class addSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "DisplayName", "Name", "Order" },
                values: new object[,]
                {
                    { 1, "Мы знаем, что хороший гардероб всегда должен развиваться в стремлении к лучшему. Мы знаем, что даже самые простые предметы всегда способны быть улучшенными. И мы знаем, что наша преданность инновациям жизни означает, что сосредоточиться на том, что будет дальше, является ключом к прогрессу.", "Женщины", "women", 1 },
                    { 2, "Мы знаем, что хороший гардероб всегда должен развиваться в стремлении к лучшему. Мы знаем, что даже самые простые предметы всегда способны быть улучшенными.", "Мужчины", "men", 2 },
                    { 3, "Мы понимаем, что по мере того, как маленькие растут, меняются и развиваются, вы должны убедиться, что их гардероб делает то же самое, чтобы не отставать. Исследуйте самые последние дополнения к диапазонам Qlouni для детей и детей", "Дети", "kids", 3 },
                    { 4, "Магазин Нового прибытия онлайн. Показывая последние коллекции и релизы от Qlouni", "Младенцы", "baby", 4 }
                });

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Id", "Hex", "Name", "Order" },
                values: new object[,]
                {
                    { 1, "#f3f3f3", "Белый", 1 },
                    { 2, "#18181a", "Чёрный", 2 }
                });

            migrationBuilder.InsertData(
                table: "Sizes",
                columns: new[] { "Id", "Name", "Order" },
                values: new object[,]
                {
                    { 1, "Xxxs", 1 },
                    { 2, "Xxs", 2 },
                    { 3, "Xs", 3 },
                    { 4, "S", 4 },
                    { 5, "M", 5 },
                    { 6, "L", 6 },
                    { 7, "Xl", 7 },
                    { 8, "Xxl", 8 },
                    { 9, "Xxxl", 9 }
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: 9);

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

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}

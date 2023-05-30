using Microsoft.EntityFrameworkCore;
using SHA3.Net;
using Store.Domain.Enums;
using Store.Domain.Models;
using Store.Domain.Models.ProductEntities;
using System.Security.Policy;
using System.Text;

namespace Store.Application.Infrastructure.Extensions
{
    public static class SeedExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category(1,"women", "Женщины", "Мы знаем, что хороший гардероб всегда должен развиваться в стремлении к лучшему. Мы знаем, что даже самые простые предметы всегда способны быть улучшенными. И мы знаем, что наша преданность инновациям жизни означает, что сосредоточиться на том, что будет дальше, является ключом к прогрессу.", 1),
                new Category(2, "men", "Мужчины", "Мы знаем, что хороший гардероб всегда должен развиваться в стремлении к лучшему. Мы знаем, что даже самые простые предметы всегда способны быть улучшенными.", 2),
                new Category(3, "kids", "Дети", "Мы понимаем, что по мере того, как маленькие растут, меняются и развиваются, вы должны убедиться, что их гардероб делает то же самое, чтобы не отставать. Исследуйте самые последние дополнения к диапазонам Qlouni для детей и детей", 3),
                new Category(4, "baby", "Младенцы", "Магазин Нового прибытия онлайн. Показывая последние коллекции и релизы от Qlouni", 4)
            );

            string hash;
            string salt = DateTime.Now.GetHashCode().ToString();
            using (var shaAlg = Sha3.Sha3256())
            {
                string inputPassHash = Convert.ToBase64String(shaAlg.ComputeHash(Encoding.UTF8.GetBytes("andreyandrey")));
                hash = Convert.ToBase64String(shaAlg.ComputeHash(Encoding.UTF8.GetBytes(inputPassHash + salt)));
            }
            modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Login = "admin",
                Email = "Admin@admin.com",
                Password = hash,
                Guid = Guid.NewGuid(),
                Salt = salt,
                UserRole = UserRole.Administrator,
                IsEmailConfirmed = true,
                RegistrationDate = DateTime.UtcNow
            });

            modelBuilder.Entity<Size>().HasData(
                new Size(1, "Xxxs", 1),
                new Size(2, "Xxs", 2),
                new Size(3, "Xs", 3),
                new Size(4, "S", 4),
                new Size(5, "M", 5),
                new Size(6, "L", 6),
                new Size(7, "Xl", 7),
                new Size(8, "Xxl", 8),
                new Size(9, "Xxxl", 9)
            );
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using E = StudentReviewManager.DAL.Models;

namespace StudentReviewManager.DAL.Data
{
    public class SeedData
    {
        public static async Task Seedasync(
            UserManager<E.User> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext dbcontext
        )
        {
            await SeedRole(roleManager);
            await SeedUsers(userManager);
        }

        private static async Task SeedUsers(UserManager<E.User> userManager)
        {
            if (await userManager.FindByNameAsync("admin@localhost.com") == null)
            {
                var admin = new E.User
                {
                    UserName = "admin@localhost.com",
                    Email = "admin@localhost.com",
                };
                var result = await userManager.CreateAsync(admin, "1q2w3e4rA!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }

        private static async Task SeedCities(ApplicationDbContext dbcontext)
        {
            #region cities
            string[] seedCities =
            [
                "Київ",
                "Харків",
                "Львів",
                "Дніпро",
                "Одеса",
                "Вінниця",
                "Ковель",
                "Луцьк",
                "Дніпро",
                "Кам'янське",
                "Кривий Ріг ",
                "Нікополь",
                "Павлоград",
                "Бахмут ",
                "Краматорськ",
                "Маріуполь",
                "Слов'янськ",
                "Житомир",
                "Берегове",
                "Мукачево",
                "Ужгород",
                "Хуст",
                "Бердянськ",
                "Запоріжжя",
                "Мелітополь",
                "Івано - Франківськ",
                "Коломия",
                "Косів ",
                "Біла Церква  ",
                "Бориспіль ",
                "Буча",
                "Ірпінь",
                "Київ",
                "Переяслав",
                "Яготин",
                "Кропивницький",
                "Дрогобич",
                "Дубляни",
                "Львів",
                "Миколаїв",
                "Ізмаїл",
                "Одеса",
                "Кременчук",
                "Лубни",
                "Полтава",
                "Дубно",
                "Острог",
                "Рівне ",
                "Глухів",
                "Конотоп",
                "Суми",
                "Шостка",
                "Бережани ",
                "Кременець",
                "Тернопіль",
                "Харків",
                "Нова Каховка",
                "Херсон",
                "Хмельницький",
                "Умань",
                "Черкаси",
                "Чернівці",
                "Ніжин",
                "Чернігів",
            ];
            #endregion
            if (await dbcontext.Cities.AnyAsync())
            {
                await dbcontext.Cities.AddRangeAsync();
                await dbcontext.SaveChangesAsync();
            }
        }

        private static async Task SeedRole(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var role = new IdentityRole { Name = "Admin" };
                var result = await roleManager.CreateAsync(role);
            }
            if (!await roleManager.RoleExistsAsync("User"))
            {
                var user = new IdentityRole { Name = "User" };
                var result = await roleManager.CreateAsync(user);
            }
        }
    }
}

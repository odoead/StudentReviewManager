using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentReviewManager.DAL.Models;
using E = StudentReviewManager.DAL.Models;

namespace StudentReviewManager.DAL.Data
{
    public class SeedData
    {
        public static async Task Seed(UserManager<E.User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbcontext)
        {
            await SeedRole(roleManager);
            await SeedUsers(userManager);
            await SeedCities(dbcontext);
            await SeedDegrees(dbcontext);
            await SeedSpecialities(dbcontext);
        }

        private static async Task SeedUsers(UserManager<E.User> userManager)
        {
            if (await userManager.FindByNameAsync("admin@localhost.com") == null)
            {
                var admin = new E.User { UserName = "admin@localhost.com", Email = "admin@localhost.com",Nickname="admin" };
                var result = await userManager.CreateAsync(admin, "1q2w3e4rA!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
            if (await userManager.FindByNameAsync("default.user@gmail.com") == null)
            {
                var admin = new E.User { UserName = "default.user@gmail.com", Email = "default.user@gmail.com",Nickname="default" };
                var result = await userManager.CreateAsync(admin, "1q2w3e4rA!QaW");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "User");
                }
            }
        }

        private static async Task SeedCities(ApplicationDbContext dbcontext)
        {
            #region cities
            string[] seedNames =
            [
                "Kyiv",
                "Kharkiv",
                "Lviv",
                "Dnipro",
                "Odesa",
                "Vinnytsia",
                "Kovel",
                "Lutsk",
                "Dnipro",
                "Kamianske",
                "Kryvyi Rih",
                "Nikopol",
                "Pavlohrad",
                "Bakhmut",
                "Kramatorsk",
                "Mariupol",
                "Sloviansk",
                "Zhytomyr",
                "Beregovo",
                "Mukachevo",
                "Uzhhorod",
                "Khust",
                "Berdiansk",
                "Zaporizhzhia",
                "Melitopol",
                "Ivano-Frankivsk",
                "Kolomyia",
                "Kosiv",
                "Bila Tserkva",
                "Boryspil",
                "Bucha",
                "Irpin",
                "Kyiv",
                "Pereyaslav",
                "Yagotyn",
                "Kropyvnytskyi",
                "Drohobych",
                "Dubliany",
                "Lviv",
                "Mykolaiv",
                "Izmail",
                "Odesa",
                "Kremenchuk",
                "Lubny",
                "Poltava",
                "Dubno",
                "Ostroh",
                "Rivne",
                "Hlukhiv",
                "Konotop",
                "Sumy",
                "Shostka",
                "Berezhany",
                "Kremenets",
                "Ternopil",
                "Kharkiv",
                "Nova Kakhovka",
                "Kherson",
                "Khmelnytskyi",
                "Uman",
                "Cherkasy",
                "Chernivtsi",
                "Nizhyn",
                "Chernihiv",
            ];
            #endregion
            List<City> seedCities = new List<City>();
            foreach (string name in seedNames)
            {
                seedCities.Add(new City { Name = name });
            }
            if (!await dbcontext.Cities.AnyAsync())
            {
                await dbcontext.Cities.AddRangeAsync(seedCities);
                await dbcontext.SaveChangesAsync();
            }
        }

        private static async Task SeedDegrees(ApplicationDbContext dbContext)
        {
            var degrees = new Degree[]
            {
                new Degree { Name = "Bachelor" },
                new Degree { Name = "Master" },
                new Degree { Name = "Junior" },
            };
            if (!await dbContext.Degrees.AnyAsync())
            {
                await dbContext.Degrees.AddRangeAsync(degrees);
                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedSpecialities(ApplicationDbContext dbContext)
        {
            var specs = new Specialty[]
            {
                new Specialty { Code = 125, Name = "Math" },
                new Specialty { Code = 167, Name = "Physics" },
                new Specialty { Code = 167, Name = "Ecology", },
            };
            if (!await dbContext.Specialties.AnyAsync())
            {
                await dbContext.Specialties.AddRangeAsync(specs);
                await dbContext.SaveChangesAsync();
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

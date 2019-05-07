using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;
using System.Threading.Tasks;
using WebApplication1.DAL;
using System;

namespace WebApplication1
{
    public class RoleInitializer
    {
        public static async System.Threading.Tasks.Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "Admin";
            string password = "1234";
            string user1Email = "Knaev@mail.ru";
            string user2Email = "Ribakov@mail.ru";
            string user3Email = "Sokolov@mail.ru";
            if (await roleManager.FindByNameAsync("Админ") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Админ"));
            }
            if (await roleManager.FindByNameAsync("Разработчик") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Разработчик"));
            }
            if (await roleManager.FindByNameAsync("Агент") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Агент"));
            }
            if (await roleManager.FindByNameAsync("Партнер") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Партнер"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail, DateImployment = DateTime.Now };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Админ");
                }
            }
            if (await userManager.FindByNameAsync(user1Email) == null)
            {
                User user1 = new User
                {   Email = user1Email,
                    UserName = "YuriyKnaev",
                    DateImployment = DateTime.Now,
                    Year = 1998,
                    FirmID = 2,
                    FirstName = "Юрий",
                    MiddleName = "Сергеевич",
                    LastName = "Кнаев"
                };
                IdentityResult result = await userManager.CreateAsync(user1, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user1, "Разработчик");
                }
            }
            if (await userManager.FindByNameAsync(user2Email) == null)
            {
                User user2 = new User
                {   Email = user2Email,
                    UserName = "RibakovIlya",
                    DateImployment = DateTime.Now,
                    Year = 1998,
                    FirmID = 3,
                    FirstName = "Илья",
                    MiddleName = "Александрович",
                    LastName = "Рыбаков"
                };
                IdentityResult result = await userManager.CreateAsync(user2, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user2, "Разработчик");
                }
            }
            if (await userManager.FindByNameAsync(user3Email) == null)
            {
                User user3 = new User
                {
                    Email = user3Email,
                    UserName = "SokolovVladislav",
                    DateImployment = DateTime.Now,
                    Year = 1998,
                    FirmID = 1,
                    FirstName = "Владислав",
                    MiddleName = "Олегович",
                    LastName = "Соколов"
                };
                IdentityResult result = await userManager.CreateAsync(user3, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user3, "Разработчик");
                }
            }
        }
    }
}

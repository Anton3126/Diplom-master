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
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail, DateImployment = DateTime.Now };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
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
                    LastName = "Кнаев",
                };
                IdentityResult result = await userManager.CreateAsync(user1, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user1, "user");
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
                    LastName = "Рыбаков",
                };
                IdentityResult result = await userManager.CreateAsync(user2, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user2, "user");
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
                    LastName = "Соколов",
                };
                IdentityResult result = await userManager.CreateAsync(user3, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user3, "user");
                }
            }
        }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero.Models;

namespace WebApplication10.Migrations
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@mail.ru";
            string password = "Qwerty1!";
            string fadminEmail = "Fadmin@mail.ru";
            string passwordF = "Qwerty1!";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("buyers") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("buyers"));
            }
            if (await roleManager.FindByNameAsync("fadmin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("fadmin"));
            }
            if (await roleManager.FindByNameAsync("fadmin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("fadmin"));
            }
            if (await roleManager.FindByNameAsync("supplier") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("supplier"));
            }

            if (await userManager.FindByNameAsync(fadminEmail) == null)
            {
                User fadmin = new User { Email = fadminEmail, UserName = fadminEmail };
                IdentityResult result = await userManager.CreateAsync(fadmin, passwordF);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(fadmin, "fadmin");
                }
            }
                if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}

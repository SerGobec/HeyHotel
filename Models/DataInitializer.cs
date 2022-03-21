using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyHotel.Models
{
    public class DataInitializer
    {
        public static async Task RoleInitialize(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Later I will delete it, and hide it more secure :)
            string adminGmail = "Sergobec@gmail.com";
            string adminPassword = "Qwert_123";

            if( await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await roleManager.FindByNameAsync("manager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("manager"));
            }
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if( await userManager.FindByNameAsync(adminGmail) == null )
            {
                User user = new User { Email = adminGmail, UserName = adminGmail };
                IdentityResult result = await userManager.CreateAsync(user, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "admin");
                }

            }
        }
    }
}

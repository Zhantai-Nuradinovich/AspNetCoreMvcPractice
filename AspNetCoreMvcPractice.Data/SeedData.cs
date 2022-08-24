using AspNetCoreMvcPractice.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<UserRole>>();
            var userManager = services.GetRequiredService<UserManager<User>>();
            await AddTestUsers(roleManager, userManager);
        }

        private static async Task AddTestUsers(RoleManager<UserRole> roleManager, UserManager<User> userManager)
        {
            var dataExists = roleManager.Roles.Any() || userManager.Users.Any();
            if (dataExists)
                return;

            await roleManager.CreateAsync(new UserRole("Admin"));
            await roleManager.CreateAsync(new UserRole("User"));

            var admin = new User()
            {
                Email = "admin@northwind.com",
                UserName = "Admin",
                FirstName = "Admin",
                LastName = "Test",
                CreatedAt = DateTimeOffset.UtcNow
            };

            var user = new User()
            {
                Email = "user@northwind.com",
                UserName = "User",
                FirstName = "User",
                LastName = "Test",
                CreatedAt = DateTimeOffset.UtcNow
            };

            await userManager.CreateAsync(admin, "Supersecret123!!");
            await userManager.AddToRoleAsync(admin, "Admin");
            await userManager.UpdateAsync(admin);

            await userManager.CreateAsync(user, "Supersecret123!!");
            await userManager.AddToRoleAsync(user, "User");
            await userManager.UpdateAsync(user);
        }
    }
}

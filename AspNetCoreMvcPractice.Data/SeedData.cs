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
            await AddTestUsers(
                services.GetRequiredService<RoleManager<UserRole>>(),
                services.GetRequiredService<UserManager<User>>());
        }

        private static async Task AddTestUsers(RoleManager<UserRole> roleManager, UserManager<User> userManager)
        {
            var dataExists = roleManager.Roles.Any() || userManager.Users.Any();
            if (dataExists)
                return;

            await roleManager.CreateAsync(new UserRole("Admin"));

            var user = new User()
            {
                Email = "admin@northwind.com",
                UserName = "Username",
                FirstName = "Admin",
                LastName = "Test",
                CreatedAt = DateTimeOffset.UtcNow
            };
            await userManager.CreateAsync(user, "Supersecret123!!");
            await userManager.AddToRoleAsync(user, "Admin");
            await userManager.UpdateAsync(user);
        }
    }
}

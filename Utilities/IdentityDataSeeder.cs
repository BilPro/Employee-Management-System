using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using IdentityRole = Microsoft.AspNetCore.Identity.IdentityRole;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

namespace Employee_Management_System.Utilities
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedDataAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Define roles you want to seed
            var roles = new[] { "Admin", "Manager", "User" };

            // Seed roles
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed a default admin user if it doesn't exist
            var adminUser = await userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };

                // Create the admin user with a secure password (ensure password meets requirements)
                var createResult = await userManager.CreateAsync(adminUser, "Admin@123");
                if (createResult.Succeeded)
                {
                    // Assign the "Admin" role to the user
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Seed an additional user with the "User" role.
            var regularUser = await userManager.FindByNameAsync("regularuser");
            if (regularUser == null)
            {
                regularUser = new IdentityUser
                {
                    UserName = "buser",
                    Email = "regularuser@example.com",
                    EmailConfirmed = true
                };

                var userResult = await userManager.CreateAsync(regularUser, "User@123");
                if (userResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(regularUser, "User");
                }
            }
        }
    }
}

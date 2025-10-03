using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace WebApplication1.Data
{
    public static class SeedRoles
    {
        public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            // 1️⃣ Seed Roles
            string[] roles = { "Admin", "HR", "Instructor", "Student" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!roleResult.Succeeded)
                    {
                        foreach (var error in roleResult.Errors)
                            Console.WriteLine($"Error creating role {role}: {error.Description}");
                    }
                }
            }

            // 2️⃣ Seed Admin User
            string adminEmail = "admin@site.com";
            string adminPassword = "Admin123!"; // Must satisfy Identity password rules
            string adminRole = "Admin";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,  // Use email as username
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var userResult = await userManager.CreateAsync(adminUser, adminPassword);
                if (userResult.Succeeded)
                {
                    var roleAssignResult = await userManager.AddToRoleAsync(adminUser, adminRole);
                    if (!roleAssignResult.Succeeded)
                    {
                        foreach (var error in roleAssignResult.Errors)
                            Console.WriteLine($"Error assigning role {adminRole} to admin: {error.Description}");
                    }
                }
                else
                {
                    foreach (var error in userResult.Errors)
                        Console.WriteLine($"Error creating admin user: {error.Description}");
                }
            }
        }
    }
}
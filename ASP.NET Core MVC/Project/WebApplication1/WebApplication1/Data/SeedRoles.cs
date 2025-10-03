using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace WebApplication1.Data
{
    public static class SeedRoles
    {
        public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
        {
            // ----------------------
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
        }
    }
}
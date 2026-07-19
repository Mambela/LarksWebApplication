using LarksWebApp.Models.Users;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace LarksWebApp.DAL
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Basic.ToString()));
        }

        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILoggerTrace logger)
        {
            var username = configuration["SuperAdmin:Username"];
            var email = configuration["SuperAdmin:Email"];
            var password = configuration["SuperAdmin:Password"];

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                logger.Warning("SuperAdmin seed skipped: SuperAdmin:Username/Email/Password are not configured.");
                return;
            }

            var user = await userManager.FindByEmailAsync(email);

            if (user != null)
            {
                return;
            }

            var defaultUser = new ApplicationUser
            {
                UserName = username,
                Email = email,
                FirstName = configuration["SuperAdmin:FirstName"] ?? "",
                LastName = configuration["SuperAdmin:LastName"] ?? "",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var result = await userManager.CreateAsync(defaultUser, password);

            if (!result.Succeeded)
            {
                logger.LogError("Failed to seed SuperAdmin user: " + string.Join("; ", result.Errors.Select(e => e.Description)));
                return;
            }

            await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Basic.ToString());
            await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Admin.ToString());
            await userManager.AddToRoleAsync(defaultUser, Enums.Roles.SuperAdmin.ToString());
        }

    }
}

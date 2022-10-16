using LarksWebApp.Models.Users;
using Microsoft.AspNetCore.Identity;

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

        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new ApplicationUser { 
                UserName  = "superadmin",
                Email= "superAdmin@larks.Africa",
                FirstName  = "Kevin",
                LastName = "Mambela",
                EmailConfirmed = true,
                PhoneNumberConfirmed= true
            
            };


            if (userManager.Users.All(u => u.Id != defaultUser.Id)) {

                var user = await userManager.FindByEmailAsync(defaultUser.Email);

                if (user == null) {


                    await userManager.CreateAsync(defaultUser, "4Kingmambel@");
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.SuperAdmin.ToString());
                }
            }
        }

    }
}

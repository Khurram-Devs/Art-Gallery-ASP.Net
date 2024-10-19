using Art_Gallery.Constants;
using Microsoft.AspNetCore.Identity;

namespace Art_Gallery.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();
            var logger = service.GetRequiredService<ILogger<DbSeeder>>();

            // Adding roles to DB
            var adminRoleResult = await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            if (adminRoleResult.Succeeded)
            {
                logger.LogInformation("Admin role created.");
            }

            var userRoleResult = await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));
            if (userRoleResult.Succeeded)
            {
                logger.LogInformation("User role created.");
            }

            // Create Admin User
            var admin = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true
            };

            var userInDb = await userMgr.FindByEmailAsync(admin.Email);

            if (userInDb is null)
            {
                var createResult = await userMgr.CreateAsync(admin, "#Admin123");
                if (createResult.Succeeded)
                {
                    logger.LogInformation("Admin user created.");
                    await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
                    logger.LogInformation("Admin user added to Admin role.");
                }
                else
                {
                    logger.LogError("Failed to create admin user.");
                }
            }
            else
            {
                logger.LogInformation("Admin user already exists.");
            }
        }

    }
}

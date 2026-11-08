
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using onine.core.Constants;
using onine.core.Entites.Identity;

namespace onine.Repository.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(
            UserManager<AppUser> userManager,
            IConfiguration configuration)
        {
            var email = configuration["AdminSeed:Email"];
            var password = configuration["AdminSeed:Password"];

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            var admin = await userManager.FindByEmailAsync(email);

            if (admin == null)
            {
                admin = new AppUser
                {
                    DisplayName = "Admin",
                    Email = email,
                    UserName = email
                };

                var result = await userManager.CreateAsync(
                    admin, password);

                if (!result.Succeeded)
                {
                    throw new Exception(
                        string.Join(", ",
                            result.Errors.Select(e => e.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(admin, AppRoles.Admin))
            {
                var roleResult = await userManager.AddToRoleAsync(
                    admin, AppRoles.Admin);

                if (!roleResult.Succeeded)
                {
                    throw new Exception(
                        string.Join(", ",
                            roleResult.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}

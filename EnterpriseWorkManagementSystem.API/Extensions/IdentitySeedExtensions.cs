using Microsoft.AspNetCore.Identity;

namespace EnterpriseWorkManagementSystem.API.Extensions
{
    public static class IdentitySeedExtensions
    {
        public static async Task SeedRolesAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles =
            [
                "Admin",
            "Manager",
            "Employee"
            ];

            foreach (var role in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);

                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}

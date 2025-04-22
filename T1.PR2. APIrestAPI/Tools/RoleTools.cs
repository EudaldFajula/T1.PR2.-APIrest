using Microsoft.AspNetCore.Identity;

namespace T1.PR2._APIrestAPI.Tools
{
    public static class RoleTools
    {
        public static async Task CreateInitialsRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] rols = { "Admin", "User" };

            foreach (var rol in rols)
            {
                if (!await roleManager.RoleExistsAsync(rol))
                {
                    await roleManager.CreateAsync(new IdentityRole(rol));
                }
            }
        }
    }
}

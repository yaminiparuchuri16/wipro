using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Role_Based_Product_Management.Models;


namespace Role_Based_Product_Manager.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userMgr = services.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Admin", "Manager" };
            foreach (var r in roles)
            {
                if (!await roleMgr.RoleExistsAsync(r))
                    await roleMgr.CreateAsync(new IdentityRole(r));
            }

            // Admin
            var admin = await userMgr.FindByNameAsync("admin");
            if (admin == null)
            {
                admin = new ApplicationUser { UserName = "admin", Email = "admin@example.com", EmailConfirmed = true };
                await userMgr.CreateAsync(admin, "Admin@123");
                await userMgr.AddToRoleAsync(admin, "Admin");
            }

            // Manager
            var mgr = await userMgr.FindByNameAsync("manager1");
            if (mgr == null)
            {
                mgr = new ApplicationUser { UserName = "manager1", Email = "manager1@example.com", EmailConfirmed = true };
                await userMgr.CreateAsync(mgr, "Manager@123");
                await userMgr.AddToRoleAsync(mgr, "Manager");
            }
        }
    }
}

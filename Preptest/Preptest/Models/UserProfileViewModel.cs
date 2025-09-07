using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

public static class DbInitializer
{
    public static async Task SeedRolesAndUsers(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = { "Admin", "User" };
        foreach (var r in roles)
            if (!await roleManager.RoleExistsAsync(r))
                await roleManager.CreateAsync(new IdentityRole(r));

        var admin = await userManager.FindByNameAsync("admin");
        if (admin == null)
        {
            admin = new ApplicationUser { UserName = "admin", Email = "admin@example.com", EmailConfirmed = true };
            if ((await userManager.CreateAsync(admin, "Admin@123")).Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }

        var user = await userManager.FindByNameAsync("user1");
        if (user == null)
        {
            user = new ApplicationUser { UserName = "user1", Email = "user1@example.com", EmailConfirmed = true };
            if ((await userManager.CreateAsync(user, "User@123")).Succeeded)
                await userManager.AddToRoleAsync(user, "User");
        }
    }
}
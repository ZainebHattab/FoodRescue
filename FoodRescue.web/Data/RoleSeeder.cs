using Microsoft.AspNetCore.Identity;

namespace FoodRescue.web.Data;

public static class RoleSeeder
{
    public static readonly string[] Roles = { "Admin", "StoreOwner", "Customer" };

    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var config = services.GetRequiredService<IConfiguration>();
        var logger = services.GetRequiredService<ILogger<ApplicationUser>>();

        foreach (var role in Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var email = config["Seed:AdminEmail"];
        var password = config["Seed:AdminPassword"];
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning("Seed:AdminEmail or Seed:AdminPassword is missing; no admin created.");
            return;
        }

        if (await userManager.FindByEmailAsync(email) is null)
        {
            var admin = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FullName = "Administrator"
            };
            var result = await userManager.CreateAsync(admin, password);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
            else
                logger.LogError("Admin creation failed: {Errors}",
                    string.Join("; ", result.Errors.Select(e => e.Description)));
        }
    }
}

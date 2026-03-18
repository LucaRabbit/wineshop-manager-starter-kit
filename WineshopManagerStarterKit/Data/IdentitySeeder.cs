using Microsoft.AspNetCore.Identity;

namespace WineshopManagerStarterKit.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
        // Create roles if they don't exist
        string[] roles = { "Administrator", "Contributor" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Create admin user if it doesn't exist
        var adminEmail = "admin@wineshop.fr";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "password"); // Mot de passe par défaut pour l'utilisateur admin (doit être changé en production)

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Administrator"); // Ajouter l'utilisateur admin au rôle "Administrator"
            }
        }
    }
}
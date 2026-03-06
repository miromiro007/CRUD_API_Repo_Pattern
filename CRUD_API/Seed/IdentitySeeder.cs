using CRUD_API.Models;
using Microsoft.AspNetCore.Identity;

namespace CRUD_API.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedAdminUser(IServiceProvider serviceProvider) 
        {
            // Récupération des services Identity
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Liste des rôles à créer
            string[] roles = new string[] { "Admin", "User" };

            // Création des rôles s'ils n'existent pas

            foreach (var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                  await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            //Username 
            string adminUsername = "admin";
            // Email et mot de passe de l'utilisateur admin
            string adminEmail = "admin@gmail.com";
            // Vérification si l'utilisateur admin existe déjà
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null) 
            {
                var admin = new AppUser
                {
                    UserName = adminUsername,
                    Email = adminEmail,
                    FullName = "Admin User",
                    CreatedDate = DateTime.UtcNow
                };

                // Création de l'utilisateur admin avec un mot de passe sécurisé
                string adminPassword = "Admin123!"; // Assurez-vous de choisir un mot de passe fort

                // Création du compte admin avec mot de passe

                var result = await userManager.CreateAsync(admin, adminPassword);

                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
                var roleResult = await userManager.AddToRoleAsync(admin, "Admin");
                if (!roleResult.Succeeded)
                {
                    throw new Exception($"Failed to assign admin role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
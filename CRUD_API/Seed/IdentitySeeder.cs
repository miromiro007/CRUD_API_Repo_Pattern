using CRUD_API.Models;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace CRUD_API.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedAdminUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = new string[] { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    // ✅ LOG STRUCTURE : On passe le nom du rôle en tant que propriété
                    Log.Information("Rôle créé avec succès : {RoleName}", role);
                }
            }

            string adminUsername = "admin";
            string adminEmail = "admin@gmail.com";
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

                string adminPassword = "Admin123!";
                var result = await userManager.CreateAsync(admin, adminPassword);

                if (result.Succeeded)
                {
                    // ✅ LOG STRUCTURE : On logue l'objet admin (sans le password !) 
                    // pour avoir ses détails en JSON
                    Log.Information("Utilisateur Admin créé : {@AdminDetails}", new { admin.UserName, admin.Email });

                    var roleResult = await userManager.AddToRoleAsync(admin, "Admin");
                    if (roleResult.Succeeded)
                    {
                        Log.Information("Rôle 'Admin' assigné à l'utilisateur {Email}", adminEmail);
                    }
                }
                else
                {
                    // ❌ LOG D'ERREUR : On capture les erreurs d'Identity
                    Log.Error("Échec de la création de l'Admin. Erreurs : {@Errors}", result.Errors);
                }
            }
            else
            {
                // Log de diagnostic léger
                Log.Debug("Le seed de l'admin a été sauté : l'utilisateur {Email} existe déjà.", adminEmail);
            }
        }
    }
}
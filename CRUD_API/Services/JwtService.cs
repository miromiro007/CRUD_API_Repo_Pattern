using CRUD_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;

namespace CRUD_API.Services
{
    public class JwtService
    {
        // IConfiguration permet de lire les valeurs du fichier appsettings.json
        // Exemple : JWT:SecretKey, JWT:Issuer, JWT:Audience
        private readonly IConfiguration _iConfiguration;
        // UserManager est un service fourni par ASP.NET Identity
        // Il permet de gérer les utilisateurs : création, mot de passe, rôles, claims, etc.
        private readonly UserManager<AppUser> _userManager;

        // Constructeur : injection de dépendances (Dependency Injection)
        // ASP.NET injecte automatiquement IConfiguration et UserManager
        public JwtService(IConfiguration iConfiguration, UserManager<AppUser> userManager)
        {
            _iConfiguration = iConfiguration;
            _userManager = userManager;
        }

        // Méthode principale qui génère un token JWT pour un utilisateur
        public async Task<string> GenerateToken(AppUser user)
        {
            // Création d'une liste de Claims
            // Un Claim = information sur l'utilisateur stockée dans le token
            var claims = new List<Claim>
            {
                // Nom de l'utilisateur
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                // Identifiant unique de l'utilisateur
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                // JTI = JWT ID (identifiant unique du token)
                // utile pour la sécurité et éviter les duplications
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Récupération des rôles de l'utilisateur depuis la base de données
            var userRoles = await _userManager.GetRolesAsync(user);

            // Ajout de chaque rôle dans les claims
            // Cela permet de faire : [Authorize(Roles="Admin")]
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Création de la clé secrète utilisée pour signer le token
            // Elle est récupérée depuis appsettings.json
            var key = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(_iConfiguration["JWT:SecretKey"])
            );

            // Création des credentials de signature
            // HmacSha256 est l'algorithme utilisé pour signer le token
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Crée le token JWT avec les claims, l'expiration, l'issuer, l'audience et les credentials
            var token = new JwtSecurityToken(
                issuer: _iConfiguration["JWT:Issuer"],
                audience: _iConfiguration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddSeconds(15),
                signingCredentials: creds
            );

            // Retourne le token sous forme de chaîne
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public Task<string> GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Task.FromResult(Convert.ToBase64String(randomNumber));
            }
        }


        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            // Validation des paramètres de validation du token
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                ValidIssuer = _iConfiguration["JWT:Issuer"],
                ValidAudience = _iConfiguration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(_iConfiguration["JWT:SecretKey"])
                )
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken != null)
                if (jwtSecurityToken == null ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

            return principal;
        }
    }
}
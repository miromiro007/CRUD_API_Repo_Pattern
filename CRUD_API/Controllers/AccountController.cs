using CRUD_API.DTOs;
using CRUD_API.Models;
using CRUD_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRUD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtService _jwtService;

        public AccountController(UserManager<AppUser> userManager, JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        // REGISTER USER
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto user)
        {
            //creation utilisateur
            var appUser = new AppUser
            {
                UserName = user.Email,
                Email = user.Email
            };

            //Creation dans la base de données
            var result = await _userManager.CreateAsync(appUser, user.Password);
            var reultRole = await _userManager.AddToRoleAsync(appUser, "User");

            return Ok(result);

        }

        // LOGIN USER
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            // chercher utilisateur
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return Unauthorized("Invalid credentials");

            // vérifier password
            var validPassword = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!validPassword)
                return Unauthorized("Invalid credentials");

            // générer token JWT
            var accessToken = await _jwtService.GenerateToken(user);
            var refreshToken = await _jwtService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                accessToken = accessToken,
                refreshToken = refreshToken,
                expiration = DateTime.UtcNow.AddSeconds(15)
            });
        }

        // REFRESH TOKEN
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenRefreshRequestDto dto)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(dto.AccessToken);
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return BadRequest("Invalid access token");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null ||
                user.RefreshToken != dto.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid refresh token");
            }

            var newAccessToken = await _jwtService.GenerateToken(user);
            var newRefreshToken = await _jwtService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _userManager.UpdateAsync(user);

            return Ok("Logged out successfully");
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("madeHimAdmin")]
        public async Task<IActionResult> MadeHimAdmin(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.AddToRoleAsync(user, "Admin");

            return Ok(result);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody]  UpdateUserDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Utilisateur non authentifié ezaezaeaz");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound("Utilisateur introuvable");
                
            user.FullName = string.IsNullOrWhiteSpace(dto.FullName) ? user.FullName : dto.FullName;
            user.PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber) ? user.PhoneNumber : dto.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != user.Email)
            {
                var existingUser = await _userManager.FindByEmailAsync(dto.Email);
                if (existingUser != null && existingUser.Id != user.Id)
                    return BadRequest("Email déjà utilisé");

                var setEmailResult = await _userManager.SetEmailAsync(user, dto.Email);
                if (!setEmailResult.Succeeded)
                    return BadRequest(setEmailResult.Errors);

                var setUserNameResult = await _userManager.SetUserNameAsync(user, dto.Email);
                if (!setUserNameResult.Succeeded)
                    return BadRequest(setUserNameResult.Errors);
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new
            {
                message = "Profil mis à jour avec succès"
            });
        }
    }

    }
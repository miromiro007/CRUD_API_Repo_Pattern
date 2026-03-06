using Microsoft.AspNetCore.Identity;

namespace CRUD_API.Models
{
    public class AppUser:IdentityUser
    {
        public string? FullName { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime? CreatedDate { get; set; }
        = DateTime.Now;


    }
}

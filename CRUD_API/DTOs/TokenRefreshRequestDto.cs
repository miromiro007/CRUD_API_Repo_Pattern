namespace CRUD_API.DTOs
{
    public class TokenRefreshRequestDto
    {

        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; }
    }
}

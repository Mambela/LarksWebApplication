namespace LarksWebApp.DTOs.DTO_User
{
    public class LoginResponseDTO
    {
        public string Username { get; set; }

        public string FullName { get; set; }

        public string Token { get; set; }

        public DateTime Expiration { get; set; }

        public string RefreshToken { get; set; }
    }
}

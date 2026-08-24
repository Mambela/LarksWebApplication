using System.ComponentModel.DataAnnotations;

namespace LarksWebApp.DTOs.DTO_User
{
    public class RefreshTokenRequestDTO
    {
        [Required(ErrorMessage = "A refresh token is required")]
        public string RefreshToken { get; set; }
    }
}

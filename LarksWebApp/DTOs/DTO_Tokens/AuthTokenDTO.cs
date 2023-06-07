using LarksWebApp.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace LarksWebApp.DTOs.DTO_Tokens
{
    public class AuthTokenDTO
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public string Value { get; set; }

        public ApplicationUser User { get; set; }
        [MaxLength(50)]
        public string UserId { get; set; }

        public DateTime ExpiryDate { get; set; }


        public DateTime IssueDate { get; set; }
    }
}

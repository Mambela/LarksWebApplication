using LarksWebApp.Model.BaseModels;
using LarksWebApp.Models.Users;

namespace LarksWebApp.Models
{
    public class HooksModel:IdentityBaseEntity
    {
        
        public string Owner { get; set; }

        public string EndPoint { get; set; }

        public string Method { get; set; }

        public bool IsActive { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}

using LarksWebApp.Model.BaseModels;
using LarksWebApp.Models.Users;

namespace LarksWebApp.DTOs.DTO_Addresses
{
    public class HooksDTO :IdentityBaseEntity
    {
        public Guid? Id { get; set; }

        public string Designation { get; set; }

        public string Owner { get; set; }

        public string EndPoint { get; set; }

        public string Method { get; set; }

        public bool IsActive { get; set; }

        public string ApplicationUserId { get; set; }    
 
    }
}

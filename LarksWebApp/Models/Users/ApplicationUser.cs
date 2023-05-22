using Microsoft.AspNetCore.Identity;

namespace LarksWebApp.Models.Users
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public IList<HooksModel> Hooks {get; set; }
    }
}

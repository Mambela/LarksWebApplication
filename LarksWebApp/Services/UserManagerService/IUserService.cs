using LarksWebApp.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LarksWebApp.Services.UserManager
{
    public interface IUserService
    {
         string GetUserId(ClaimsIdentity UserIdentity);

        Task<JwtSecurityToken> GenerateUserTokenAsync(ApplicationUser user);

    }

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        public UserService(UserManager<ApplicationUser> _usermanager,IConfiguration _configuration)
        {
            userManager = _usermanager;
            configuration = _configuration;
        }
        
        public async Task<JwtSecurityToken> GenerateUserTokenAsync(ApplicationUser user)
        {
            try {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };


                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));


                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddDays(14),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return token;
            }
            catch (Exception ex)
            {

                return null;
            }
            
        }

        public string GetUserId(ClaimsIdentity UserIdentity )
        {
            string userId = "";

            var claimsIdentity = UserIdentity;
            if (claimsIdentity != null)
            {
                // the principal identity is a claims identity.
                // now we need to find the NameIdentifier claim
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);


                if (userIdClaim != null)
                {

                    // var userIdValue = userIdClaim.Value;
                    userId = userIdClaim.Value.ToString();
                }
                return userId;
            }

            return null;
        }

    }
}

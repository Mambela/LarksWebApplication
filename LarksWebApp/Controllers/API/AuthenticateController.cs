using LarksWebApp.DTOs.DTO_User;
using LarksWebApp.Models.Users;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private ILoggerTrace _serviceLogger;
        private readonly IConfiguration _configuration;

        public AuthenticateController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILoggerTrace serviceLogger,IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _serviceLogger = serviceLogger;
            _configuration = configuration;
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        { 
            try
            {
                var user = await _userManager.FindByEmailAsync(loginModel.Username);

                if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
                {

                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginModel.Username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };


                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));


                    var token = new JwtSecurityToken(
                        expires:DateTime.Now.AddDays(3),
                        claims:authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey,SecurityAlgorithms.HmacSha256)
                        );


                    return Ok(new
                    {   
                        username = loginModel.Username,
                        fullName = user.FirstName+" "+user.LastName,
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                _serviceLogger.LogError("An error occurred while trying to login " + ex.Message);
                return BadRequest(new {errors = "Error while attempting to login"});
            }
        }



        
    }
}

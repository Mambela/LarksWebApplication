using LarksWebApp.Models.Users;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetupController : ControllerBase
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private ILoggerTrace serviceLogger;

        public SetupController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,ILoggerTrace logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            serviceLogger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<IdentityRole>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public IActionResult GetAllRoles()
        {
            try
            {
                var roles = _roleManager.Roles.ToList();

                return Ok(roles);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Error while retrieving roles " + ex.Message);
                return BadRequest(new { error = "Error while trying to retrieve Roles" });
                
            }
        }

       
    }
}

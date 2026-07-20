using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TesterAPIController : ControllerBase
    {

        [HttpGet(Name = "HealthTesterapi")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult GetTest()
        {
            return Ok("Test controller API FOLder");
        }
    }
}

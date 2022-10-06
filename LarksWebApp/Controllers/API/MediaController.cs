using LarksWebApp.Repository.LocationRepository;
using LarksWebApp.Services.FileManagement;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {

        //This controller is strictly responsible for returning Media to the user 
       //It will return an image based on the string that is passed to it.
        private readonly IFileManagement _fileManagement;
        private readonly ILocationRepository locationRepo;
        private readonly ILoggerTrace serviceLogger;

        private readonly IWebHostEnvironment webHostEnvironment;


        public MediaController(IFileManagement fileManagement,ILocationRepository locationRepository, ILoggerTrace serviceLogger, IWebHostEnvironment webHostEnvironment)
        {
            _fileManagement = fileManagement;
            locationRepo = locationRepository;
            this.serviceLogger = serviceLogger;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET api/<MediaController>/5
        [HttpGet("LocationImages")]
        public ActionResult GetLocationImages(string filename, string Category)
        {
            try
            {
                string imageRoot = "\\Files\\Images\\Locations\\" + Category + "\\" + filename;

                string _rootImage = webHostEnvironment.WebRootPath + imageRoot.ToLower();

                Byte[] imageInByte = System.IO.File.ReadAllBytes(_rootImage);

                return File(imageInByte,"image/jpeg");
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Error while trying to retrieve image"+ ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while attempting to get image");
            }
        }

      
    }
}

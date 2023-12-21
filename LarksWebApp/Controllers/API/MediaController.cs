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
        public ActionResult GetLocationImages(string filename)
        {
            try
            {
                string resourceRoot = "Files";
                string types = "Images";
                string locations = "Locations";
                string _rootImage =Path.Combine(Directory.GetCurrentDirectory(),webHostEnvironment.WebRootPath,resourceRoot,types,locations,filename);
                Console.WriteLine(_rootImage);



                if (System.IO.File.Exists(_rootImage))
                {
                    Byte[] imageInByte = System.IO.File.ReadAllBytes(_rootImage);
                    return File(imageInByte, "image/jpeg");
                }
                else {
                    serviceLogger.LogError($"Crime image file not found: {filename}");
                    return NotFound();
                }
                
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Error while trying to retrieve image"+ ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while attempting to get image");
            }
        }

      
    }
}

using AutoMapper;
using LarksWebApp.DTOs.DTO_Addresses;
using LarksWebApp.DTOs.DTO_Location;
using LarksWebApp.Repository.LocationRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        ///All locations must ahave an Address  aDAd
        private readonly ILocationRepository locationRepo;
        private readonly ILoggerTrace logger;
        private readonly IMapper _mapper;


        public LocationController(ILocationRepository repository, ILoggerTrace logger, IMapper mapper)
        {
            this.locationRepo = repository;
            this.logger = logger;
            _mapper = mapper;
        }

        [HttpGet("NeighbourhoodLocations")]
        public IActionResult GetNeighBourhoodLocations(int NeighbourhoodId) {

            try
            {

                var neighbourhoodLocationList = locationRepo.GetNeighbourhoodLocations(NeighbourhoodId);


                var neighbourhoodLocationListDTO = new List<LocationDTO>();


                foreach (var location in neighbourhoodLocationList)
                {
                    var images = new List<string>();


                    foreach (var image in location.Images)
                        images.Add(image.ImagePath);


                    var locationObj = new LocationDTO {
                        AdditionalInformation = location.AdditionalInformation,
                        AddressId = location.AddressId,
                        CategoryId = location.CategoryId,
                        Category = location.Category.Designation,
                        SubcategoryId = location.SubcategoryId,
                        Subcategory = location.Subcategory.Designation,
                        Images= images
                    };


                    neighbourhoodLocationListDTO.Add(locationObj);
                }


                return Ok(neighbourhoodLocationListDTO);

            }
            catch (Exception ex)
            {
                logger.LogError("Error while fetching the locations in a Neighbourhood " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while trying to fetch locations");
                
            }
        }



        ///Locations By Neighbourhood 


        ///Location by Category 
        [HttpGet("CategoryLocations")]
        public IActionResult GetlocationinCategory(int categoryId)
        {
            try
            {
                var neighbourhoodLocationList = locationRepo.GetCategoryLocations(categoryId);



                var neighbourhoodLocationListDTO = _mapper.Map<List<LocationDTO>>(neighbourhoodLocationList);


                return Ok(neighbourhoodLocationListDTO);
            }
            catch (Exception ex)
            {
                logger.LogError("Error while fetching the locations in a category " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while trying to fetch locations"); ;
            }
 
        }


        ///Location by subCategory 
        [HttpGet("SubcategoryLocations")]
        public IActionResult GetlocationInSubCategory(int subcategoryId) {

            try
            {
                var neighbourhoodLocationList = locationRepo.GetSubcategoryLocations(subcategoryId);



                var neighbourhoodLocationListDTO = _mapper.Map<List<LocationDTO>>(neighbourhoodLocationList);


                return Ok(neighbourhoodLocationListDTO);
            }
            catch (Exception ex)
            {
                logger.LogError("Error while fetching the locations in a category " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while trying to fetch locations"); ;
            }
        }


        ///Location near a POINT 


        ///Location by Proximity 


    }
}

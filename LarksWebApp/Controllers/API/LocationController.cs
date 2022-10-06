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
                        Id =location.Id,
                        Designation = location.Designation,
                        AdditionalInformation = location.AdditionalInformation,
                        AddressId = location.AddressId,
                        CategoryId = location.CategoryId,
                        StreetName = location.Address.Street.Designation,
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


        ///Data Table Locations In Neighbourhood
        ///

        [Route("[action]")]
        [HttpPost]
        public IActionResult LocationsInNeighbourhoodDT(int neighbourhoodId)
        {
            try
            {
                var neighbourhoodLocationList = locationRepo.GetNeighbourhoodLocations(neighbourhoodId);

                var neighbourhoodLocationListDTO = new List<LocationDTO>();

                foreach (var location in neighbourhoodLocationList)
                {
                   
                    var locationObj = new LocationDTO
                    {
                        Designation = location.Designation,
                        AdditionalInformation = location.AdditionalInformation,
                        AddressId = location.AddressId,
                        CategoryId = location.CategoryId,
                        StreetName = location.Address.Street.Designation,
                        Category = location.Category.Designation,
                        SubcategoryId = location.SubcategoryId,
                        Subcategory = location.Subcategory.Designation                        
                    };

                    neighbourhoodLocationListDTO.Add(locationObj);
                }

                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][Address]"].FirstOrDefault() + "][Address]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 10;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    neighbourhoodLocationListDTO = (List<LocationDTO>)neighbourhoodLocationListDTO.OrderBy(c => c.Designation);
                }


                if (!string.IsNullOrEmpty(searchValue))
                {
                    neighbourhoodLocationListDTO = (List<LocationDTO>)neighbourhoodLocationListDTO.Where(m => m.Designation.Contains(searchValue)
                                                || m.Designation.Contains(searchValue));

                }

                recordsTotal = neighbourhoodLocationListDTO.Count();
                var data = neighbourhoodLocationListDTO.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                logger.LogError("Error in location in neighbourhood " + ex.Message);

                return BadRequest();

            }            

        }


        ///Location near a POINT 


        ///Location by Proximity 


    }
}

using AutoMapper;
using LarksWebApp.DTOs;
using LarksWebApp.DTOs.DTO_Addresses;
using LarksWebApp.DTOs.DTO_Category;
using LarksWebApp.DTOs.DTO_Location;
using LarksWebApp.DTOs.DTO_Subcategory;
using LarksWebApp.DTOs.PaginationDTO;
using LarksWebApp.Models;
using LarksWebApp.Repository.CategoryRepository;
using LarksWebApp.Repository.CountryRepository;
using LarksWebApp.Repository.LocationRepository;
using LarksWebApp.Services.FileManagement;
using LarksWebApp.Services.Loggers;
using LarksWebApp.Services.PaginationService;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ICountryRepository _countryRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IFileManagement _fileManagement;
        private readonly ILoggerTrace logger;
        private readonly IMapper _mapper;
        private readonly IUriService uriService;

        public LocationController(ILocationRepository repository, ILoggerTrace logger, IMapper mapper, 
            ICountryRepository countryRepo,
            ICategoryRepository categoryRepo,
            IFileManagement fileManagement,
            IUriService _uriService)
        {

            _countryRepo = countryRepo;
            _categoryRepo = categoryRepo;
            _fileManagement = fileManagement;
            this.locationRepo = repository;
            this.logger = logger;
            _mapper = mapper;
            uriService = _uriService;
        }

        ///Locations By Neighbourhood 
        [HttpGet("NeighbourhoodLocations")]
        public IActionResult GetNeighBourhoodLocations(int suburbId) {

            try
            {

                var neighbourhoodLocationList = locationRepo.GetSuburbLocations(suburbId);


                var neighbourhoodLocationListDTO = new List<LocationDTO>();


                foreach (var location in neighbourhoodLocationList)
                {
                    var images = new List<string>();


                    foreach (var image in location.Images)
                        images.Add(image.ImagePath);


                    var addressDTO = new ViewAddressDTO
                    {
                        Id = (int)location.AddressId,
                        Municipality = location.Municipality.Designation,
                        Neighbourhood = location.Suburb.Designation,
                        Coordinates = new DTOs.BaseCoordinatesDTO 
                        {
                            lat = location.Lat,
                            lng = location.Lng, 
                        },
                        FullAddress =  ", " + location.Suburb.Designation + ", " + location.Designation
                    };

                    var locationObj = new LocationDTO
                    {
                        Id = location.Id,
                        Designation = location.Designation,
                        AdditionalInformation = location.AdditionalInformation,
                        //AddressId = location.AddressId,
                        Address = addressDTO,
                        CategoryId = (int)location.CategoryId,
                        //StreetName = location.Address.Street.Designation,
                        Category = location.Category.Designation,
                        //SubcategoryId = location.SubcategoryId,
                        //Subcategory = location.Subcategory.Designation,
                        Images = images
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

 
        ///Location by Category 
        [HttpGet("CategoryLocations")]
        public IActionResult GetlocationinCategory(int categoryId)
        {
            try
            {
                var neighbourhoodLocationList = locationRepo.GetCategoryLocations(categoryId);

                //var neighbourhoodLocationListDTO = _mapper.Map<List<LocationDTO>>(neighbourhoodLocationList);
                var neighbourhoodLocationListDTO = new List<LocationDTO>();

                foreach (var location in neighbourhoodLocationList)
                {
                    neighbourhoodLocationListDTO.Add(
                        new LocationDTO
                        {
                            AdditionalInformation = location.AdditionalInformation,
                            Category = location.Category.Designation,
                            Designation = location.Designation,
                            CategoryId = (int)location.CategoryId,
                            Subcategory = location.LocalSubcategories.Select(x =>
                                new SubCategoryDTO 
                                    { CategoryId = x.Subcategories.CategoryId,
                                      Designation = x.Subcategories.Designation,
                                      Id = x.Subcategories.Id,
                                    }),
                            Id = location.Id,
                            Images = location.Images.Select(s => s.ImagePath).ToList(),
                            Lat =location.Lat,
                            Lng = location.Lng
                        });
                }


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
                var neighbourhoodLocationList = locationRepo.GetSuburbLocations(neighbourhoodId);

                var neighbourhoodLocationListDTO = new List<LocationDTO>();

                foreach (var location in neighbourhoodLocationList)
                {
                   
                    var locationObj = new LocationDTO
                    {
                        Designation = location.Designation,
                        AdditionalInformation = location.AdditionalInformation,
                        //AddressId = location.AddressId,
                        CategoryId = (int)location.CategoryId,
                        Category = location.Category.Designation,
                        //SubcategoryId = location.SubcategoryId,
                        Subcategory = location.Category.Subcategories.Select(s => new SubCategoryDTO { Designation = s.Designation, Id = s.Id })                        
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


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("newLocation")]
        public async Task<IActionResult> CreateLocation([FromForm] Create_Location_DTO locationCollection) 
        {

            try
            {

                if (!ModelState.IsValid)
                {
                    var listofCountry = _countryRepo.GetAll();

                    locationCollection.ListofCountryDTO = _mapper.Map<List<CountryDTO>>(listofCountry);

                    locationCollection.ListOfCategories = _mapper.Map<List<CategoryDTO>>(_categoryRepo.GetAllCategories("en"));

                    return BadRequest();
                }

                var savedImages = new List<string>();

                var LocationImageList = new List<LocationImageModel>();

                if(locationCollection.Files != null)
                { 
                    if (locationCollection.Files.Count > 0)
                    {
                        var _categoryObj = _categoryRepo.GetCategoryById((locationCollection.searchCategoryId_hidden != 0)? locationCollection.searchCategoryId_hidden:locationCollection.CategoryId);

                        savedImages = await _fileManagement.MultipleFileUploads(locationCollection.Files, "Locations", _categoryObj.Designation);
                    }
                }


                // Save the Location first then use the object.

                var locationObj = new LocationModel
                {
                    AddressId = (locationCollection.searchAddress_hidden != 0)? locationCollection.searchAddress_hidden :0,
                    CategoryId = (locationCollection.searchCategoryId_hidden != 0) ? locationCollection.searchCategoryId_hidden : locationCollection.CategoryId,
                    MunicipalityId = locationCollection.MunicipalityID,
                    CityId = locationCollection.CityId,
                    SuburbId = locationCollection.SuburbId,
                    Designation = locationCollection.Designation,
                    AdditionalInformation = locationCollection.AdditionalInfo,
                    Lat = (decimal)locationCollection.Lat,
                    Lng = (decimal)locationCollection.Lng
                    
                };


                if (await locationRepo.CreateLocation(locationObj) == null)
                {
                    return BadRequest();
                }


                var LocationSubcategory = locationCollection.SubCategories.Select(id => new LocationModelSubcategoryModel { LocationId = locationObj.Id, SubcategoriesId = id });

                if (!await locationRepo.Createlocationsubcategory(LocationSubcategory))
                {
                    return BadRequest();
                }




                foreach (var image in savedImages)
                {

                    var locatinImageModel = new LocationImageModel
                    {
                        ImagePath = image,
                        LocationId = locationObj.Id

                    };

                    LocationImageList.Add(locatinImageModel);
                }


                if (await locationRepo.CreateLocationImage(LocationImageList) == null)
                {
                    return BadRequest();
                }
              
                locationCollection.Id = locationObj.Id;

                return StatusCode(StatusCodes.Status201Created,locationCollection);
            }
            catch (Exception ex)
            {
                logger.LogError("Error thrown when creating location " + ex.Message);
                return BadRequest();
            }


        }


        ///Location near a POINT 
        [HttpGet]
        [Route("locationbydistance")]
        public async Task<IActionResult> GetLocations([FromQuery] decimal lat, decimal lng, double distance,[FromQuery] PaginationFilter paginationFilter)
        {
            try
            {
                var locationsObjList = locationRepo.LocationToInDistanceAsync(lat, lng, distance, paginationFilter);

                int totalRecords = locationRepo.LocationCount(lat, lng, distance, paginationFilter, "SEARCHGEODISTANCE");
                string searchParams = $"?distance={distance}&lat={lat}&lng={lng}";
                var route = Request.Path.Value+searchParams;

                var locationsDTOList = new List<ViewLocationDTO>();
                

                foreach (var location in locationsObjList) 
                {
                    var locationObjDTO = _mapper.Map<ViewLocationDTO>(location);

                    var locationSubCategories = new List<SubCategoryDTO>();

                    locationObjDTO.Subcategories = location.LocalSubcategories.Select(s =>new SubCategoryDTO 
                    { Id = s.Subcategories.Id,Designation = s.Subcategories.Designation, Lng = s.Subcategories.Lng}).ToList();
                    locationObjDTO.CategoryId = (int)location.CategoryId;
                    locationObjDTO.Lat = location.Lat;
                    locationObjDTO.Lng = location.Lng;
                    locationObjDTO.Category = location.Category.Designation;
                    locationObjDTO.Distance = location.Distance;
                    locationObjDTO.Suburb = location.Suburb.Designation;
                    locationObjDTO.City = location.City.Designation;
                    locationObjDTO.Municipality = location.Municipality.Designation;
                    locationObjDTO.Images = location.Images.Select(s => s.ImagePath).ToList();
                    locationsDTOList.Add(locationObjDTO);

                }


                var validFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);

                var pagedResponse = PaginationHelper.CreatePageResponse<ViewLocationDTO>(locationsDTOList, validFilter,totalRecords,uriService,route);

                return Ok(pagedResponse);
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to retrieve location error ->" + ex.Message);
                return BadRequest();
            }
        }

        ///Location by Proximity 
        [HttpGet]
        [Route("locationbydistanceandcategory")]
        public async Task<IActionResult> GetLocationByCategory([FromQuery] decimal lat, decimal lng, double distance,int category, [FromQuery] PaginationFilter paginationFilter)
        {
            try
            {
                var locationsObjList = locationRepo.LocationCategoryByDistance(lat, lng, distance, paginationFilter,category);

                int totalRecords = locationRepo.LocationCount(lat, lng, distance, paginationFilter, "SEARCHGEOCATEGORY",category);
                string searchParams = $"?distance={distance}&category={category}&lat={lat}&lng={lng}";
                var route = Request.Path.Value + searchParams;

                var locationsDTOList = new List<ViewLocationDTO>();


                foreach (var location in locationsObjList)
                {
                    var locationObjDTO = _mapper.Map<ViewLocationDTO>(location);

                    var locationSubCategories = new List<SubCategoryDTO>();

                    locationObjDTO.Subcategories = location.LocalSubcategories.Select(s => new SubCategoryDTO
                    { Id = s.Subcategories.Id, Designation = s.Subcategories.Designation, Lng = s.Subcategories.Lng }).ToList();
                    locationObjDTO.CategoryId = (int)location.CategoryId;
                    locationObjDTO.Lat = location.Lat;
                    locationObjDTO.Lng = location.Lng;
                    locationObjDTO.Category = location.Category.Designation;
                    locationObjDTO.Suburb = location.Suburb.Designation;
                    locationObjDTO.Distance = location.Distance;
                    locationObjDTO.City = location.City.Designation;
                    locationObjDTO.Municipality = location.Municipality.Designation;
                    locationObjDTO.Images = location.Images.Select(s => s.ImagePath).ToList();
                    locationsDTOList.Add(locationObjDTO);

                }


                var validFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);

                var pagedResponse = PaginationHelper.CreatePageResponse<ViewLocationDTO>(locationsDTOList, validFilter, totalRecords, uriService, route);

                return Ok(pagedResponse);
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to retrieve location error ->" + ex.Message);
                return BadRequest();
            }
        }


    }
}

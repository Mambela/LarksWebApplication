using AutoMapper;
using LarksWebApp.DTOs;
using LarksWebApp.DTOs.DTO_Addresses;
using LarksWebApp.DTOs.DTO_Category;
using LarksWebApp.DTOs.DTO_Location;
using LarksWebApp.DTOs.DTO_Subcategory;
using LarksWebApp.DTOs.PaginationDTO;
using LarksWebApp.Models;
using LarksWebApp.Repository.AddressRepository.Address_Repository;
using LarksWebApp.Repository.CategoryRepository;
using LarksWebApp.Repository.CountryRepository;
using LarksWebApp.Repository.HoodRepository;
using LarksWebApp.Repository.LocationRepository;
using LarksWebApp.Repository.StreetRepository;
using LarksWebApp.Services.FileManagement;
using LarksWebApp.Services.Loggers;
using LarksWebApp.Services.PaginationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Serilog;

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
        private readonly ISuburbRepository suburbRepository;
        private readonly ILoggerTrace logger;
        private readonly IMapper _mapper;
        private readonly IUriService uriService;
        private readonly IStreetRepository streetRepository;
        private readonly IAddressRepository addressRepository;
        private readonly IConfiguration _configuration;

        public LocationController(ILocationRepository repository, ILoggerTrace _logger, IMapper mapper, 
            ICountryRepository countryRepo,
            ICategoryRepository categoryRepo,
            IFileManagement fileManagement,
            IUriService _uriService, ISuburbRepository _suburbRepository, IStreetRepository _streetRepository,
            IAddressRepository _addressRepository, IConfiguration configuration)
        {

            _countryRepo = countryRepo;
            _categoryRepo = categoryRepo;
            _fileManagement = fileManagement;
            locationRepo = repository;
            logger = _logger;
            _mapper = mapper;
            uriService = _uriService;
            suburbRepository = _suburbRepository;
            streetRepository = _streetRepository;
            addressRepository = _addressRepository;
            _configuration = configuration;
        }



        ///Locations By Neighbourhood 
        [HttpGet("NeighbourhoodLocations")]
        public IActionResult GetNeighBourhoodLocations(int suburbId) {

            try
            {
                var filter = new PaginationFilter();
                var neighbourhoodLocationList = locationRepo.GetSuburbLocations(suburbId, filter);


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
        [HttpGet("categorylocations")]
        public IActionResult GetlocationinCategory([FromQuery] int categoryId,PaginationFilter filter)
        {
            try
            {
                var neighbourhoodLocationList = locationRepo.GetCategoryLocations(categoryId,filter);

                int totalRecords = locationRepo.CountLocationByAttribute("CATEGORY", categoryId);
                var route = Request.Path.Value; 

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

                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

                var pagedResponse = PaginationHelper.CreatePageResponse<LocationDTO>(neighbourhoodLocationListDTO, validFilter, totalRecords, uriService, route);
                return Ok(pagedResponse);
            }
            catch (Exception ex)
            {
                logger.LogError("Error while fetching the locations in a category " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while trying to fetch locations"); ;
            }
 
        }


        [HttpGet("locationbyId")]
        public IActionResult GetLocationById([FromQuery] int locationId)
        {
            try
            {
                var locationObj = locationRepo.GetLocation(locationId);


                if (locationObj == null)
                {
                    return Ok(locationObj);
                }

                //    District = (locationObj.Municipality.Province.Districts.Count > 0)? locationObj.Municipality.Province.Districts.First().Designation:"N/A",
                

                var locationDTO = new  ViewLocationDTO
                { 
                    Id = locationObj.Id,
                    City = locationObj.City.Designation,
                    Category = locationObj.Category.Designation,
                    CategoryId = locationObj.Category.Id,
                    Country = locationObj.Municipality.Province.Country.Designation,
                    District = (locationObj.Suburb.District != null)? locationObj.Suburb.District.Designation:"N/A",
                    Designation = locationObj.Designation,
                    Municipality = locationObj.Municipality.Designation,
                    Suburb = locationObj.Suburb.Designation,
                    Images = locationObj.Images.Select(s =>s.ImagePath).ToList(),
                    AdditionalInformation = locationObj.AdditionalInformation,
                    Lat = locationObj.Lat,
                    Lng= locationObj.Lng,
                    
                };

                var subCategoriesList = new List<SubCategoryDTO>();

                foreach (var subCategories in locationObj.LocalSubcategories)
                {
                    subCategoriesList.Add( new SubCategoryDTO 
                    {
                        Id = subCategories.SubcategoriesId,
                        CategoryId = (int)locationObj.CategoryId,
                        Designation = subCategories.Subcategories.Designation,
                        Lng = subCategories.Subcategories.Lng
                    });                   
                }

                locationDTO.Subcategories = subCategoriesList;

                return Ok(locationDTO);
            }
            catch (Exception ex)
            {

                return BadRequest();
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
                var filter = new PaginationFilter();
                var neighbourhoodLocationList = locationRepo.GetSuburbLocations(neighbourhoodId, filter);

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


        //[Authorize(Roles = "Admin,SuperAdmin")]
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

                var savedImages = new List<MediaResponse>();

                var LocationImageList = new List<LocationImageModel>();

                if(locationCollection.Files != null)
                { 
                    if (locationCollection.Files.Count > 0)
                    {
                        var _categoryObj = _categoryRepo.GetCategoryById((locationCollection.searchCategoryId_hidden != 0)? locationCollection.searchCategoryId_hidden:locationCollection.CategoryId);

                        savedImages = await _fileManagement.SaveMedia(locationCollection.Files,"Locations");
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
                        ImagePath = image.imageMessage,
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

        [HttpPost]
        [Route("newlocationwithaddress")]
        public async Task<IActionResult> CreateLocationWithSuggestions([FromForm] CreateLocation_w_address_DTO locationDTO)
        {
            try
            {
                int suburbId = (int)((locationDTO.SuburbId ==0 || locationDTO.SuburbId ==null) ? 0:locationDTO.SuburbId);
                int streetId = 0;
                int districtId = (int)((locationDTO.DistrictId == null) ? 0 : locationDTO.DistrictId);
                int cityId = locationDTO.CityId;
                int addressId = 0;

                //Check if Suburb Provided Exists if not then Create it 
                if (locationDTO.SuburbId == 0 || locationDTO.SuburbId == null)
                {
                    if (locationDTO.Suburb != null)
                    {
                        var suburb = suburbRepository.VerifySuburbExist(locationDTO.Suburb, locationDTO.MunicipalityId, cityId, districtId);

                        if (suburb == null)
                        {
                            if (districtId == 0 || districtId == null)
                            {
                                suburb = new SuburbModel
                                {
                                    CityId = cityId,
                                    MunicipalityId = locationDTO.MunicipalityId,
                                    Designation = locationDTO.Suburb, //Suburb designation is from the Client. 
                                };
                            }
                            else {

                                suburb = new SuburbModel
                                {
                                    CityId = cityId,
                                    DistrictId = districtId,
                                    MunicipalityId = locationDTO.MunicipalityId,
                                    Designation = locationDTO.Suburb,
                                };

                            }
                         

                           


                            if (await suburbRepository.CreateSuburb(suburb) == null)
                            {
                                throw new Exception("Failed to create suburb.");
                            }
                        }
                        suburbId = suburb.Id;
                    }
                    else
                    {
                        ModelState.AddModelError("error", "A suburb is required, either provide an Id or a suburb name");
                        return BadRequest(ModelState);
                    }
                }

                //Check if Street Exist if not then Create it 
                if ((locationDTO.StreetId == 0 || locationDTO.StreetId == null) && locationDTO.Street != null)
                {

                    if (locationDTO.Street != null)
                    {
                        var street = streetRepository.VerifyStreetExistance(locationDTO.Street);

                        if (street == null)
                        {
                            street = new StreetModel
                            {
                                Designation = locationDTO.Street
                            };

                            if (await streetRepository.CreateStreetModel(street) != null)
                            {
                                
                            }
                        }

                        streetId = street.Id;

                    }

                }
                else
                {

                    if (locationDTO.StreetId > 0 || locationDTO.StreetId != null)
                    {
                        /** 
                         Link Street to Suburb. 
                         Check if the street is created 
                         * **/
                        
                        streetId = (locationDTO.StreetId == null || locationDTO.StreetId == 0)? 0:(int)locationDTO.StreetId;

                        if (!streetRepository.VerifyStreetSuburbLink(streetId, suburbId))
                        {
                            //Suburb streets don't exist create it 

                            var streetSuburb = new SuburbStreets { StreetsID = streetId, SuburbId = suburbId };

                            if (!await streetRepository.CreateSuburbStreetsLink(streetSuburb))
                            {
                                throw new Exception("Failed to create a suburb and Street link");
                            }

                        }
                    }
                }


         
                                  

                //Create address first 
                if (locationDTO.IncludeAddress)
                {
                    if (locationDTO.BuildingTypeId == 0 || locationDTO.BuildingTypeId == null)
                    {
                        ModelState.AddModelError("error", "If you are creating a new address then provide a building type.");
                        return BadRequest(ModelState);  
                    }

                    var address = new AddressModel
                    {
                        StreetID = streetId,
                        CountryID = locationDTO.CountryId,
                        ProvinceID = locationDTO.ProvinceId,
                        SuburbId = suburbId,
                        AdditionalInfo = (locationDTO.AdditionalInfo == null) ? "" : locationDTO.AdditionalInfo,
                        StreetNumber = (locationDTO.StreetNumber == null) ? "" : locationDTO.StreetNumber,
                        BuildingTypeId = locationDTO.BuildingTypeId
                    };

                    if (await addressRepository.CreateAddress(address) !=null)
                    {
                        addressId = address.Id;
                    }
                }

                //Now we can create a location
                var location = new LocationModel 
                {
                    Designation = locationDTO.Designation,
                    CategoryId = locationDTO.CategoryId,
                    CityId = cityId,
                    AdditionalInformation = locationDTO.LocationDescription,
                    AddressId = addressId,
                    MunicipalityId = locationDTO.MunicipalityId,
                    SuburbId = suburbId,
                    Lat = locationDTO.Lat,
                    Lng = locationDTO.Lng
                };

                if (await locationRepo.CreateLocation(location) == null)
                {
                    ModelState.AddModelError("error", "Failed to create a new Location ");
                }

                var savedImages = new List<string>();

                var LocationImageList = new List<LocationImageModel>();

                if (locationDTO.Files != null)
                {
                    if (locationDTO.Files.Count > 0)
                    {
                        var _categoryObj = _categoryRepo.GetCategoryById((locationDTO.CategoryId != 0) ? locationDTO.CategoryId  : locationDTO.CategoryId);

                        savedImages = await _fileManagement.MultipleFileUploads(locationDTO.Files, "Locations", _categoryObj.Designation);
                    }
                }


                var LocationSubcategory = locationDTO.SubCategories.Select(id => new LocationModelSubcategoryModel { LocationId = location.Id, SubcategoriesId = id });

                if (!await locationRepo.Createlocationsubcategory(LocationSubcategory))
                {
                    return BadRequest();
                }




                foreach (var image in savedImages)
                {

                    var locatinImageModel = new LocationImageModel
                    {
                        ImagePath = image,
                        LocationId = location.Id

                    };

                    LocationImageList.Add(locatinImageModel);
                }


                if (await locationRepo.CreateLocationImage(LocationImageList) == null)
                {
                    return BadRequest();
                }



                var createdLocation = new Created_Location_DTO 
                {
                    Id=location.Id,
                    AddressId = addressId,
                    CategoryId = locationDTO.CategoryId,
                    CityId = locationDTO.CityId,
                    ProvinceId   = locationDTO.ProvinceId,
                    Designation = locationDTO.Designation,
                    MunicipalityId = locationDTO.MunicipalityId,
                    Lat = locationDTO.Lat,
                    Lng = locationDTO.Lng,
                    SuburbId = suburbId
                };

                return StatusCode(StatusCodes.Status201Created,createdLocation);
            }
            catch (Exception ex)
            {
                logger.LogError("Error creating location "+ex.Message);

                 ModelState.AddModelError("errors", "Failed to create a new location");

                return BadRequest(ModelState);
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

        [HttpGet]
        [Route("locationsinsuburb")]
        public async Task<IActionResult> GetLocationInSuburb([FromQuery] decimal lat, decimal lng, double distance, int category, [FromQuery] PaginationFilter paginationFilter)
        {
            try
            {
                var locationsObjList = locationRepo.LocationCategoryByDistance(lat, lng, distance, paginationFilter, category);

                int totalRecords = locationRepo.LocationCount(lat, lng, distance, paginationFilter, "SEARCHGEOCATEGORY", category);
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


        [HttpGet]
        [Route("locationsincity")]
        public async Task<IActionResult> GetLocationInCity([FromQuery] PaginationFilter filter, int cityId)
        {
            
                try
                {
                     var neighbourhoodLocationList = locationRepo.GetLocationsInCity(cityId, filter);
                    int totalRecords = locationRepo.CountLocationByAttribute("CITY", cityId);

                    string searchParams = $"?cityId={cityId}";
                    var route = Request.Path.Value + searchParams;
                

                    //var neighbourhoodLocationListDTO = _mapper.Map<List<LocationDTO>>(neighbourhoodLocationList);

                    var locationsDTOList = new List<ViewLocationDTO>();


                    foreach (var location in neighbourhoodLocationList)
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
                        locationObjDTO.Distance = 0;
                        locationObjDTO.City = location.City.Designation;
                        locationObjDTO.Municipality = location.Municipality.Designation;
                        locationObjDTO.Images = location.Images.Select(s => s.ImagePath).ToList();
                        locationsDTOList.Add(locationObjDTO);

                    }

                    var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

                        var pagedResponse = PaginationHelper.CreatePageResponse<ViewLocationDTO>(locationsDTOList, validFilter, totalRecords, uriService, route);

                    return Ok(pagedResponse);
                }
                catch (Exception ex)
                {
                    logger.LogError("Error while fetching the locations in a category " + ex.Message);

                    return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while trying to fetch locations"); ;
                }
            
        }


        [HttpGet]
        [Route("locationsinmunicipality")]
        public async Task<IActionResult> GetLocationInmunicipality([FromQuery] int municipalityId,[FromQuery] PaginationFilter filter)
        {
            try
            {
                var locationsInMunicipalityList = locationRepo.GetLocationsInMunicipality(municipalityId,filter);
                int totalRecords = locationRepo.CountLocationByAttribute("MUNICIPALITY", municipalityId);
            
                            

                string searchParams = $"?municipalityId={municipalityId}";
                var route = Request.Path.Value + searchParams;


                //var neighbourhoodLocationListDTO = _mapper.Map<List<LocationDTO>>(neighbourhoodLocationList);

                var locationsDTOList = new List<ViewLocationDTO>();


                foreach (var location in locationsInMunicipalityList)
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
                    locationObjDTO.Distance = 0;
                    locationObjDTO.City = location.City.Designation;
                    locationObjDTO.Municipality = location.Municipality.Designation;
                    locationObjDTO.Images = location.Images.Select(s => s.ImagePath).ToList();
                    locationsDTOList.Add(locationObjDTO);

                }

                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

                var pagedResponse = PaginationHelper.CreatePageResponse<ViewLocationDTO>(locationsDTOList, validFilter, totalRecords, uriService, route);

                return Ok(pagedResponse);
            }
            catch (Exception ex)
            {
                logger.LogError("Error while fetching the locations in a category " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while trying to fetch locations"); ;
            }
        }



        [HttpGet]
        [Route("searchlocation")]
        public async Task<IActionResult> GetLocationByName([FromQuery] string searchParameter, [FromQuery] PaginationFilter filter)
        {
            try
            {

                if (searchParameter.Length < 3)
                {
                    ModelState.AddModelError("error","Search parameter must be more than 2 characters");
                    return BadRequest(ModelState);
                }

                var locationsInMunicipalityList = locationRepo.GetLocationByLocationName(searchParameter, filter);
                int totalRecords = locationRepo.CountLocationByAttribute("LOCATION_NAME", 0, searchParameter);



                string searchParams = $"?searchParameter={searchParameter}";
                var route = Request.Path.Value + searchParams;


                //var neighbourhoodLocationListDTO = _mapper.Map<List<LocationDTO>>(neighbourhoodLocationList);

                var locationsDTOList = new List<ViewLocationDTO>();


                foreach (var location in locationsInMunicipalityList)
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
                    locationObjDTO.Distance = 0;
                    locationObjDTO.City = location.City.Designation;
                    locationObjDTO.Municipality = location.Municipality.Designation;
                    locationObjDTO.Images = location.Images.Select(s => s.ImagePath).ToList();
                    locationsDTOList.Add(locationObjDTO);

                }

                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

                var pagedResponse = PaginationHelper.CreatePageResponse<ViewLocationDTO>(locationsDTOList, validFilter, totalRecords, uriService, route);

                return Ok(pagedResponse);
            }
            catch (Exception ex)
            {
                logger.LogError("Error while fetching the locations in a category " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while trying to fetch locations"); ;
            }
        }


        [HttpGet] 
        [Route("searchlocationAll")]
        public async Task<IActionResult> SearchlocationAll([FromQuery] string countryName,[FromQuery] int pageSize,[FromQuery] int skip, [FromQuery] string ? searchParameter, [FromQuery] int ? categoryId, [FromQuery] int ? suburbId, [FromQuery] int ? cityId, [FromQuery] int ? municipalityId)
        {
            try
            {
                var locationList = locationRepo.GetLocationUsingStoreProc(countryName,searchParameter, categoryId,suburbId, cityId, municipalityId,pageSize,skip);

                var locationsDTOList = new List<ViewLocationDTO>();


                foreach (var location in locationList)
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
                    locationObjDTO.Distance = 0;
                    locationObjDTO.City = location.City.Designation;
                    locationObjDTO.Municipality = location.Municipality.Designation;
                    locationObjDTO.Images = location.Images.Select(s => s.ImagePath).ToList();
                    locationsDTOList.Add(locationObjDTO);

                }

                return Ok(locationsDTOList);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("locationsInList")]
        public async Task<IActionResult> retrievalLocation([FromQuery] IList<int> locationListIds, [FromQuery] PaginationFilter filter)
        {
            try
            {
                var arrayEmptyImage = new List<string>();

                var locationList = locationRepo.GetLocationList(locationListIds, filter);

                var locationsDTOList = new List<ViewLocationDTO>();

                foreach (var location in locationList)
                {
                    var locationObjDTO = _mapper.Map<ViewLocationDTO>(location);

                    var locationSubCategories = new List<SubCategoryDTO>();

                    locationObjDTO.Subcategories = location.LocalSubcategories.Select(s => new SubCategoryDTO
                    { Id = s.Subcategories.Id, Designation = s.Subcategories.Designation, Lng = s.Subcategories.Lng }).ToList();
                    locationObjDTO.CategoryId = (int)location.CategoryId;
                    locationObjDTO.Lat = location.Lat;
                    locationObjDTO.Lng = location.Lng;
                    locationObjDTO.Country = location.City.Province.Country.Designation;
                    locationObjDTO.Category = location.Category.Designation;
                    locationObjDTO.Suburb = location.Suburb.Designation;
                    locationObjDTO.Distance = 0;
                    locationObjDTO.City = location.City.Designation;
                    locationObjDTO.Municipality = location.Municipality.Designation;
                    locationObjDTO.Images = (location.Images.Count() > 0) ? location.Images.Select(s => s.ImagePath).ToList() : null;
                    locationsDTOList.Add(locationObjDTO);

                }

                return Ok(locationsDTOList);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        
        [HttpPatch]
        [Route("updateLocation")]
        public async Task<IActionResult> updateLocationAsync([FromForm] UpdateLocationDTO locationDTO) {
            try
            {
                var locationObj = locationRepo.GetLocation(locationDTO.Id);



                int suburbId = (int)((locationDTO.SuburbId == 0 || locationDTO.SuburbId == null) ? 0 : locationDTO.SuburbId);
                int streetId = 0;
                int districtId = (int)((locationDTO.DistrictId == null) ? 0 : locationDTO.DistrictId);
                int cityId = locationDTO.CityId;
                int addressId = 0;

                //Check if Suburb Provided Exists if not then Create it 
                if (locationDTO.SuburbId == 0 || locationDTO.SuburbId == null)
                {
                    if (locationDTO.Suburb != null)
                    {
                        var suburb = suburbRepository.VerifySuburbExist(locationDTO.Suburb, locationDTO.MunicipalityId, cityId, districtId);

                        if (suburb == null)
                        {
                            if (districtId == 0 || districtId == null)
                            {
                                suburb = new SuburbModel
                                {
                                    CityId = cityId,
                                    MunicipalityId = locationDTO.MunicipalityId,
                                    Designation = locationDTO.Suburb, //Suburb designation is from the Client. 
                                };
                            }
                            else
                            {

                                suburb = new SuburbModel
                                {
                                    CityId = cityId,
                                    DistrictId = districtId,
                                    MunicipalityId = locationDTO.MunicipalityId,
                                    Designation = locationDTO.Suburb,
                                };

                            }





                            if (await suburbRepository.CreateSuburb(suburb) == null)
                            {
                                throw new Exception("Failed to create suburb.");
                            }
                        }
                        suburbId = suburb.Id;
                    }
                    else
                    {
                        ModelState.AddModelError("error", "A suburb is required, either provide an Id or a suburb name");
                        return BadRequest(ModelState);
                    }
                }

                //Check if Street Exist if not then Create it 
                if ((locationDTO.StreetId == 0 || locationDTO.StreetId == null) && locationDTO.Street != null)
                {

                    if (locationDTO.Street != null)
                    {
                        var street = streetRepository.VerifyStreetExistance(locationDTO.Street);

                        if (street == null)
                        {
                            street = new StreetModel
                            {
                                Designation = locationDTO.Street
                            };

                            if (await streetRepository.CreateStreetModel(street) != null)
                            {

                            }
                        }

                        streetId = street.Id;

                    }

                }
                else
                {

                    if (locationDTO.StreetId > 0 || locationDTO.StreetId != null)
                    {
                        /** 
                         Link Street to Suburb. 
                         Check if the street is created 
                         * **/

                        streetId = (locationDTO.StreetId == null || locationDTO.StreetId == 0) ? 0 : (int)locationDTO.StreetId;

                        if (!streetRepository.VerifyStreetSuburbLink(streetId, suburbId))
                        {
                            //Suburb streets don't exist create it 

                            var streetSuburb = new SuburbStreets { StreetsID = streetId, SuburbId = suburbId };

                            if (!await streetRepository.CreateSuburbStreetsLink(streetSuburb))
                            {
                                throw new Exception("Failed to create a suburb and Street link");
                            }

                        }
                    }
                }





                //Create address first 
                if (locationDTO.IncludeAddress)
                {
                    if (locationDTO.BuildingTypeId == 0 || locationDTO.BuildingTypeId == null)
                    {
                        ModelState.AddModelError("error", "If you are creating a new address then provide a building type.");
                        return BadRequest(ModelState);
                    }

                    var address = new AddressModel
                    {
                        StreetID = streetId,
                        CountryID = locationDTO.CountryId,
                        ProvinceID = locationDTO.ProvinceId,
                        SuburbId = suburbId,
                        AdditionalInfo = (locationDTO.AdditionalInfo == null) ? "" : locationDTO.AdditionalInfo,
                        StreetNumber = (locationDTO.StreetNumber == null) ? "" : locationDTO.StreetNumber,
                        BuildingTypeId = locationDTO.BuildingTypeId
                    };

                    if (await addressRepository.CreateAddress(address) != null)
                    {
                        addressId = address.Id;
                    }
                }

                //Now we can create a location
              

                locationObj.Designation = locationDTO.Designation;
                locationObj.CategoryId = locationDTO.CategoryId;
                locationObj.CityId = locationDTO.CityId;
                locationObj.AdditionalInformation= locationDTO.AdditionalInfo;
                locationObj.AddressId = addressId;
                locationObj.MunicipalityId = locationDTO.MunicipalityId;
                locationObj.SuburbId = suburbId;
                locationObj.Lat = locationDTO.Lat;
                locationObj.Lng = locationDTO.Lng;
                

                if (!locationRepo.UpdateLocation(locationObj))
                {
                    ModelState.AddModelError("error", "Failed to create a new Location ");
                }


                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError("Error updating Location "+ex.Message);
                return BadRequest();
            }
        }

       
    }
}

using AutoMapper;
using LarksWebApp.DAL;
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
using LarksWebApp.Repository.SuburbRepository;
using LarksWebApp.Repository.LocationRepository;
using LarksWebApp.Repository.StreetRepository;
using LarksWebApp.Repository.SubcategoryRepository;
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
        private readonly LarksContext _dbContext;
        private readonly ISubcategoryRepository _subcategoryRepo;

        public LocationController(ILocationRepository repository, ILoggerTrace _logger, IMapper mapper,
            ICountryRepository countryRepo,
            ICategoryRepository categoryRepo,
            IFileManagement fileManagement,
            IUriService _uriService, ISuburbRepository _suburbRepository, IStreetRepository _streetRepository,
            IAddressRepository _addressRepository, IConfiguration configuration, LarksContext dbContext,
            ISubcategoryRepository subcategoryRepo)
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
            _dbContext = dbContext;
            _subcategoryRepo = subcategoryRepo;
        }

        private bool SubcategoriesAreValidForCategory(Guid categoryId, IEnumerable<Guid> subcategoryIds)
        {
            if (subcategoryIds == null || !subcategoryIds.Any())
            {
                return true;
            }

            var validIds = _subcategoryRepo.GetSubcategoriesInCategory(categoryId).Select(s => s.Id).ToHashSet();

            return subcategoryIds.All(id => validIds.Contains(id));
        }



        ///Locations By Suburb 
        [HttpGet("SuburbLocations")]
        public IActionResult GetSuburbLocations(Guid suburbId) {

            try
            {
                var filter = new PaginationFilter();
                var suburbLocationList = locationRepo.GetSuburbLocations(suburbId, filter);


                var suburbLocationListDTO = new List<LocationDTO>();


                foreach (var location in suburbLocationList)
                {
                    var images = new List<string>();


                    foreach (var image in location.Images)
                        images.Add(image.ImagePath);


                    var addressDTO = new ViewAddressDTO
                    {
                        Id = (Guid)location.AddressId,
                        Municipality = location.Municipality.Designation,
                        Suburb = location.Suburb.Designation,
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
                        CategoryId = (Guid)location.CategoryId,
                        //StreetName = location.Address.Street.Designation,
                        Category = location.Category.Designation,
                        //SubcategoryId = location.SubcategoryId,
                        //Subcategory = location.Subcategory.Designation,
                        Images = images
                    };


                    suburbLocationListDTO.Add(locationObj);
                }


                return Ok(suburbLocationListDTO);

            }
            catch (Exception ex)
            {
                logger.LogError("Error while fetching the locations in a Suburb " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while trying to fetch locations");
                
            }
        }

 
        ///Location by Category 
        [HttpGet("categorylocations")]
        public IActionResult GetlocationinCategory([FromQuery] Guid categoryId,PaginationFilter filter)
        {
            try
            {
                var suburbLocationList = locationRepo.GetCategoryLocations(categoryId,filter);

                int totalRecords = locationRepo.CountLocationByAttribute("CATEGORY", categoryId);
                var route = Request.Path.Value; 

                var suburbLocationListDTO = new List<LocationDTO>();

                foreach (var location in suburbLocationList)
                {
                    suburbLocationListDTO.Add(
                        new LocationDTO
                        {
                            AdditionalInformation = location.AdditionalInformation,
                            Category = location.Category.Designation,
                            Designation = location.Designation,
                            CategoryId = (Guid)location.CategoryId,
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

                var pagedResponse = PaginationHelper.CreatePageResponse<LocationDTO>(suburbLocationListDTO, validFilter, totalRecords, uriService, route);
                return Ok(pagedResponse);
            }
            catch (Exception ex)
            {
                logger.LogError("Error while fetching the locations in a category " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while trying to fetch locations"); ;
            }
 
        }


        [HttpGet("locationbyId")]
        public IActionResult GetLocationById([FromQuery] Guid locationId)
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
                        CategoryId = (Guid)locationObj.CategoryId,
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
        public IActionResult GetlocationInSubCategory(Guid subcategoryId) {

            try
            {
                var suburbLocationList = locationRepo.GetSubcategoryLocations(subcategoryId);



                var suburbLocationListDTO = _mapper.Map<List<LocationDTO>>(suburbLocationList);


                return Ok(suburbLocationListDTO);
            }
            catch (Exception ex)
            {
                logger.LogError("Error while fetching the locations in a category " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while trying to fetch locations"); ;
            }
        }


        ///Data Table Locations In Suburb
        ///

        [Route("[action]")]
        [HttpPost]
        public IActionResult LocationsInSuburbDT(Guid suburbId)
        {
            try
            {
                var filter = new PaginationFilter();
                var suburbLocationList = locationRepo.GetSuburbLocations(suburbId, filter);

                var suburbLocationListDTO = new List<LocationDTO>();

                foreach (var location in suburbLocationList)
                {
                   
                    var locationObj = new LocationDTO
                    {
                        Designation = location.Designation,
                        AdditionalInformation = location.AdditionalInformation,
                        //AddressId = location.AddressId,
                        CategoryId = (Guid)location.CategoryId,
                        Category = location.Category.Designation,
                        //SubcategoryId = location.SubcategoryId,
                        Subcategory = location.Category.Subcategories.Select(s => new SubCategoryDTO { Designation = s.Designation, Id = s.Id })                        
                    };

                    suburbLocationListDTO.Add(locationObj);
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
                    suburbLocationListDTO = (List<LocationDTO>)suburbLocationListDTO.OrderBy(c => c.Designation);
                }


                if (!string.IsNullOrEmpty(searchValue))
                {
                    suburbLocationListDTO = (List<LocationDTO>)suburbLocationListDTO.Where(m => m.Designation.Contains(searchValue)
                                                || m.Designation.Contains(searchValue));

                }

                recordsTotal = suburbLocationListDTO.Count();
                var data = suburbLocationListDTO.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                logger.LogError("Error in location in suburb " + ex.Message);

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
                        var _categoryObj = _categoryRepo.GetCategoryById((locationCollection.searchCategoryId_hidden != Guid.Empty)? locationCollection.searchCategoryId_hidden:locationCollection.CategoryId);

                        savedImages = await _fileManagement.SaveMedia(locationCollection.Files,"Locations");
                    }
                }


                // Save the Location first then use the object.

                var resolvedCategoryId = (locationCollection.searchCategoryId_hidden != Guid.Empty) ? locationCollection.searchCategoryId_hidden : locationCollection.CategoryId;

                if (!SubcategoriesAreValidForCategory(resolvedCategoryId, locationCollection.SubCategories))
                {
                    return BadRequest();
                }

                var locationObj = new LocationModel
                {
                    AddressId = (locationCollection.searchAddress_hidden != Guid.Empty)? locationCollection.searchAddress_hidden :Guid.Empty,
                    CategoryId = resolvedCategoryId,
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
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                Guid suburbId = locationDTO.SuburbId.GetValueOrDefault();
                Guid streetId = Guid.Empty;
                Guid districtId = locationDTO.DistrictId.GetValueOrDefault();
                Guid cityId = locationDTO.CityId;
                Guid addressId = Guid.Empty;

                //Check if Suburb Provided Exists if not then Create it
                if (suburbId == Guid.Empty)
                {
                    if (locationDTO.Suburb != null)
                    {
                        var suburb = suburbRepository.VerifySuburbExist(locationDTO.Suburb, locationDTO.MunicipalityId, cityId, districtId);

                        if (suburb == null)
                        {
                            suburb = new SuburbModel
                            {
                                CityId = cityId,
                                DistrictId = (districtId == Guid.Empty) ? null : districtId,
                                MunicipalityId = locationDTO.MunicipalityId,
                                Designation = locationDTO.Suburb, //Suburb designation is from the Client.
                            };

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

                //Check if Street Exists, if not then Create it; otherwise link the existing Street to the Suburb
                if (locationDTO.StreetId == null && locationDTO.Street != null)
                {
                    var street = streetRepository.VerifyStreetExistance(locationDTO.Street);

                    if (street == null)
                    {
                        street = new StreetModel
                        {
                            Designation = locationDTO.Street
                        };

                        if (await streetRepository.CreateStreetModel(street) == null)
                        {
                            throw new Exception("Failed to create street.");
                        }
                    }

                    streetId = street.Id;
                }
                else if (locationDTO.StreetId != null)
                {
                    streetId = locationDTO.StreetId.Value;
                }
                else
                {
                    ModelState.AddModelError("error", "A street is required, either provide an Id or a street name");
                    return BadRequest(ModelState);
                }

                if (!streetRepository.VerifyStreetSuburbLink(streetId, suburbId))
                {
                    //Suburb streets don't exist create it

                    var streetSuburb = new SuburbStreets { StreetsID = streetId, SuburbId = suburbId };

                    if (!await streetRepository.CreateSuburbStreetsLink(streetSuburb))
                    {
                        throw new Exception("Failed to create a suburb and Street link");
                    }
                }

                //Create address first
                if (locationDTO.IncludeAddress)
                {
                    if (locationDTO.BuildingTypeId == Guid.Empty || locationDTO.BuildingTypeId == null)
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
                        AdditionalInfo = locationDTO.AdditionalInfo ?? "",
                        StreetNumber = locationDTO.StreetNumber ?? "",
                        BuildingTypeId = locationDTO.BuildingTypeId
                    };

                    if (await addressRepository.CreateAddress(address) == null)
                    {
                        throw new Exception("Failed to create address.");
                    }

                    addressId = address.Id;
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
                    return BadRequest(ModelState);
                }

                var savedImages = new List<string>();

                var LocationImageList = new List<LocationImageModel>();

                if (locationDTO.Files != null && locationDTO.Files.Count > 0)
                {
                    var _categoryObj = _categoryRepo.GetCategoryById(locationDTO.CategoryId);

                    if (_categoryObj == null)
                    {
                        ModelState.AddModelError("error", "The provided category does not exist.");
                        return BadRequest(ModelState);
                    }

                    savedImages = await _fileManagement.MultipleFileUploads(locationDTO.Files, "Locations", _categoryObj.Designation);
                }


                if (locationDTO.SubCategories != null && locationDTO.SubCategories.Count > 0)
                {
                    if (!SubcategoriesAreValidForCategory(locationDTO.CategoryId, locationDTO.SubCategories))
                    {
                        ModelState.AddModelError("error", "One or more subcategories do not belong to the selected category.");
                        return BadRequest(ModelState);
                    }

                    var LocationSubcategory = locationDTO.SubCategories.Select(id => new LocationModelSubcategoryModel { LocationId = location.Id, SubcategoriesId = id });

                    if (!await locationRepo.Createlocationsubcategory(LocationSubcategory))
                    {
                        return BadRequest();
                    }
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


                if (LocationImageList.Count > 0 && await locationRepo.CreateLocationImage(LocationImageList) == null)
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

                await transaction.CommitAsync();

                return StatusCode(StatusCodes.Status201Created,createdLocation);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

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
                    locationObjDTO.CategoryId = (Guid)location.CategoryId;
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
        public async Task<IActionResult> GetLocationByCategory([FromQuery] decimal lat, decimal lng, double distance,Guid category, [FromQuery] PaginationFilter paginationFilter)
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
                    locationObjDTO.CategoryId = (Guid)location.CategoryId;
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
        public async Task<IActionResult> GetLocationInSuburb([FromQuery] decimal lat, decimal lng, double distance, Guid category, [FromQuery] PaginationFilter paginationFilter)
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
                    locationObjDTO.CategoryId = (Guid)location.CategoryId;
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
        public async Task<IActionResult> GetLocationInCity([FromQuery] PaginationFilter filter, Guid cityId)
        {
            
                try
                {
                     var suburbLocationList = locationRepo.GetLocationsInCity(cityId, filter);
                    int totalRecords = locationRepo.CountLocationByAttribute("CITY", cityId);

                    string searchParams = $"?cityId={cityId}";
                    var route = Request.Path.Value + searchParams;
                

                    //var suburbLocationListDTO = _mapper.Map<List<LocationDTO>>(suburbLocationList);

                    var locationsDTOList = new List<ViewLocationDTO>();


                    foreach (var location in suburbLocationList)
                    {
                        var locationObjDTO = _mapper.Map<ViewLocationDTO>(location);

                        var locationSubCategories = new List<SubCategoryDTO>();

                        locationObjDTO.Subcategories = location.LocalSubcategories.Select(s => new SubCategoryDTO
                        { Id = s.Subcategories.Id, Designation = s.Subcategories.Designation, Lng = s.Subcategories.Lng }).ToList();
                        locationObjDTO.CategoryId = (Guid)location.CategoryId;
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
        public async Task<IActionResult> GetLocationInmunicipality([FromQuery] Guid municipalityId,[FromQuery] PaginationFilter filter)
        {
            try
            {
                var locationsInMunicipalityList = locationRepo.GetLocationsInMunicipality(municipalityId,filter);
                int totalRecords = locationRepo.CountLocationByAttribute("MUNICIPALITY", municipalityId);
            
                            

                string searchParams = $"?municipalityId={municipalityId}";
                var route = Request.Path.Value + searchParams;


                //var suburbLocationListDTO = _mapper.Map<List<LocationDTO>>(suburbLocationList);

                var locationsDTOList = new List<ViewLocationDTO>();


                foreach (var location in locationsInMunicipalityList)
                {
                    var locationObjDTO = _mapper.Map<ViewLocationDTO>(location);

                    var locationSubCategories = new List<SubCategoryDTO>();

                    locationObjDTO.Subcategories = location.LocalSubcategories.Select(s => new SubCategoryDTO
                    { Id = s.Subcategories.Id, Designation = s.Subcategories.Designation, Lng = s.Subcategories.Lng }).ToList();
                    locationObjDTO.CategoryId = (Guid)location.CategoryId;
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
                int totalRecords = locationRepo.CountLocationByAttribute("LOCATION_NAME", Guid.Empty, searchParameter);



                string searchParams = $"?searchParameter={searchParameter}";
                var route = Request.Path.Value + searchParams;


                //var suburbLocationListDTO = _mapper.Map<List<LocationDTO>>(suburbLocationList);

                var locationsDTOList = new List<ViewLocationDTO>();


                foreach (var location in locationsInMunicipalityList)
                {
                    var locationObjDTO = _mapper.Map<ViewLocationDTO>(location);

                    var locationSubCategories = new List<SubCategoryDTO>();

                    locationObjDTO.Subcategories = location.LocalSubcategories.Select(s => new SubCategoryDTO
                    { Id = s.Subcategories.Id, Designation = s.Subcategories.Designation, Lng = s.Subcategories.Lng }).ToList();
                    locationObjDTO.CategoryId = (Guid)location.CategoryId;
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
        public async Task<IActionResult> SearchlocationAll([FromQuery] string countryName,[FromQuery] int pageSize,[FromQuery] int skip, [FromQuery] string ? searchParameter, [FromQuery] Guid ? categoryId, [FromQuery] Guid ? suburbId, [FromQuery] Guid ? cityId, [FromQuery] Guid ? municipalityId)
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
                    locationObjDTO.CategoryId = (Guid)location.CategoryId;
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
        public async Task<IActionResult> retrievalLocation([FromQuery] IList<Guid> locationListIds, [FromQuery] PaginationFilter filter)
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
                    locationObjDTO.CategoryId = (Guid)location.CategoryId;
                    locationObjDTO.Lat = location.Lat;
                    locationObjDTO.Lng = location.Lng;
                    locationObjDTO.Country = (location.City.Province.Country.Designation == null)? "":location.City.Province.Country.Designation;
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



                Guid suburbId = (locationDTO.SuburbId == Guid.Empty || locationDTO.SuburbId == null) ? Guid.Empty : (Guid)locationDTO.SuburbId;
                Guid streetId = Guid.Empty;
                Guid districtId = (locationDTO.DistrictId == null) ? Guid.Empty : (Guid)locationDTO.DistrictId;
                Guid cityId = locationDTO.CityId;
                Guid addressId = Guid.Empty;

                //Check if Suburb Provided Exists if not then Create it 
                if (locationDTO.SuburbId == Guid.Empty || locationDTO.SuburbId == null)
                {
                    if (locationDTO.Suburb != null)
                    {
                        var suburb = suburbRepository.VerifySuburbExist(locationDTO.Suburb, locationDTO.MunicipalityId, cityId, districtId);

                        if (suburb == null)
                        {
                            if (districtId == Guid.Empty || districtId == null)
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
                if ((locationDTO.StreetId == Guid.Empty || locationDTO.StreetId == null) && locationDTO.Street != null)
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

                    if (locationDTO.StreetId != Guid.Empty || locationDTO.StreetId != null)
                    {
                        /** 
                         Link Street to Suburb. 
                         Check if the street is created 
                         * **/

                        streetId = (locationDTO.StreetId == null || locationDTO.StreetId == Guid.Empty) ? Guid.Empty : (Guid)locationDTO.StreetId;

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
                    if (locationDTO.BuildingTypeId == Guid.Empty || locationDTO.BuildingTypeId == null)
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
                        AdditionalInfo = locationDTO.AdditionalInfo ?? "",
                        StreetNumber = locationDTO.StreetNumber ?? "",
                        BuildingTypeId = locationDTO.BuildingTypeId
                    };

                    if (await addressRepository.CreateAddress(address) != null)
                    {
                        addressId = address.Id;
                    }
                }

                //Now we can create a location
              

                if (locationDTO.SubCategories != null && locationDTO.SubCategories.Count > 0)
                {
                    if (!SubcategoriesAreValidForCategory(locationDTO.CategoryId, locationDTO.SubCategories))
                    {
                        ModelState.AddModelError("error", "One or more subcategories do not belong to the selected category.");
                        return BadRequest(ModelState);
                    }

                    if (!await locationRepo.RemoveLocationSubcategories(locationObj.Id))
                    {
                        ModelState.AddModelError("error", "Failed to update location subcategories.");
                        return BadRequest(ModelState);
                    }

                    var updatedSubcategories = locationDTO.SubCategories.Select(id => new LocationModelSubcategoryModel { LocationId = locationObj.Id, SubcategoriesId = id });

                    if (!await locationRepo.Createlocationsubcategory(updatedSubcategories))
                    {
                        ModelState.AddModelError("error", "Failed to update location subcategories.");
                        return BadRequest(ModelState);
                    }
                }

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

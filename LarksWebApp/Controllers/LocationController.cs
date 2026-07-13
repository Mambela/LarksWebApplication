using AutoMapper;
using LarksWebApp.DTOs;
using LarksWebApp.DTOs.DTO_Addresses;
using LarksWebApp.DTOs.DTO_Category;
using LarksWebApp.DTOs.DTO_Location;
using LarksWebApp.Models;
using LarksWebApp.Repository.CategoryRepository;
using LarksWebApp.Repository.CountryRepository;
using LarksWebApp.Repository.LocationRepository;
using LarksWebApp.Services.FileManagement;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        private readonly ICountryRepository _countryRepo;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IFileManagement _fileManagement;
        private readonly ILocationRepository locationRepo;
        private readonly ILoggerTrace logger;
        

        public LocationController(ICountryRepository countryRepo, IMapper mapper, ILoggerTrace serviceLogger,
            ICategoryRepository categoryRepository, IFileManagement filemanagement, ILocationRepository repo,ILoggerTrace _logger)
        {
            _categoryRepo = categoryRepository;
            _countryRepo = countryRepo;
            _mapper = mapper;
            this.serviceLogger = serviceLogger;
            _fileManagement = filemanagement;
            locationRepo = repo;
            logger = _logger;
        }


        // GET: LocationController
        public ActionResult Index()
        {
            try
            {

                var listofCountry = _countryRepo.GetAll();

                var listofCountryDTO = new List<CountryDTO>();

                var listOfLocationsCategory = _mapper.Map<List<CategoryDTO>>(_categoryRepo.GetAllCategories());

                listofCountryDTO = _mapper.Map<List<CountryDTO>>(listofCountry);

                var createAdressDTO = new Create_Location_DTO();

                createAdressDTO.ListofCountryDTO = listofCountryDTO;
                createAdressDTO.ListOfCategories = listOfLocationsCategory;

                return View(createAdressDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Address controller getting the countries " + ex.Message);

                return BadRequest();
            }
        }

        // GET: LocationController/Details/5
        public ActionResult Details(Guid id)
        {
            return View();
        }

        // GET: LocationController/Create
        public ActionResult Create(string lng)
        {
            try
            {
                var listofCountry = _countryRepo.GetAll();

                var listofCountryDTO = new List<CountryDTO>();

                var listOfLocationsCategory = _mapper.Map<List<CategoryDTO>>(_categoryRepo.GetAllCategories());

                listofCountryDTO = _mapper.Map<List<CountryDTO>>(listofCountry);

                var createAdressDTO = new Create_Location_DTO();

                createAdressDTO.ListofCountryDTO = listofCountryDTO;
                createAdressDTO.ListOfCategories = listOfLocationsCategory;


                return View(createAdressDTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Address Controller getting the countries" + ex.Message);

                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromForm] Create_Location_DTO locationCollection, IList<IFormFile> Files)
        {

            try
            {

                if (!ModelState.IsValid)
                {
                    var listofCountry = _countryRepo.GetAll();

                    locationCollection.ListofCountryDTO = _mapper.Map<List<CountryDTO>>(listofCountry);

                    locationCollection.ListOfCategories = _mapper.Map<List<CategoryDTO>>(_categoryRepo.GetAllCategories());

                    return View(locationCollection);
                }



                var savedImages = new List<string>();

                var LocationImageList = new List<LocationImageModel>();

                if (locationCollection.Files.Count > 0)
                {
                    var _categoryObj = _categoryRepo.GetCategoryById(locationCollection.searchCategoryId_hidden);

                    savedImages = await _fileManagement.MultipleFileUploads(locationCollection.Files, "Locations", _categoryObj.Designation);
                }


                // Save the Location first then use the object.

                var locationObj = new LocationModel
                {
                    AddressId = locationCollection.searchAddress_hidden,
                    CategoryId = locationCollection.searchCategoryId_hidden,
                    MunicipalityId = locationCollection.MunicipalityID,
                    CityId = locationCollection.CityId,
                    SuburbId = locationCollection.SuburbId,
                    Designation = locationCollection.Designation,
                    AdditionalInformation = locationCollection.AdditionalInfo
                };


                if (await locationRepo.CreateLocation(locationObj) == null)
                {
                    return RedirectToAction(nameof(Index));
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
                    return RedirectToAction(nameof(Index));
                }


                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                logger.LogError("Error thrown when creating location "+ ex.Message);
                return View();
            }
        
        }

        // GET: LocationController/Edit/5
        public ActionResult Edit(Guid id)
        {
            return View();
        }

        // POST: LocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
            
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LocationController/Delete/5
        public ActionResult Delete(Guid id)
        {
            return View();
        }

        // POST: LocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

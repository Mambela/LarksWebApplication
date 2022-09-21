using AutoMapper;
using LarksWebApp.DTOs;
using LarksWebApp.DTOs.DTO_Category;
using LarksWebApp.DTOs.DTO_Location;
using LarksWebApp.Repository.CategoryRepository;
using LarksWebApp.Repository.CountryRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers
{
    public class LocationController : Controller
    {
        private readonly ICountryRepository _countryRepo;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;
        private readonly ICategoryRepository _categoryRepo;

        public LocationController(ICountryRepository countryRepo, IMapper mapper, ILoggerTrace serviceLogger, ICategoryRepository categoryRepository)
        {
            _categoryRepo = categoryRepository;
            _countryRepo = countryRepo;
            _mapper = mapper;
            this.serviceLogger = serviceLogger;
        }


        // GET: LocationController
        public ActionResult Index()
        {
            return View();
        }

        // GET: LocationController/Details/5
        public ActionResult Details(int id)
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

                var listOfLocationsCategory =_mapper.Map<List<CategoryDTO>>(_categoryRepo.GetAllCategories());

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

        // POST: LocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection locationCollection)
        {
            try
            {
                //if (ModelState.IsValid)
                //{

                    if (locationCollection.Files.Count > 0)
                    {
                        var file = locationCollection.Files[0];
                    }
                

                //}
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LocationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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

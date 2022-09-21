using AutoMapper;
using LarksWebApp.DTOs;
using LarksWebApp.DTOs.DTO_Streets;
using LarksWebApp.Repository.CountryRepository;
using LarksWebApp.Repository.StreetRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers
{
    public class StreetController : Controller
    {

        private readonly ICountryRepository _countryRepo;
        private readonly IStreetRepository _streetRepo;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;
        public StreetController(ICountryRepository countryRepo, IStreetRepository streetRepo, IMapper mapper, ILoggerTrace loggerTrace)
        {
            _countryRepo = countryRepo;
            _streetRepo = streetRepo;
            _mapper = mapper;
            serviceLogger = loggerTrace;
        }
        // GET: StreetController
        public ActionResult Index()
        {

            return View();
        }

        // GET: StreetController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StreetController/Create
        public ActionResult Create()
        {

            try
            {
                var listofCountry = _countryRepo.GetAll();

                var listofCountryDTO = new List<CountryDTO>();

                listofCountryDTO = _mapper.Map<List<CountryDTO>>(listofCountry);

                var createStreetDTO = new Create_Street_DTO();

                createStreetDTO.ListofCountryDTO = listofCountryDTO;

                return View(createStreetDTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Street Controller getting the data" + ex.Message);

                return View();
            }
         
        }

        // POST: StreetController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: StreetController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StreetController/Edit/5
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

        // GET: StreetController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StreetController/Delete/5
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

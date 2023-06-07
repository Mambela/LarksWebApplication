using AutoMapper;
using LarksWebApp.DTOs;
using LarksWebApp.DTOs.DTO_Country;
using LarksWebApp.DTOs.DTO_Provinces;
using LarksWebApp.Models;
using LarksWebApp.Repository.CountryRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers
{

    [Authorize]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;

        public CountryController(ICountryRepository countryRepository, IMapper mapper, ILoggerTrace serviceLogger)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            this.serviceLogger = serviceLogger;

        }
        // GET: CountryController
        public ActionResult Index()
        {
            var countryList = _countryRepository.GetAll();

            var countryListDTO = new List<CountryDTO>();

            foreach (var country in countryList)
            {
                countryListDTO.Add(_mapper.Map<CountryDTO>(country));

            }

            return View(countryListDTO);
        }

        // GET: CountryController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var countryObj = _countryRepository.GetCountry(id);

                if (countryObj == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                var countryDTO = _mapper.Map<CountryDTO>(countryObj);

                var provincesListDTO = new List<ProvinceDTO>();

                foreach (var province in countryObj.Provinces)
                {
                    provincesListDTO.Add(_mapper.Map<ProvinceDTO>(province));
                }

                countryDTO.Provinces = provincesListDTO;

                TempData["countryID"]= countryDTO.Id;

                return View(countryDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            
        }

        // GET: CountryController/Create
        public ActionResult Create()
        {
            return View();
        }



        // POST: CountryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                CountryModel countryModel = new CountryModel();

                countryModel.Designation = collection["Designation"];

                if (_countryRepository.CreateCountry(countryModel) != null)
                {
                    return RedirectToAction(nameof(Index),new {msg=$"Country {countryModel.Designation} saved successfully! "});
                    
                }
                return RedirectToAction(nameof(Index));
                
            }
            catch(Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return View();
            }
        }

        // GET: CountryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CountryController/Edit/5
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

        // GET: CountryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CountryController/Delete/5
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

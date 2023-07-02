using AutoMapper;
using LarksWebApp.DTOs;
using LarksWebApp.DTOs.DTO_Addresses;
using LarksWebApp.DTOs.DTO_Country;
using LarksWebApp.DTOs.DTO_Streets;
using LarksWebApp.Repository.AddressRepository.BuildingTypeRepository;
using LarksWebApp.Repository.CountryRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers
{
    [Authorize]
    public class AddressesController : Controller
    {
        private readonly ICountryRepository _countryRepo;
        private readonly IBuildingTypeRepository _buildingType;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;
        public AddressesController(ICountryRepository countryRepository,IMapper mapper,ILoggerTrace loggerTrace,IBuildingTypeRepository buildingType )
        {
            serviceLogger = loggerTrace;
            _mapper = mapper;
            _countryRepo = countryRepository;
            _buildingType = buildingType;
        }
        // GET: AddressesController
        public ActionResult Index()
        {

            try
            {

                var countryList = _countryRepo.GetAll();

                var countryListDTO = new List<CountryDTO>();

                foreach (var country in countryList)
                {
                    countryListDTO.Add(_mapper.Map<CountryDTO>(country));

                }

                var viewCountryListDTO = new ViewListCountryDTO();

                viewCountryListDTO.ListofCountryDTO = countryListDTO;

                return View(viewCountryListDTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Error while attempting to fetch countries for address" + ex.Message);


                return BadRequest();
            }
             
        }

        // GET: AddressesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AddressesController/Create
        public ActionResult Create()
        {
            try
            {
                var listofCountry = _countryRepo.GetAll();

                var listofCountryDTO = new List<CountryDTO>();

                listofCountryDTO = _mapper.Map<List<CountryDTO>>(listofCountry);

                var createAdressDTO = new Create_Addresses_DTO();

                var buldingTypeDTOList = _mapper.Map<IList<Create_BuildingType_DTO>>(_buildingType.GetAll("fr"));


                createAdressDTO.ListofCountryDTO = listofCountryDTO;
                createAdressDTO.ListOfBuildingType_DTO = buldingTypeDTOList;

                return View(createAdressDTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Address Controller getting the countries" + ex.Message);

                return View();
            }
        }

        // POST: AddressesController/Create
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

        // GET: AddressesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AddressesController/Edit/5
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

        // GET: AddressesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AddressesController/Delete/5
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

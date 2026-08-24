using AutoMapper;
using LarksWebApp.DTOs.DTO_City;
using LarksWebApp.DTOs.DTO_Districts;
using LarksWebApp.DTOs.DTO_Municipalities;
using LarksWebApp.DTOs.DTO_Provinces;
using LarksWebApp.Models;
using LarksWebApp.Repository.CountryRepository;
using LarksWebApp.Repository.DistrictRepository;
using LarksWebApp.Repository.SuburbRepository;
using LarksWebApp.Repository.MunicipalityRepository;
using LarksWebApp.Repository.ProviceRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers
{
    [Authorize]
    public class ProvinceController : Controller
    {
        private readonly IProvinceRepository _provinceRepository;
        private readonly ICountryRepository countryRepository;
        private readonly IMunicipalityRepository _municipalityRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly ISuburbRepository _suburbRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;

        // GET: ProvinceController
        public ProvinceController(IProvinceRepository provinceRepository, IMapper mapper, ILoggerTrace logger,
            ICountryRepository _countryRepo, IMunicipalityRepository municipalityRepository,
            IDistrictRepository districtRepository, ISuburbRepository suburbRepository)
        {
            countryRepository = _countryRepo;
            _provinceRepository = provinceRepository;
            _municipalityRepository = municipalityRepository;
            _districtRepository = districtRepository;
            _suburbRepository = suburbRepository;
            _mapper = mapper;
            serviceLogger = logger;
        }
        public ActionResult Index()
        {
            var provinceListObj = _provinceRepository.GetAllProvinces();

            var provinceListDTO = _mapper.Map<List<ProvinceDTO>>(provinceListObj);

            return View(provinceListDTO);
        }

        // GET: ProvinceController/Details/5
        public ActionResult Details(Guid id)
        {

            try
            {
                var provinceObj = _provinceRepository.GetProvinceModel(id);

                if (provinceObj == null)
                {
                 
                    return View();
                }

                var provinceDTO = _mapper.Map<ProvinceDTO>(provinceObj);

                var citiesDTO = new List<CitiesDTO>();
                var disctrictDTO = new List<DistrictDTO>();
                var municipalitiesDTO = new List<MunicipalityDTO>();

               


                TempData["provinceID"] = provinceDTO.Id;

                return View(provinceDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        // GET: ProvinceController/AddLocationDetails/5
        public ActionResult AddLocationDetails(Guid provinceId)
        {
            try
            {
                var provinceObj = _provinceRepository.GetProvinceModel(provinceId);

                if (provinceObj == null)
                {
                    return RedirectToAction(nameof(Details), new { id = provinceId });
                }

                var provinceDTO = _mapper.Map<ProvinceDTO>(provinceObj);

                return View(provinceDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Add location details " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST: ProvinceController/CreateMunicipality
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateMunicipality(Guid provinceId, string designation)
        {
            try
            {
                var municipality = new MunicipalityModel()
                {
                    ProvinceId = provinceId,
                    Designation = designation
                };

                await _municipalityRepository.CreateMunicipalityModel(municipality);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Create municipality " + ex.Message);
            }

            return RedirectToAction(nameof(AddLocationDetails), new { provinceId });
        }

        // POST: ProvinceController/CreateDistrict
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDistrict(Guid provinceId, string designation)
        {
            try
            {
                var district = new DistrictModel()
                {
                    ProvinceId = provinceId,
                    Designation = designation
                };

                await _districtRepository.CreateDistrictModel(district);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Create district " + ex.Message);
            }

            return RedirectToAction(nameof(AddLocationDetails), new { provinceId });
        }

        // POST: ProvinceController/CreateSuburb
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateSuburb(Guid provinceId, Guid cityId, Guid districtId, Guid municipalityId, string designation)
        {
            try
            {
                var suburb = new SuburbModel()
                {
                    CityId = cityId,
                    DistrictId = districtId,
                    MunicipalityId = municipalityId,
                    Designation = designation
                };

                await _suburbRepository.CreateSuburb(suburb);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Create suburb " + ex.Message);
            }

            return RedirectToAction(nameof(AddLocationDetails), new { provinceId });
        }

        public ActionResult CountryProvinces(Guid CountryId)
        {
            try
            {
                var provincesObj = _provinceRepository.GetProvincesInCountry(CountryId);

                var proviceDTOList = new List<ProvinceDTO>();

                proviceDTOList = _mapper.Map<List<ProvinceDTO>>(provincesObj);

                return Ok(proviceDTOList);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Get Provinces in Country " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }


        // GET: ProvinceController/Create
        public ActionResult Create()
        {
            

            return View();
        }

        // POST: ProvinceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Invalid Model State");
                }

                var provinceModel = new ProvinceModel
                {
                    CountryId = Guid.Parse(TempData["countryID"].ToString()),
                    Designation = collection["Designation"]
                };

                if (_provinceRepository.CreateProvinceModel(provinceModel) != null)
                {

                    var provinceObj = _provinceRepository.GetProvinceModel(Guid.Parse(TempData["countryID"].ToString()));
                    var provinceDTO = _mapper.Map<ProvinceDTO>(provinceObj);

                    //return RedirectToAction(nameof(Index), new { msg = $"Province {provinceModel.Designation} saved successfully! " });

                    return RedirectToAction("Index", "Country","Province saved successfully");
                }


                return RedirectToAction(nameof(Index), new {msg =$"Error while saving Province {provinceModel.Designation} "});
            }
            catch(Exception ex)
            {

                serviceLogger.LogError("Create a province " + ex.Message);
                return View();
            }
        }

        // GET: ProvinceController/Edit/5
        public ActionResult Edit(Guid id)
        {

            try
            {
                var provinceObj = _provinceRepository.GetProvinceModel(id);

                if (provinceObj == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                var provinceDTO  = _mapper.Map<Create_Province_DTO>(provinceObj);

                return View(provinceDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return View();
            }

        }

        // POST: ProvinceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection provinceCollection)
        {
            try
            {
                if (!Guid.TryParse(provinceCollection["CountryId"], out Guid countryId) || string.IsNullOrWhiteSpace(provinceCollection["Designation"]))
                {
                    return View();
                }

                var provinceModel = new ProvinceModel()
                {
                    Designation = provinceCollection["Designation"],
                    Id = id,
                    CountryId = countryId
                };

                if (!_provinceRepository.UpdateProvince(provinceModel))
                {
                    return View();
                }


                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return View();
            }
        }

        // GET: ProvinceController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var provinceObj = _provinceRepository.GetProvinceModel(id);

            if (provinceObj == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return View(_mapper.Map<ProvinceDTO>(provinceObj));
        }

        // POST: ProvinceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                var provinceObj = _provinceRepository.GetProvinceModel(id);

                if (provinceObj != null)
                {
                    _provinceRepository.DeleteProvince(provinceObj);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return View();
            }
        }
    }
}

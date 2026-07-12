using AutoMapper;
using LarksWebApp.DTOs.DTO_City;
using LarksWebApp.DTOs.DTO_Districts;
using LarksWebApp.DTOs.DTO_Municipalities;
using LarksWebApp.DTOs.DTO_Provinces;
using LarksWebApp.Models;
using LarksWebApp.Repository.CountryRepository;
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
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;

        // GET: ProvinceController
        public ProvinceController(IProvinceRepository provinceRepository, IMapper mapper, ILoggerTrace logger,ICountryRepository _countryRepo)
        {
            countryRepository = _countryRepo;
            _provinceRepository = provinceRepository;
            _mapper = mapper;
            serviceLogger = logger;
        }
        public ActionResult Index()
        {
            return View();
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
                var provinceDTO  = _mapper.Map<ProvinceDTO>(provinceObj);

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
                if (id == null || provinceCollection["designation"].ToString() != null)
                {
                    return View();    
                }


                var provinceModel = new ProvinceModel()
                {
                    Designation = provinceCollection["designation"],
                    Id = id,
                    CountryId = Guid.Parse(provinceCollection["countryID"])
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
            return View();
        }

        // POST: ProvinceController/Delete/5
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

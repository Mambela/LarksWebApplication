using AutoMapper;
using LarksWebApp.DTOs.DTO_City;
using LarksWebApp.DTOs.DTO_Districts;
using LarksWebApp.DTOs.DTO_Municipalities;
using LarksWebApp.DTOs.DTO_Provinces;
using LarksWebApp.Models;
using LarksWebApp.Repository.ProviceRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers
{
    public class ProvinceController : Controller
    {
        private readonly IProvinceRepository _provinceRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;

        // GET: ProvinceController
        public ProvinceController(IProvinceRepository provinceRepository, IMapper mapper, ILoggerTrace logger)
        {
            _provinceRepository = provinceRepository;
            _mapper = mapper;
            serviceLogger = logger;
        }
        public ActionResult Index()
        {
            return View();
        }

        // GET: ProvinceController/Details/5
        public ActionResult Details(int id)
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
                    CountryId = Convert.ToInt32(TempData["countryID"].ToString()),
                    Designation = collection["Designation"]
                };

                if (_provinceRepository.CreateProvinceModel(provinceModel) != null)
                {
                    return RedirectToAction(nameof(Index), new { msg = $"Province {provinceModel.Designation} saved successfully! " });
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
        public ActionResult Edit(int id)
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
        public ActionResult Edit(int id, IFormCollection provinceCollection)
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
                    CountryId = Convert.ToInt32(provinceCollection["countryID"])
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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProvinceController/Delete/5
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

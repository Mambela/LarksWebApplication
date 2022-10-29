using AutoMapper;
using LarksWebApp.DTOs.DTO_Municipalities;
using LarksWebApp.Models;
using LarksWebApp.Repository.CityRepository;
using LarksWebApp.Repository.MunicipalityRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers
{

    [Authorize]
    public class MunicipalitiesController : Controller
    {
        private readonly IMunicipalityRepository _municipalityRepo;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;
        public MunicipalitiesController(IMunicipalityRepository municipalityRepo, IMapper mapper, ILoggerTrace serviceLogger)
        {
            _municipalityRepo = municipalityRepo;
            _mapper = mapper;
            this.serviceLogger = serviceLogger;
        }



        // GET: MunicipalitiesController
        public ActionResult Index(int provinceId)
        {

            try
            {
                var municitypalitiesListObj = _municipalityRepo.GetMunicipalityModels(provinceId);

                var municipalityListDTO = new List<MunicipalityDTO>();

                for (int i = 0; i < municitypalitiesListObj.Count; i++)
                {
                    municipalityListDTO.Add(_mapper.Map<MunicipalityDTO>(municitypalitiesListObj[i]));
                }

                TempData["provinceID"] = provinceId;

                return View(municipalityListDTO);
            }

            catch (Exception ex)
            {

                serviceLogger.LogError(ex.Message);
                throw;
            }
            return View();
        }

        // GET: MunicipalitiesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MunicipalitiesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MunicipalitiesController/Create
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

                var municipality = new MunicipalityModel()
                {
                    ProvinceId = Convert.ToInt32(TempData["provinceID"].ToString()),
                    Designation = collection["Designation"]
                };

                if (_municipalityRepo.CreateMunicipalityModel(municipality) != null)
                {
                    return RedirectToAction(nameof(Index), new { msg = $"Municipality {municipality.Designation} saved successfully! " });
                }


                return RedirectToAction(nameof(Index), new { msg = $"Error while saving municipality {municipality.Designation} " });

            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return View();
            }
        }

        // GET: MunicipalitiesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MunicipalitiesController/Edit/5
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

        // GET: MunicipalitiesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MunicipalitiesController/Delete/5
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

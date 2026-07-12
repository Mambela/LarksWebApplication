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
        public ActionResult Index(Guid provinceId)
        {

            try
            {
                var municitypalitiesListObj = _municipalityRepo.GetMunicipalityModels(provinceId);

                var municipalityListDTO = new List<MunicipalityDTO>();

                for (int i = 0; i < municitypalitiesListObj.Count; i++)
                {
                    municipalityListDTO.Add(_mapper.Map<MunicipalityDTO>(municitypalitiesListObj[i]));
                }

                var municipalityObj = new ViewMunicipalityCreateDTO();
                
                municipalityObj.ProviceId = provinceId;
                municipalityObj.MunicipalityDTO = municipalityListDTO;


                TempData["provinceID"] = provinceId;

                return View(municipalityObj);
            }

            catch (Exception ex)
            {

                serviceLogger.LogError(ex.Message);
                throw;
            }
            return View();
        }

        // GET: MunicipalitiesController/Details/5
        public ActionResult Details(Guid id)
        {

            

            return View();
        }

        // GET: MunicipalitiesController/Create
        public ActionResult Create(Guid provinceId)
        {
            //Try Retrieving the Province ID FROM HERE. 

            ViewData["provinceId"] = provinceId;

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
                    ProvinceId = Guid.Parse(collection["provinceId"]),
                    Designation = collection["Designation"]
                };

                if (_municipalityRepo.CreateMunicipalityModel(municipality) != null)
                {

                    return RedirectToAction(nameof(Index), "Municipalities", new {provinceId= Guid.Parse(collection["provinceId"])});
                    //return RedirectToRoute("Municipalities?provinceId=" + Guid.Parse(collection["provinceId"]));
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
        public ActionResult Edit(Guid id)
        {
            return View();
        }

        // POST: MunicipalitiesController/Edit/5
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

        // GET: MunicipalitiesController/Delete/5
        public ActionResult Delete(Guid id)
        {
            return View();
        }

        // POST: MunicipalitiesController/Delete/5
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

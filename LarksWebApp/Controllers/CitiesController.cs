using AutoMapper;
using LarksWebApp.DTOs.DTO_City;
using LarksWebApp.Models;
using LarksWebApp.Repository.CityRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
 

namespace LarksWebApp.Controllers
{
    [Authorize]
    public class CitiesController : Controller
    {
        private readonly ICityRepository _citytRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;

        public CitiesController(ICityRepository citytRepository, IMapper mapper, ILoggerTrace serviceLogger)
        {
            _citytRepository = citytRepository;
            _mapper = mapper;
            this.serviceLogger = serviceLogger;
        }


    
        // GET: CityController
        public ActionResult Index(Guid provinceId)
        {
            try
            {
                var citiesObjList = _citytRepository.GetAllCitiesInProvince(provinceId);

                var cityDTOlist = new List<CitiesDTO>();

                for (int i = 0; i < citiesObjList.Count; i++)
                {
                    cityDTOlist.Add(_mapper.Map<CitiesDTO>(citiesObjList[i]));
                }

                TempData["provinceID"] = provinceId;

                return View(cityDTOlist);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return View();
                throw;
            }
            
        }

        // GET: CityController/Details/5
        public ActionResult Details(Guid id)
        {


            return View();
        }

        // GET: CityController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CityController/Create
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

                var cityModel = new CityModel()
                {
                    ProvinceId = Guid.Parse(TempData["provinceID"].ToString()),
                    Designation = collection["Designation"]
                };

                if (_citytRepository.CreateCity(cityModel) != null)
                {
                    return RedirectToAction(nameof(Index), new { msg = $"City {cityModel.Designation} saved successfully! " });
                }


                return RedirectToAction(nameof(Index), new { msg = $"Error while saving City {cityModel.Designation} " });


            }
            catch(Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return View();
            }
        }

        // GET: CityController/Edit/5
        public ActionResult Edit(Guid id)
        {
            return View();
        }

        // POST: CityController/Edit/5
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

        // GET: CityController/Delete/5
        public ActionResult Delete(Guid id)
        {
            return View();
        }

        // POST: CityController/Delete/5
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

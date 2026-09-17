using AutoMapper;
using LarksWebApp.DTOs.DTO_City;
using LarksWebApp.Models;
using LarksWebApp.Repository.CityRepository;
using LarksWebApp.Repository.ProviceRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace LarksWebApp.Controllers
{
    // [Authorize]
    public class CitiesController : Controller
    {
        private readonly ICityRepository _citytRepository;
        private readonly IProvinceRepository _provinceRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;

        public CitiesController(ICityRepository citytRepository, IMapper mapper, ILoggerTrace serviceLogger, IProvinceRepository provinceRepository)
        {
            _citytRepository = citytRepository;
            _provinceRepository = provinceRepository;
            _mapper = mapper;
            this.serviceLogger = serviceLogger;
        }



        // GET: CityController
        public ActionResult Index(Guid? provinceId)
        {
            try
            {
                var cityDTOlist = new List<CitiesDTO>();

                if (provinceId.HasValue && provinceId.Value != Guid.Empty)
                {
                    var citiesObjList = _citytRepository.GetAllCitiesInProvince(provinceId.Value);

                    for (int i = 0; i < citiesObjList.Count; i++)
                    {
                        cityDTOlist.Add(_mapper.Map<CitiesDTO>(citiesObjList[i]));
                    }

                    TempData["provinceID"] = provinceId.Value;
                }
                else
                {
                    foreach (var city in _citytRepository.GetAllCities())
                    {
                        var dto = _mapper.Map<CitiesDTO>(city);
                        dto.ProvinceName = city.Province?.Designation;
                        cityDTOlist.Add(dto);
                    }
                }

                ViewData["provinceId"] = provinceId ?? Guid.Empty;

                return View(cityDTOlist);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return View();
            }

        }

        // GET: CityController/Details/5
        public ActionResult Details(Guid id)
        {


            return View();
        }

        // GET: CityController/Create
        public ActionResult Create(Guid? provinceId)
        {
            ViewData["provinces"] = new SelectList(_provinceRepository.GetAllProvinces(), "Id", "Designation", provinceId);

            return View(new Create_City_DTO { ProvinceId = provinceId ?? Guid.Empty });
        }

        // POST: CityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    throw new Exception("Invalid Model State");
                }

                var provinceId = Guid.Parse(collection["provinceId"]);

                var cityModel = new CityModel()
                {
                    ProvinceId = provinceId,
                    Designation = collection["Designation"]
                };

                if (await _citytRepository.CreateCity(cityModel) != null)
                {
                    return RedirectToAction(nameof(Index), new { provinceId });
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
            var cityObj = _citytRepository.GetCity(id);

            if (cityObj == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            ViewData["provinces"] = new SelectList(_provinceRepository.GetAllProvinces(), "Id", "Designation", cityObj.ProvinceId);

            return View(_mapper.Map<Create_City_DTO>(cityObj));
        }

        // POST: CityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                var cityObj = _citytRepository.GetCity(id);

                if (cityObj == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                if (!Guid.TryParse(collection["provinceId"], out Guid provinceId) || string.IsNullOrWhiteSpace(collection["Designation"]))
                {
                    ViewData["provinces"] = new SelectList(_provinceRepository.GetAllProvinces(), "Id", "Designation", cityObj.ProvinceId);
                    return View(_mapper.Map<Create_City_DTO>(cityObj));
                }

                cityObj.Designation = collection["Designation"];
                cityObj.ProvinceId = provinceId;

                _citytRepository.UpdateCity(cityObj);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return View();
            }
        }

        // GET: CityController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var cityObj = _citytRepository.GetCity(id);

            if (cityObj == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return View(_mapper.Map<CitiesDTO>(cityObj));
        }

        // POST: CityController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                var cityObj = _citytRepository.GetCity(id);

                if (cityObj != null)
                {
                    _citytRepository.DeleteCity(cityObj);
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

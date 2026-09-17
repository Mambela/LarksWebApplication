 using AutoMapper;
using LarksWebApp.DTOs.DTO_Municipalities;
using LarksWebApp.DTOs.DTO_Provinces;
using LarksWebApp.Models;
using LarksWebApp.Repository.CityRepository;
using LarksWebApp.Repository.SuburbRepository;
using LarksWebApp.Repository.MunicipalityRepository;
using LarksWebApp.Repository.ProviceRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LarksWebApp.Controllers
{

    // [Authorize]
    public class MunicipalitiesController : Controller
    {
        private readonly IMunicipalityRepository _municipalityRepo;
        private readonly IProvinceRepository _provinceRepository;
        private readonly ISuburbRepository _suburbRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;
        public MunicipalitiesController(IMunicipalityRepository municipalityRepo, IMapper mapper, ILoggerTrace serviceLogger,
            IProvinceRepository provinceRepository, ISuburbRepository suburbRepository)
        {
            _municipalityRepo = municipalityRepo;
            _provinceRepository = provinceRepository;
            _suburbRepository = suburbRepository;
            _mapper = mapper;
            this.serviceLogger = serviceLogger;
        }



        // GET: MunicipalitiesController
        public ActionResult Index(Guid? provinceId)
        {

            try
            {
                var municipalityListDTO = new List<MunicipalityDTO>();

                if (provinceId.HasValue && provinceId.Value != Guid.Empty)
                {
                    var municitypalitiesListObj = _municipalityRepo.GetMunicipalityModels(provinceId.Value);

                    for (int i = 0; i < municitypalitiesListObj.Count; i++)
                    {
                        municipalityListDTO.Add(_mapper.Map<MunicipalityDTO>(municitypalitiesListObj[i]));
                    }

                    TempData["provinceID"] = provinceId.Value;
                }
                else
                {
                    var allMunicipalities = _municipalityRepo.GetAllMunicipalities();

                    foreach (var municipality in allMunicipalities)
                    {
                        var dto = _mapper.Map<MunicipalityDTO>(municipality);
                        dto.ProvinceName = municipality.Province?.Designation;
                        municipalityListDTO.Add(dto);
                    }
                }

                var municipalityObj = new ViewMunicipalityCreateDTO();

                municipalityObj.ProviceId = provinceId ?? Guid.Empty;
                municipalityObj.MunicipalityDTO = municipalityListDTO;

                return View(municipalityObj);
            }

            catch (Exception ex)
            {

                serviceLogger.LogError(ex.Message);
                throw;
            }
        }

        // GET: MunicipalitiesController/Details/5
        public ActionResult Details(Guid id)
        {

            

            return View();
        }

        // GET: MunicipalitiesController/Create
        public ActionResult Create(Guid? provinceId)
        {
            ViewData["provinceId"] = provinceId ?? Guid.Empty;
            ViewData["provinces"] = new SelectList(_provinceRepository.GetAllProvinces(), "Id", "Designation", provinceId);

            return View();
        }

        // POST: MunicipalitiesController/Create
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

                var municipality = new MunicipalityModel()
                {
                    ProvinceId = provinceId,
                    Designation = collection["Designation"]
                };

                if (await _municipalityRepo.CreateMunicipalityModel(municipality) != null)
                {
                    return RedirectToAction(nameof(Index), "Municipalities", new { provinceId });
                }


                return RedirectToAction(nameof(Index), new { msg = $"Error while saving municipality {municipality.Designation} " });

            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return View();
            }
        }

        // GET: MunicipalitiesController/CreateSuburb
        public ActionResult CreateSuburb(Guid provinceId)
        {
            try
            {
                var provinceObj = _provinceRepository.GetProvinceModel(provinceId);

                if (provinceObj == null)
                {
                    return RedirectToAction(nameof(Index), new { provinceId });
                }

                var provinceDTO = _mapper.Map<ProvinceDTO>(provinceObj);

                return View(provinceDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Create suburb " + ex.Message);
                return RedirectToAction(nameof(Index), new { provinceId });
            }
        }

        // POST: MunicipalitiesController/CreateSuburb
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateSuburb(Guid provinceId, Guid cityId, Guid districtId, Guid municipalityId, string designation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Invalid Model State");
                }

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

            return RedirectToAction(nameof(Index), new { provinceId });
        }

        // GET: MunicipalitiesController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var municipalityObj = _municipalityRepo.GetMuncipality(id);

            if (municipalityObj == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            ViewData["provinces"] = new SelectList(_provinceRepository.GetAllProvinces(), "Id", "Designation", municipalityObj.ProvinceId);

            return View(_mapper.Map<Create_Municipality_DTO>(municipalityObj));
        }

        // POST: MunicipalitiesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                var municipalityObj = _municipalityRepo.GetMuncipality(id);

                if (municipalityObj == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                if (!Guid.TryParse(collection["provinceId"], out Guid provinceId) || string.IsNullOrWhiteSpace(collection["Designation"]))
                {
                    ViewData["provinces"] = new SelectList(_provinceRepository.GetAllProvinces(), "Id", "Designation", municipalityObj.ProvinceId);
                    return View(_mapper.Map<Create_Municipality_DTO>(municipalityObj));
                }

                municipalityObj.Designation = collection["Designation"];
                municipalityObj.ProvinceId = provinceId;

                _municipalityRepo.UpdateMunicipality(municipalityObj);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return View();
            }
        }

        // GET: MunicipalitiesController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var municipalityObj = _municipalityRepo.GetMuncipality(id);

            if (municipalityObj == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return View(_mapper.Map<MunicipalityDTO>(municipalityObj));
        }

        // POST: MunicipalitiesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                var municipalityObj = _municipalityRepo.GetMuncipality(id);

                if (municipalityObj != null)
                {
                    _municipalityRepo.DeleteMunicipality(municipalityObj);
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

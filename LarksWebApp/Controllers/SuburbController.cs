using AutoMapper;
using LarksWebApp.DTOs.DTO_Suburbs;
using LarksWebApp.Models;
using LarksWebApp.Repository.CityRepository;
using LarksWebApp.Repository.CountryRepository;
using LarksWebApp.Repository.MunicipalityRepository;
using LarksWebApp.Repository.SuburbRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LarksWebApp.Controllers
{
    [Authorize]
    public class SuburbController : Controller
    {
        private readonly ISuburbRepository _suburbRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMunicipalityRepository _municipalityRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;


        public SuburbController(ISuburbRepository suburbRepository, ICountryRepository countryRepository,
            IMunicipalityRepository municipalityRepository, ICityRepository cityRepository,
            IMapper mapper, ILoggerTrace serviceLogger)
        {
            _suburbRepository = suburbRepository;
            _countryRepository = countryRepository;
            _municipalityRepository = municipalityRepository;
            _cityRepository = cityRepository;
            _mapper = mapper;
            this.serviceLogger = serviceLogger;
        }


        // GET: SuburbController
        public ActionResult Index(Guid? provinId)
        {
            try
            {
                var suburbObjList = provinId.HasValue && provinId.Value != Guid.Empty
                    ? _suburbRepository.GetSuburbsInMunicipality(provinId.Value)
                    : _suburbRepository.GetSuburbs();

                var suburbDTOList = new List<SuburbDTO>();

                foreach (var suburb in suburbObjList)
                {
                    var dto = _mapper.Map<SuburbDTO>(suburb);
                    dto.CityName = suburb.City?.Designation;
                    dto.DistrictName = suburb.District?.Designation;
                    dto.MunicipalityName = suburb.Municipality?.Designation;
                    dto.ProvinceName = suburb.Municipality?.Province?.Designation;
                    suburbDTOList.Add(dto);
                }

                return View(suburbDTOList);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return View(new List<SuburbDTO>());
            }
        }

        // GET: SuburbController/Details/5
        public ActionResult Details(Guid id)
        {
            return View();
        }

        // GET: SuburbController/Create
        public ActionResult Create()
        {
            ViewData["countries"] = new SelectList(_countryRepository.GetAll(), "Id", "Designation");

            return View();
        }

        // POST: SuburbController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(IFormCollection suburb)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Invalid Model State");
                }

                var suburbObj = new SuburbModel()
                {
                    CityId = Guid.Parse(suburb["newCityId"]),

                    Designation = suburb["Designation"]
                };


                if(Guid.TryParse(suburb["newMunicipalityId"], out Guid municipalityId))
                {
                    suburbObj.MunicipalityId = municipalityId;
                }

                if(Guid.TryParse(suburb["newDistrictId"], out Guid districtId))
                {
                    suburbObj.DistrictId = districtId;
                }


                /**
                MunicipalityId = Guid.Parse(suburb["newMunicipalityId"]),
                    DistrictId = Guid.Parse(suburb["newDistrictId"]),*/

                var existingSuburb = _suburbRepository.VerifySuburbExist(suburbObj.Designation, suburbObj.MunicipalityId, suburbObj.CityId, suburbObj.DistrictId);

                if (existingSuburb == null)
                {
                    await _suburbRepository.CreateSuburb(suburbObj);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Create a new Suburb " + ex.Message);
                return RedirectToAction(nameof(Create));
            }
        }

        // GET: SuburbController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var suburbObj = _suburbRepository.GetSuburb(id);

            if (suburbObj == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            ViewData["municipalities"] = new SelectList(_municipalityRepository.GetAllMunicipalities(), "Id", "Designation", suburbObj.MunicipalityId);
            ViewData["cities"] = new SelectList(_cityRepository.GetAllCities(), "Id", "Designation", suburbObj.CityId);

            return View(_mapper.Map<SuburbDTO>(suburbObj));
        }

        public ActionResult sectorSuburb(string sector, Guid id)
        {

            try
            {
                var suburbObjList = _suburbRepository.GetSuburbs(sector, id);

                var suburbDTOList = new List<SuburbDTO>();

                foreach (var suburb in suburbObjList)
                {
                    suburbDTOList.Add(_mapper.Map<SuburbDTO>(suburb));
                }


                return Ok(suburbDTOList);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Suburb Sector " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while trying to retrieve suburbs ");
            }

        }



        // POST: SuburbController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                var suburbObj = _suburbRepository.GetSuburb(id);

                if (suburbObj == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                suburbObj.Designation = collection["Designation"];

                if (Guid.TryParse(collection["MunicipalityId"], out Guid municipalityId))
                {
                    suburbObj.MunicipalityId = municipalityId;
                }

                if (Guid.TryParse(collection["CityId"], out Guid cityId))
                {
                    suburbObj.CityId = cityId;
                }

                _suburbRepository.UpdateSuburb(suburbObj);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Edit suburb " + ex.Message);
                return View();
            }
        }

        // GET: SuburbController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var suburbObj = _suburbRepository.GetSuburb(id);

            if (suburbObj == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return View(_mapper.Map<SuburbDTO>(suburbObj));
        }

        // POST: SuburbController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                var suburbObj = _suburbRepository.GetSuburb(id);

                if (suburbObj != null)
                {
                    _suburbRepository.DeleteSuburb(suburbObj);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Delete suburb " + ex.Message);
                return View();
            }
        }
    }
}

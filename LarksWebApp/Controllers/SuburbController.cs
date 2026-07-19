using AutoMapper;
using LarksWebApp.DTOs.DTO_Neighbourhoods;
using LarksWebApp.Models;
using LarksWebApp.Repository.CountryRepository;
using LarksWebApp.Repository.HoodRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LarksWebApp.Controllers
{
    [Authorize]
    public class NeighbourhoodController : Controller
    {
        private readonly ISuburbRepository _hoodRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;


        public NeighbourhoodController(ISuburbRepository hoodRepository, ICountryRepository countryRepository, IMapper mapper, ILoggerTrace serviceLogger)
        {
            _hoodRepository = hoodRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
            this.serviceLogger = serviceLogger;
        }


        // GET: NeighbourhoodController
        public ActionResult Index(Guid? provinId)
        {
            try
            {
                var neighbourhoodObjList = provinId.HasValue && provinId.Value != Guid.Empty
                    ? _hoodRepository.GetNeighbourhoodInMunicipality(provinId.Value)
                    : _hoodRepository.GetNeighbourhoodModels();

                var neighbourhoodDTOList = new List<SuburbDTO>();

                foreach (var neighbourhood in neighbourhoodObjList)
                {
                    var dto = _mapper.Map<SuburbDTO>(neighbourhood);
                    dto.CityName = neighbourhood.City?.Designation;
                    dto.DistrictName = neighbourhood.District?.Designation;
                    dto.MunicipalityName = neighbourhood.Municipality?.Designation;
                    dto.ProvinceName = neighbourhood.Municipality?.Province?.Designation;
                    neighbourhoodDTOList.Add(dto);
                }

                return View(neighbourhoodDTOList);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return View(new List<SuburbDTO>());
            }
        }

        // GET: NeighbourhoodController/Details/5
        public ActionResult Details(Guid id)
        {
            return View();
        }

        // GET: NeighbourhoodController/Create
        public ActionResult Create()
        {
            ViewData["countries"] = new SelectList(_countryRepository.GetAll(), "Id", "Designation");

            return View();
        }

        // POST: NeighbourhoodController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(IFormCollection neighbourhood)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Invalid Model State");
                }

                var neighbourhoodObj = new SuburbModel()
                {
                    CityId = Guid.Parse(neighbourhood["newCityId"]),
                    
                    Designation = neighbourhood["Designation"]
                };


                if(Guid.TryParse(neighbourhood["newMunicipalityId"], out Guid municipalityId))
                {
                    neighbourhoodObj.MunicipalityId = municipalityId;
                }

                if(Guid.TryParse(neighbourhood["newDistrictId"], out Guid districtId))
                {
                    neighbourhoodObj.DistrictId = districtId;
                }
                

                /**
                MunicipalityId = Guid.Parse(neighbourhood["newMunicipalityId"]),
                    DistrictId = Guid.Parse(neighbourhood["newDistrictId"]),*/



                await _hoodRepository.CreateSuburb(neighbourhoodObj);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Create a new Neighbourhood " + ex.Message);
                return RedirectToAction(nameof(Create));
            }
        }

        // GET: NeighbourhoodController/Edit/5
        public ActionResult Edit(Guid id)
        {
            return View();
        }

        public ActionResult sectorNeighbourhood(string sector, Guid id)
        {

            try
            {
                var hoodObjList = _hoodRepository.GetNeighbourhoodModels(sector, id);

                var hoodDTOList = new List<SuburbDTO>();

                foreach (var hood in hoodObjList)
                {
                    hoodDTOList.Add(_mapper.Map<SuburbDTO>(hood));
                }


                return Ok(hoodDTOList);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Neighbourhood Sector " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while trying to retrieve neighbourhood ");
            }

        }



        // POST: NeighbourhoodController/Edit/5
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

        // GET: NeighbourhoodController/Delete/5
        public ActionResult Delete(Guid id)
        {
            return View();
        }

        // POST: NeighbourhoodController/Delete/5
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
